using System;
using System.IO;
using System.Threading;
namespace SzachyChat
{
    public class Program
    {
        public static bool wasMessageSent = false;
        public static bool wasMessageInput = false;
        public static string Message;
        static void Main(string[] args)
        {

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
        static void InitKlient()
        {
            Thread Odbierz = new Thread(new ThreadStart(OdbierzWiad));
            Odbierz.Start();
        }
        static void OdbierzWiad()
        {

        }
    }
}