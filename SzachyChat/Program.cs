using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Security.Cryptography;

namespace SzachyChat
{
    static class Program
    {
        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>
        [STAThread]
        static void Main()
        {
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
            localhost = IPAddress.Parse("127.0.0.1");
            // The one in SzachyMulti will have port 8082
            tcpListener = new TcpListener(localhost, 8081);
            tcpClient = await Task.FromResult(tcpListener.AcceptTcpClientAsync()).Result;
            ns = tcpClient.GetStream();
        }
        public static NetworkStream ns;
        public static TcpClient tcpClient;
        public static IPAddress localhost;
        public static TcpListener tcpListener;
        public static Thread listenThread;
        public static Queue<string> read_strings;
        private static bool shouldStop = false;

        public static async void StartReading()
        {
            byte[] bytes = new byte[1024];
            while(!shouldStop)
            {
                while(ns.DataAvailable)
                {
                    await Task.Run(() => ns.ReadAsync(bytes.ToArray(), 0, 1024)).ContinueWith((abc) => { lock(read_strings) { read_strings.Enqueue(bytes.ToString()); } bytes = new byte[1024]; });
                }
            }
        }
        public static async void StopReading()
        {
            shouldStop = true;
            ns.Close();
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
