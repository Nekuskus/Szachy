using System;
using System.IO;
using System.Threading;
namespace SzachyChat
{
    public class Program
    {
        public static bool wasMessageInput;
        public static int lastcount;
        public static string Message;
        static void Main(string[] args)
        {

            Thread Odbierz = new Thread(new ParameterizedThreadStart(OdbierzCzat(args[0])));
            Odbierz.Start();
            Console.Title = "Czat sesji";
            Console.SetWindowSize(40, 50);
            Console.SetBufferSize(40, 750);
            while (true)
            {
                wasMessageInput = false;
                Message = Console.ReadLine();
                wasMessageInput = true;
            }
        }
        static void OdbierzCzat(string ID)
        {
            PrzeczytajCzat(ID);
            Thread.Sleep(4000);
        }
        static void PrzeczytajCzat(string ID)
        {
            StreamReader sr = new StreamReader($@".\Logs\Log{ID}.txt");
        }
    }
}