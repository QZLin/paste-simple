using PasteSimple.Helpers;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.Owin.Hosting;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Net;

namespace PasteSimple
{

    /// <summary>
    /// Mail Home Form of ClipSync
    /// </summary>
    public partial class PasteSimpleMainForm : Form
    {

        /// <summary>
        /// time interval for checking copying data
        /// </summary>
        public string mTime = DateTime.Now.ToLongTimeString();

        /// <summary>
        /// Global Helper Instance
        /// </summary>
        private readonly GlobalHelper globalHelper;

        /// <summary>
        /// Signal R Hub Proxy
        /// </summary>
        public IHubProxy _hub;

        /// <summary>
        /// signal R Disposable Hub object
        /// </summary>
        private IDisposable signalRDisposable { get; set; }

        bool isSignalRConnected = false;

        /// <summary>
        /// User Id
        /// </summary>
        public string uid = "";

        /// <summary>
        /// General Logger Target
        /// </summary>
        public Logger generaLogger = LogManager.GetLogger("GeneralLog");
        /// <summary>
        /// Copy History Logger Target
        /// </summary>
        public Logger copyHistoryLogger = LogManager.GetLogger("CopyHistory");

        /// <summary>
        /// Default Constructor
        /// </summary>
        internal PasteSimpleMainForm()
        {
            InitializeComponent();
            this.globalHelper = new GlobalHelper();
        }

        private void PasteSimpleMainForm_Load(object sender, EventArgs e)
        {
            //try
            //{

            this.LogWriter("Your ip is: " + globalHelper.GetMachineIpAddress());

            this.serverGroupBox.Show();
            this.serverAddressTextBox.Text = "*";

            if (File.Exists("last_uid"))
                this.connectUidTextBox.Text = File.ReadAllText("last_uid");
            else
            {
                this.connectUidTextBox.Text = new Random().Next(1000, 9999).ToString();
                File.WriteAllText("last_uid", this.connectUidTextBox.Text);
            }
            //auto start server and self login on start
            this.startServerButton.PerformClick();
            this.loginButton.PerformClick();

            //}
            //catch (Exception ex)
            //{
            //    this.LogWriter(ex.ToString());
            //    this.generaLogger.Error(ex);
            //}
        }

        /// <summary>
        /// Start Server Button Click
        /// </summary>
        private void StartServerButton_Click(object sender, EventArgs e)
        {
            this.startServerButton.Enabled = false;
            string url = "http://" + this.serverAddressTextBox.Text + ":" + this.serverPortTextBox.Text + "/";
            this.LogWriter(string.Format("Starting server on: {0}", url));
            this.LogWriter(string.Format("Test your server: {0}", url + "signalr/hubs"));
            this.LogWriter("You need to open a port in outbound rule of Windows FireWall. PORT: " + this.serverPortTextBox.Text);
            try
            {
                //var a = 0; var b = 10 / a; //test other exception
                signalRDisposable = WebApp.Start(url);
            }
            catch (System.Reflection.TargetInvocationException exception)
            {
                if (TryFindInnerException<HttpListenerException>(exception, out var httpListenerException))
                {
                    MessageBox.Show(httpListenerException.ToString());
                    this.startServerButton.Enabled = true;
                    return;
                }

                throw exception;
            }

            this.connectServerAddressTextBox.Text = globalHelper.GetMachineIpAddress();
            this.connectServerPortTextBox.Text = this.serverPortTextBox.Text;
            this.startServerButton.Enabled = false;

            this.OpenPortButton.PerformClick();
            /*}
            catch (System.Reflection.TargetInvocationException ex)
            {
                this.LogWriter("You need to run as administrator");
                this.LogWriter("Only server address localhost is allowed without administrator permissions");
                MessageBox.Show("You need to run as administrator", "Error");
                this.generaLogger.Error(ex);
                this.startServerButton.Enabled = true;
            }*/
        }

