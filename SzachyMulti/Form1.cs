using System;
using System.Net;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;

namespace SzachyMulti
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void WriteInColour(Color c, string s, int start, int length)
        {
            Chat_TextBox.SelectionStart = Chat_TextBox.Text.Length;
            Chat_TextBox.SelectionLength = length;
            Chat_TextBox.SelectionColor = c;
            Chat_TextBox.AppendText(s.Substring(start, length));
            Chat_TextBox.SelectionColor = Color.Black;
            Chat_TextBox.AppendText(s.Substring(length));
        }
        private async void Form1_Load(object sender, EventArgs e)
        {
            Names_Label.Text = Names_Label.Text.Replace("Nick_1", ChatClient.thisNick).Replace("Color_1", ChatClient.thisColour).Replace("Nick_2", ChatClient.enemyNick).Replace("Color_2", ChatClient.enemyColour);
            Chat_TextBox.ForeColor = Color.Black;
            WriteInColour(Color.CadetBlue, $"{ChatClient.thisNick}: Hi\n", 0, ChatClient.thisNick.Length);
            WriteInColour(Color.MediumVioletRed, $"{ChatClient.enemyNick}: Hello\n", 0, ChatClient.enemyNick.Length);
            await Task.Run(() => ChatClient.StartReading());
            
            await Task.Run(() => {
                while(true)
                {
                    if(ChatClient.read_strings.Count > 0)
                    {
                        lock(ChatClient.read_strings)
                        {
                            var tuple = ChatClient.read_strings.Dequeue();
                            if(tuple.Item1 == true)
                                WriteInColour(Color.MediumVioletRed, $"{ChatClient.enemyNick}: {tuple.Item2}\n", 0, ChatClient.enemyNick.Length);
                            else
                                Chat_TextBox.AppendText($"{tuple.Item2}\n");
                        }
                    }
                    System.Threading.Thread.Sleep(350);
                }
            });
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            Input_TextBox.Enabled = false;
            byte[] chatbyte = UnicodeEncoding.UTF8.GetBytes(new char[] {'C'});
            byte[] bytelength = BitConverter.GetBytes(Input_TextBox.Text.Length);
            byte[] bytemessage = UnicodeEncoding.UTF8.GetBytes(Input_TextBox.Text);
            byte[] message = chatbyte.Concat(bytelength).Concat(bytemessage).ToArray();
            await ChatClient.ns.WriteAsync(message, 0, message.Length);
            WriteInColour(Color.BlueViolet, $"{ChatClient.thisNick}: {Input_TextBox.Text}\n", 0, ChatClient.thisNick.Length);
            Input_TextBox.Text = ""; 
            Input_TextBox.Enabled = true;
        }

        private void Input_TextBox_TextChanged(object sender, EventArgs e)
        {
            if(!(Input_TextBox.Text.Length == 0))
            {
                button1.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
            }
        }
    }
    static class ChatClient
    {
        public static async void Init(bool names = false)
        {
            if(names)
            {
                ChatClient.thisNick = "ThisPlayer";
                ChatClient.enemyNick = "EnemyPlayer";
                ChatClient.thisColour = "White";
                ChatClient.enemyColour = "Black";
            }
            // localhost = IPAddress.Parse("127.17.155.122");
            // The one in SzachyMulti will have port 8082
            tcpListener = new TcpListener(IPAddress.Any, 8081);
            tcpListener.Start();
            tcpClient = await tcpListener.AcceptTcpClientAsync();
            ns = await Task.Run(() => tcpClient.GetStream());
            listenThread.Start();
        }
        public static string thisNick;
        public static string enemyNick;
        public static string thisColour;
        public static string enemyColour;
        public static NetworkStream ns;
        public static TcpClient tcpClient;
        public static IPAddress localhost;
        public static TcpListener tcpListener;
        public static Thread listenThread = new Thread(StartReading);
        /*
            FORMAT:
            MESSAGE (true, string)
            BROADCAST (false, string)
        */
        public static Queue<(bool, string)> read_strings = new Queue<(bool, string)>();

        private static bool shouldStop = false;

        public static async void StartReading()
        {
            while(!shouldStop)
            {
                if(ns == null)
                {
                    Thread.Sleep(750);
                    continue;
                }
                while(ns.DataAvailable)
                {
                    /*
                        FORMAT:
                        MESSAGE "[M][SC]1234onethousandtwohundredthirtyfourcharacersmessagehere"
                        BROADCAST "[B]1234onethousandtwohundredthirtyfourcharacersmessagehere"
                    */
                    char ismessage = (char)ns.ReadByte();
                    if(ismessage == 'M')
                    {
                        char app = (char)ns.ReadByte();
                        byte[] _amount = new byte[4];
                        int amount = await Task.Run(() => ns.ReadAsync(_amount, 0, 4)).ContinueWith((a) => { return BitConverter.ToInt32(_amount, 0); });
                        if(app == 'S')
                        {
                            byte[] bytes = new byte[amount];
                            await Task.Run(() => ns.ReadAsync(bytes, 0, bytes.Length)).ContinueWith((abc) => { lock(read_strings) { read_strings.Enqueue((true, bytes.ToString())); } });
                        }
                        else
                        {
                            ns.Read(new byte[amount], 0, amount);
                        }
                    }
                    else
                    {
                        byte[] _amount = new byte[4];
                        int amount = await Task.Run(() => ns.ReadAsync(_amount, 0, 4)).ContinueWith((a) => { return BitConverter.ToInt32(_amount, 0); });
                        byte[] bytes = new byte[amount];
                        await Task.Run(() => ns.ReadAsync(bytes, 0, bytes.Length)).ContinueWith((abc) => { lock(read_strings) { read_strings.Enqueue((false, bytes.ToString())); } });
                    }
                }
            }
        }
        public static async void StopReading(Object sender, EventArgs e)
        {
            shouldStop = true;
            ns.Close(350);
        }
    }
}