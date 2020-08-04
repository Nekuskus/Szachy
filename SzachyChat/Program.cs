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