        /// <summary>
        /// Server Port text box key press event
        /// </summary>
        private void serverPortTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar) || (e.KeyChar == (char)Keys.Back)))
                e.Handled = true;
        }

        /// <summary>
        /// Connect To Server Port Key Press Event
        /// </summary>
        private void connectServerPortTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar) || (e.KeyChar == (char)Keys.Back)))
                e.Handled = true;
        }

        /// <summary>
        /// Connect Uid Key Press Event
        /// </summary>
        private void connectUidTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!(Char.IsDigit(e.KeyChar) || (e.KeyChar == (char)Keys.Back)))
                e.Handled = true;
        }

        /// <summary>
        /// Login Button Click
        /// </summary>
        private void LoginButton_Click(object sender, EventArgs e)
        {

            loginButton.Enabled = false;

            string serverAddress = this.connectServerAddressTextBox.Text;
            string serverPort = this.connectServerPortTextBox.Text;
            string uid = this.connectUidTextBox.Text;

            this.LogWriter("Connecting to server");

            IDictionary<string, string> keyValuePairs = new Dictionary<string, string>
            {
                { "uid", uid },
                { "platform", "windows" },
                { "device_id", globalHelper.GetMacAddress() }
            };

            var connection = new HubConnection("http://" + serverAddress + ":" + serverPort, keyValuePairs);
            this._hub = connection.CreateHubProxy(ConfigurationManager.AppSettings["hub_name"]);
            try
            {
                connection.Start().Wait();
            }
            catch (UriFormatException exception)
            {
                MessageBox.Show(exception.ToString());
                this.LogWriter("Warning: PasteSimple Server is not running!!!");
                //isSignalRConnected = false;
                loginButton.Enabled = true;
                return;
            }
            //isSignalRConnected = true;
            /*
            catch (Exception ex)
            {
                isSignalRConnected = false;
                loginButton.Enabled = true;
                this.LogWriter("Exception in connecting to SignalR Hub : " + ex.ToString());
                this.generaLogger.Error(ex);
            }*/
            this.LogWriter("Server connected, uid: " + uid);

            AddClipBoardListener();
            this._hub.On(ConfigurationManager.AppSettings["recieve_copied_text_signalr_method_name"], delegate (String data)
            {
                data = Uri.UnescapeDataString(data);
                if (data != null && data.Length > 0)
                {
                    // avoid retransfer setted text
                    lastSetText = data;
                    waitCopyLoop = true;

                    this.LogWriter("set: " + data);
                    ClipBoardHelper.SetText(data);
                }

            });

        }
        /// <summary>
        /// Get a inner exception
        /// </summary>
        /// <typeparam name="T">type of expected inner exception</typeparam>
        /// <param name="top">Top exception</param>
        /// <param name="foundException">Inner exception in top exception</param>
        /// <returns>Wether type of inner exception equals T</returns>
        public static bool TryFindInnerException<T>(Exception top, out T foundException) where T : Exception
        {
            if (top == null)
            {
                foundException = null;
                return false;
            }
            if (typeof(T) == top.GetType())
            {
                foundException = (T)top;
                return true;
            }

            return TryFindInnerException<T>(top.InnerException, out foundException);
        }



        /// <summary>
        /// Open Port Button Click
        /// </summary>
        private void OpenPortButton_Click(object sender, EventArgs e)
        {
            this.OpenPortButton.Enabled = false;
            int serverPort = Convert.ToInt32(this.serverPortTextBox.Text);

            this.LogWriter("Checking the status of inbound port: " + serverPort + " Please wait...");
            if (this.globalHelper.IsPortOpened(serverPort, "PasteSimple"))
            {
                this.LogWriter("Port " + serverPort + " is already open");
            }
            else
            {
                this.LogWriter("Opening the inbound port: " + serverPort + " Please wait...");
                if (this.globalHelper.OpenInboundFirewallPort(serverPort, "PasteSimple", serverPort.ToString()))
                {
                    this.LogWriter("Successfully opened the inbound port : " + serverPort);
                }
                else
                {
                    this.LogWriter("Failed to open the inbound port : " + serverPort);
                    this.LogWriter("Try running as administrator");
                    MessageBox.Show("Try running as administrator", "Error");
                }
            }
            this.OpenPortButton.Enabled = true;
            /*}
            catch (System.Reflection.TargetInvocationException ex)
            {
                this.LogWriter("You need to run as administrator");
                MessageBox.Show("You need to run as administrator", "Error");
                this.generaLogger.Error(ex);
            }
            catch (Exception ex)
            {
                this.OpenPortButton.Enabled = true;
                this.LogWriter(ex.ToString());
                this.generaLogger.Error(ex);
            }*/
        }

        /// <summary>
        /// Log Writer on Form
        /// </summary>
        /// <param name="log_text">string text to write</param>
        internal void LogWriter(string log_text)
        {

            if (consoleTextBox.InvokeRequired)
            {
                this.Invoke((Action)(() =>
                    LogWriter(log_text)
                ));
                return;
            }

            this.generaLogger.Info(log_text);

            consoleTextBox.AppendText(log_text + Environment.NewLine);
            consoleTextBox.SelectionStart = consoleTextBox.Text.Length;
            consoleTextBox.ScrollToCaret();

        }

        /// <summary>
        /// Adds ClipBoard Listener to the Window
        /// </summary>
        private void AddClipBoardListener()
        {
            //NativeMethods.SetParent(Handle, NativeMethods.HWND_MESSAGE);
            NativeMethods.AddClipboardFormatListener(Handle);
        }

        /// <summary>
        ///  WindProc for getting ClipBoard Data
        /// </summary>
        string lastSetText = "";
        bool waitCopyLoop = false;
        protected override void WndProc(ref Message m)
        {
            /*try
            {*/

            if (m.Msg == NativeMethods.WM_CLIPBOARDUPDATE)
            {

                IDataObject iData = Clipboard.GetDataObject();  // Clipboard's data


                if (iData.GetDataPresent(DataFormats.UnicodeText) || iData.GetDataPresent(DataFormats.Text))
                {
                    string copied_content = Clipboard.GetText();
                    if (copied_content != null && copied_content.Length > 0)
                    {
                        if (!string.Equals(this.lastSetText, copied_content) || !waitCopyLoop)
                        {
                            this.LogWriter("clip: " + copied_content);
                            var encoded = Uri.EscapeDataString(copied_content);
                            //byte[] byteArray = Encoding.UTF8.GetBytes(copied_content);
                            _hub.Invoke(ConfigurationManager.AppSettings["send_copied_text_signalr_method_name"],
                                encoded);
                        }
                        else
                            waitCopyLoop = false;
                    }
                }
                else if (iData.GetDataPresent(DataFormats.Bitmap))
                {
                    //Bitmap image = (Bitmap)iData.GetData(DataFormats.Bitmap);   // Clipboard image
                    //TODO handle image
                }
            }

            base.WndProc(ref m);
            /*}
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                this.generaLogger.Error(ex);
            }*/
        }


    }

    internal static class NativeMethods
    {
        // See http://msdn.microsoft.com/en-us/library/ms649021%28v=vs.85%29.aspx
        public const int WM_CLIPBOARDUPDATE = 0x031D;
        public static IntPtr HWND_MESSAGE = new IntPtr(-3);

        // See http://msdn.microsoft.com/en-us/library/ms632599%28VS.85%29.aspx#message_only
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AddClipboardFormatListener(IntPtr hwnd);

        // See http://msdn.microsoft.com/en-us/library/ms633541%28v=vs.85%29.aspx
        // See http://msdn.microsoft.com/en-us/library/ms649033%28VS.85%29.aspx
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
    }

}
