using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Windows.Forms;

namespace PasteSimple2
{
    public partial class MainForm : Form
    {
        SyncServer server;
        SyncClient selfClient;

        StatusLabel statusLabel;

        public MainForm()
        {
            InitializeComponent();
            selfClient = new();
            server = new();
            selfClient.server = server;
            server.selfClient = selfClient;
            statusLabel = new StatusLabel(statusLabel1);
        }

        class StatusLabel
        {
            Label label;
            public StatusLabel(Label label)
            {
                this.label = label;
            }
            public void Start()
            {
                label.Text = "Running";
            }

            internal void Stop()
            {
                label.Text = "Stopped";
            }
        }

        public class SyncServer
        {
            readonly List<int> idList;
            readonly Dictionary<int, IPAddress> clientList;
            bool serverRunning = false;
            private UdpClient? udpClient;

            private Thread? serverThread;
            public SyncClient? selfClient;
            public SyncServer()
            {
                idList = new() { 1, 2, 3, 4, 5, 6, 6262 };
                clientList = new();
            }
            static public void SendUdp(string message, string adress, int port = 8888)
            {
                adress = "127.0.0.1"; //TODO
                ThreadStart ts = new(() =>
                {
                    UdpClient udpClient = new(adress, port);
                    udpClient.Client.SendBufferSize = 1024;
                    var msg = Encoding.UTF8.GetBytes(message);
                    udpClient.Send(msg, msg.Length);
                });
                Thread thread = new(ts);
                thread.Start();
                Debug.WriteLine($"{message.Replace("\n","\\n").Replace("\r","\\r")}->{adress}:{port}");
            }
            static public void SendSimpleData(string command, string content, string adress, int? port = null)
            {
                var data = new Dictionary<string, string>()
                {
                    {"command",command},
                    {"content",content}
                };
                if (port != null)
                    SendUdp(JsonSerializer.Serialize(data), adress, (int)port);
                else
                    SendUdp(JsonSerializer.Serialize(data), adress);
            }

            bool VerifyClient(int uid)
            {
                //TODO
                return true;
                //if (idList.Contains(uid)) { }
                //else
                //    return false;


                //return true;
            }

            void RegisterClient(int uid, IPEndPoint iPEndPoint)
            {
                clientList[uid] = iPEndPoint.Address;
                //SendUdp("SUCESS:", clientList[uid].ToString());
                SendSimpleData("SUCESS", "", clientList[uid].ToString());
            }

            void Sync(string content, IPEndPoint? exceptIpEndPoint = null)
            {
                Debug.WriteLine($"sync:{content}");
                foreach (var k in clientList.Keys)
                {
                    //SendUdp("SYNC:" + content, clientList[k].ToString());
                    SendSimpleData("SYNC", content, clientList[k].ToString());
                }
                if (exceptIpEndPoint != null)
                {
                    selfClient?.UpdateClipboard(content);
                }
            }

            public void SelfSendSync(string content)
            {
                Sync(content, null);
            }

            void HandleMsg(string command, string content, IPEndPoint remoteIpEndPoint)
            {
                switch (command)
                {
                    case "REGISTER":
                        var uid = Convert.ToInt32(content);
                        if (VerifyClient(uid))
                        {
                            RegisterClient(uid, remoteIpEndPoint);
                        }
                        break;
                    case "LOGOUT":
                        uid = Convert.ToInt32(content);
                        if (idList.Contains(uid) && clientList.ContainsKey(uid) && remoteIpEndPoint.Address.Equals(clientList[uid]))
                        {
                            clientList.Remove(uid);
                        }
                        break;
                    case "SYNC":
                        Sync(content, remoteIpEndPoint);
                        break;
                }
            }
            public void Start(int port = 9999)
            {
                if (serverRunning)
                {
                    MessageBox.Show("Server Already started");
                    return;
                }
                udpClient?.Close();
                udpClient = new(port);
                ThreadStart childref = new(() =>
                {
                    Debug.WriteLine("Starting Server...");
                    while (serverRunning)
                    {
                        var remoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                        byte[] recvBytes;
                        try
                        {
                            if (udpClient.Client == null)
                                break;
                            if (udpClient.Available == 0)
                                continue;
                            recvBytes = udpClient.Receive(ref remoteIpEndPoint);
                        }
                        catch (SocketException e)
                        {
                            Stop(true);
                            Debug.WriteLine(e);
                            break;
                        }
                        // BLOCK
                        string recvData = Encoding.UTF8.GetString(recvBytes);
                        Debug.WriteLine("recv:" + recvData);
                        Dictionary<string, string>? data;
                        try
                        {
                            data = JsonSerializer.Deserialize<Dictionary<string, string>>(recvData!);
                        }catch(System.Text.Json.JsonException e)
                        {
                            Debug.WriteLine(e);
                            continue;
                        }
                        //var r = Regex.Match(recvData, "(.*?):((?:.|\n|\r)*)");
                        //if (r.Success)
                        if (data is not null)
                        {
                            /* var command = r.Groups[1].ToString();
                             var content = r.Groups[2].ToString();*/
                            var command = data["command"];
                            var content = data["content"];
                            HandleMsg(command, content, remoteIpEndPoint);
                            Debug.WriteLine("Command>>>" + command);
                        }
                    }
                });
                serverThread = new(childref);
                serverThread.Start();
                serverRunning = true;
            }

            public void Stop(bool self = false)
            {
                serverRunning = false;
                if (!self)
                    Debug.WriteLine("Stop Server");

                if (self)
                    return;
                serverThread?.Interrupt();
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            server.Stop();
            statusLabel.Stop();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            var port = Convert.ToInt32(textBoxServerPort.Text);
            server.Start(port);
            statusLabel.Start();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            buttonStart.PerformClick();
        }

    }
}
