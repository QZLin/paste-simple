﻿using ClipSync.Helpers;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.Owin.Hosting;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ClipSync
{

    /// <summary>
    /// Mail Home Form of ClipSync
    /// </summary>
    public partial class ClipSyncControlForm : Form
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
        internal ClipSyncControlForm()
        {
            InitializeComponent();
            this.globalHelper = new GlobalHelper();
        }

        private void ClipSyncControlForm_Load(object sender, EventArgs e)
        {
            try
            {

                this.LogWriter("-------------------");
                this.LogWriter("Enter address and port then start server and wait.");
                this.LogWriter("Your ip is: " + globalHelper.GetMachineIpAddress());

                this.serverGroupBox.Show();
                this.serverAddressTextBox.Text = "*";
                //this.serverAddressTextBox.Text = "localhost";

                if (File.Exists("last_uid"))
                    this.connectUidTextBox.Text = File.ReadAllText("last_uid");
                else
                {
                    this.connectUidTextBox.Text = new Random().Next(1000, 9999).ToString();
                    File.WriteAllText("last_uid", this.connectUidTextBox.Text);
                }
                //auto start server and self login on start
                this.StartServerButton_Click(null, null);
                this.LoginButton_Click(null, null);

            }
            catch (Exception ex)
            {
                this.LogWriter(ex.ToString());
                this.generaLogger.Error(ex);
            }
        }

        /// <summary>
        /// Server Port text box key press event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void serverPortTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar) || (e.KeyChar == (char)Keys.Back)))
                e.Handled = true;
        }

        /// <summary>
        /// Connect To Server Port Key Press Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void connectServerPortTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar) || (e.KeyChar == (char)Keys.Back)))
                e.Handled = true;
        }

        /// <summary>
        /// Connect Uid Key Press Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void connectUidTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!(Char.IsDigit(e.KeyChar) || (e.KeyChar == (char)Keys.Back)))
                e.Handled = true;
        }

        /// <summary>
        /// Login Button Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginButton_Click(object sender, EventArgs e)
        {

            Login_Button.Enabled = false;

            string serverAddress = this.connectServerAddressTextBox.Text;
            string serverPort = this.connectServerPortTextBox.Text;
            string uid = this.connectUidTextBox.Text;

            this.LogWriter("Connecting to the ClipSync server");

            IDictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            keyValuePairs.Add("uid", uid);
            keyValuePairs.Add("platform", "WINDOWS");
            keyValuePairs.Add("device_id", globalHelper.GetMacAddress());

            try
            {
                var connection = new HubConnection("http://" + serverAddress + ":" + serverPort, keyValuePairs);
                this._hub = connection.CreateHubProxy(ConfigurationManager.AppSettings["hub_name"]);
                connection.Start().Wait();
                isSignalRConnected = true;
            }
            catch (Exception ex)
            {
                isSignalRConnected = false;
                Login_Button.Enabled = true;
                this.LogWriter("Exception in connecting to SignalR Hub : " + ex.ToString());
                this.generaLogger.Error(ex);
            }

            if (!isSignalRConnected)
            {
                MessageBox.Show("ClipSync Server is not running, so Please close the whole app and try again after sometime.", "Warning");
            }
            else
            {

                //MessageBox.Show("ClipSync Server is now connected, You need to connect to this uid: " + uid + " from all your devices. Now you can minimise this window", "Success");
                this.LogWriter("ClipSync Server is now connected, You need to connect to this uid: " + uid + " from all your devices. Now you can minimise this window");

                AddClipBoardListener();

                this._hub.On(ConfigurationManager.AppSettings["recieve_copied_text_signalr_method_name"], delegate (String data)
                {
                    //Console.WriteLine("Recieved Text :" + data);

                    if (data != null && data.Length > 0)
                    {
                        this.LogWriter("Your ClipBoard Updated with :");
                        ClipBoardHelper.SetText(data);
                        this.LogWriter(data);
                    }

                });
            }

        }

        /// <summary>
        /// Start Server Button Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartServerButton_Click(object sender, EventArgs e)
        {
            this.startServerButton.Enabled = false;
            this.LogWriter("Starting server on ");
            string url = "http://" + this.serverAddressTextBox.Text + ":" + this.serverPortTextBox.Text + "/";
            this.LogWriter(url);
            try
            {
                //SignalR = WebApp.Start<Startup>(url);
                signalRDisposable = WebApp.Start(url);
                this.LogWriter(string.Format("Server running at {0}", url + "signalr/hubs"));
                this.LogWriter("Your ip is: " + globalHelper.GetMachineIpAddress());
                this.LogWriter("Open the below link in your browser and if it opens then you can proceed further");
                this.LogWriter("http://" + globalHelper.GetMachineIpAddress() + ":" + this.serverPortTextBox.Text + "/signalr/hubs");
                this.LogWriter("You need to open a port in outbound rule of Windows FireWall. PORT IS : " + this.serverPortTextBox.Text);
                this.connectServerAddressTextBox.Text = globalHelper.GetMachineIpAddress();
                this.connectServerPortTextBox.Text = this.serverPortTextBox.Text;
                this.startServerButton.Enabled = false;

                this.OpenPortButton.PerformClick();
            }
            catch (System.Reflection.TargetInvocationException ex)
            {
                this.LogWriter("You need to run as administrator");
                this.LogWriter("Only server address localhost is allowed without administrator permissions");
                MessageBox.Show("You need to run as administrator", "Error");
                this.generaLogger.Error(ex);
                this.startServerButton.Enabled = true;
            }
            catch (Exception ex)
            {
                this.LogWriter(ex.ToString());
                this.generaLogger.Error(ex);
                this.startServerButton.Enabled = true;
            }
        }

        /// <summary>
        /// Open Port Button Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenPortButton_Click(object sender, EventArgs e)
        {
            try
            {

                this.OpenPortButton.Enabled = false;
                int serverPort = Convert.ToInt32(this.serverPortTextBox.Text);

                this.LogWriter("Checking the status of inbound port: " + serverPort + " Please wait...");
                if (this.globalHelper.IsPortOpened(serverPort, "ClipSync"))
                {
                    this.LogWriter("Port " + serverPort + " is already open");
                }
                else
                {
                    this.LogWriter("Opening the inbound port: " + serverPort + " Please wait...");
                    if (this.globalHelper.OpenInboundFirewallPort(serverPort, "ClipSync", serverPort.ToString()))
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
            }
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
            }
        }

        /// <summary>
        /// Log Writer on Form
        /// </summary>
        /// <param name="t">string text to write</param>
        internal void LogWriter(string t)
        {

            if (consoleTextBox.InvokeRequired)
            {
                this.Invoke((Action)(() =>
                    LogWriter(t)
                ));
                return;
            }

            this.generaLogger.Info(t);

            consoleTextBox.AppendText(Environment.NewLine + Environment.NewLine + t);
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
        /// <param name="m"></param>
        string lastCopy = "";
        bool waitCopyLoop = false;
        protected override void WndProc(ref Message m)
        {
            try
            {

                if (m.Msg == NativeMethods.WM_CLIPBOARDUPDATE)
                {

                    IDataObject iData = Clipboard.GetDataObject();      // Clipboard's data

                    if (m.Msg == NativeMethods.WM_CLIPBOARDUPDATE)
                    {
                        string copied_content = (string)iData.GetData(DataFormats.Text);
                        //do something with it
                        if (copied_content != null && copied_content.Length > 0)
                        {
                            if (!string.Equals(this.lastCopy, copied_content) || !waitCopyLoop)
                            {
                                double lastTime = TimeSpan.Parse(mTime).Seconds;
                                mTime = DateTime.Now.ToLongTimeString();
                                //if ((TimeSpan.Parse(mTime).Seconds - lastTime) > Convert.ToInt32(ConfigurationManager.AppSettings["number_of_seconds_interval_between_copy"])) {
                                this.LogWriter(copied_content);
                                _hub.Invoke(ConfigurationManager.AppSettings["send_copied_text_signalr_method_name"], copied_content);
                                //}
                                lastCopy = copied_content;
                                waitCopyLoop = true;
                            }
                            else if (waitCopyLoop)
                                waitCopyLoop = false;
                        }
                    }
                    else if (iData.GetDataPresent(DataFormats.Bitmap))
                    {
                        //Bitmap image = (Bitmap)iData.GetData(DataFormats.Bitmap);   // Clipboard image
                        //do something with it
                    }
                }

                base.WndProc(ref m);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                this.generaLogger.Error(ex);
            }
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
