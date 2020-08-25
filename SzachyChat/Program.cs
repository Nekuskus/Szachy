using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Security.Cryptography;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Runtime.Remoting.Channels;

namespace SzachyChat
{
    static class Program
    {
        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if(args.Length == 0)
            {
                ChatClient.thisNick = "ThisPlayer";
                ChatClient.enemyNick = "EnemyPlayer";
                ChatClient.thisColour = "White";
                ChatClient.enemyColour = "Black";
            }
            else
            {
                ChatClient.thisNick = args[0];
                ChatClient.enemyNick = args[1];
                ChatClient.thisColour = args[2];
                ChatClient.enemyColour = args[3];
            }
            Process p = Process.GetCurrentProcess();
            p.EnableRaisingEvents = true;
            p.Exited += ChatClient.StopReading;
            p.Exited += ((sender, e) =>
            {
                ChatClient.ns.Dispose();
                ChatClient.tcpClient.Dispose();
            });
            ChatClient.Init();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
    static class ChatClient
    {
        public static async void Init()
        {
            localhost = IPAddress.Parse("127.17.155.122");
            // The one in SzachyMulti will have port 8082
            tcpListener = new TcpListener(IPAddress.Any, 8081);
            tcpListener.Start();
            tcpClient = await Task.FromResult(tcpListener.AcceptTcpClientAsync()).Result;
            ns = await Task.Run(() => tcpClient.GetStream());
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
        public static Queue<string> read_strings = new Queue<string>();
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
                    char app = (char)ns.ReadByte();
                    byte[] _amount = new byte[4];
                    int amount = await Task.Run(() => ns.ReadAsync(_amount, 0, 4)).ContinueWith((a) => { return BitConverter.ToInt32(_amount, 0); });
                    if(app == 'S')
                    {
                        byte[] bytes = new byte[amount];
                        await Task.Run(() => ns.ReadAsync(bytes, 0, bytes.Length)).ContinueWith((abc) => { lock(read_strings) { read_strings.Enqueue(bytes.ToString()); } });
                    }
                    else
                    {
                        ns.Read(new byte[amount], 0, amount);
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
/*
using System;
using System.IO;
using System.Threading;
namespace SzachyChat
{
    public class Program
    {
        public static string ID;
        public static string Nick;
        public static bool wasMessageInput;
        public static int lastcount = 0;
        public static string Message;
        public static bool isReady = true;
        static void ConsoleReader()
        {
            while (true)
            {
                if (isReady)
                {
                    ConsoleRead();
                }
            }
        }
        static void ConsoleRead()
        {
            isReady = false;
            Message = Console.ReadLine();
            wasMessageInput = true;
        }
        static void Main(string[] args)
        {
            ID = args[0];
            Nick = args[1];
            Thread Odbierz = new Thread(new ThreadStart(OdbierzCzat));
            Odbierz.Start();
            Console.Title = "Czat sesji";
            Console.SetWindowSize(40, 50);
            Console.SetBufferSize(40, 750);
            Thread Czytaj = new Thread(new ThreadStart(ConsoleReader));
            Czytaj.Start();
        }
        static void OdbierzCzat()
        {
            PrzeczytajCzat();
            Thread.Sleep(4000);
        }
        static void PrzeczytajCzat()
        {
            StreamReader sr = new StreamReader($@".\Chat\Logs\Log{ID}.txt");
            string[] messages = sr.ReadToEnd().Split('\n');
            if (messages.Length > lastcount)
            {
                int i = lastcount;
                while (i >= messages.Length)
                {
                    Console.WriteLine(messages[i]);
                    i++;
                }
            }
            if (wasMessageInput)
            {
                trywrite:
                try
                {
                    StreamWriter sw = new StreamWriter($@".\Chat\Logs\Log{ID}.txt");
                    sw.WriteLine($"{Nick}: {Message}");
                    isReady = true;
                }
                catch
                {
                    Thread.Sleep(150);
                    goto trywrite;
                }
            }
        }
    }
}
*/
