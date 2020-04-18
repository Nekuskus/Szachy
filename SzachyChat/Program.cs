using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
namespace SzachyChat
{
    public class Program
    {
        public static string ID;
        public static bool wasMessageInput;
        public static int lastcount = 0;
        public static string Message;
        public static bool isReady = true;
        static void Main(string[] args)
        {
            Thread Odbierz = new Thread(new ParameterizedThreadStart(OdbierzCzat));
            Odbierz.Start(args[0]);
            Console.Title = "Czat sesji";
            Console.SetWindowSize(40, 50);
            Console.SetBufferSize(40, 750);
            while(!isReady)
            {
                isReady = false;
                Message = Console.ReadLine();
                wasMessageInput = true;
            }
        }
        static void OdbierzCzat(object IDarg)
        {
            ID = (string)IDarg;
            PrzeczytajCzat(ID);
            Thread.Sleep(4000);
        }
        static void PrzeczytajCzat(string ID)
        {
            StreamReader sr = new StreamReader($@".\Logs\Log{ID}.txt");
            string[] messages = sr.ReadToEnd().Split('\n');
            if(messages.Length > lastcount)
            {
                int i = lastcount;
                while(i >= messages.Length)
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
                    StreamWriter sw = new StreamWriter($@".\Logs\Log{ID}.txt");
                    sw.WriteLine(Message);
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