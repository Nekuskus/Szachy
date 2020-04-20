using FluentFTP;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace SzachyMulti
{
    public class Exception_2_rozne_od_2 : Exception
    {
        public override string Message
        {
            get
            {
                return "If you're seeing this, the code is in what I thought was an unreachable state.\nI could give you advice for what to do. But honestly, why should you trust me? I clearly screwed this up. I'm writing a message that should never appear, yet I know it will probably appear someday.\nOn a deep level, I know I'm not up to this task. I'm so sorry.\nhttps://xkcd.com/2200/";
            }
        }
    }
    public struct Pozycja
    {
        int _pos1;
        int _pos2;
        public Pozycja(char pos1, char pos2)
        {
            _pos1 = Convert.ToInt32(Program.SkonwertujLitere(pos1));
            _pos2 = Convert.ToInt32(pos2) - 1;
        }
        public int Pos1
        {
            get => _pos1;
            set => _pos1 = value;
        }
        public int Pos2
        {
            get => _pos2;
            set => _pos2 = value;
        }
    };
    class Szachy
    {
        public static string[,] Plansza = new string[8, 8];
        public static bool[,] SzachyB = new bool[8, 8];
        public static bool[,] SzachyC = new bool[8, 8];
        public static void PostawPionki()
        {
            //pierwsze - linia, drugie, kolumna
            Plansza[0, 0] = "wieza1B";
            Plansza[0, 1] = "kon1B";
            Plansza[0, 2] = "goniec1B";
            Plansza[0, 3] = "krolowaB";
            Plansza[0, 4] = "krolB";
            Plansza[0, 5] = "goniec2B";
            Plansza[0, 6] = "kon2B";
            Plansza[0, 7] = "wieza2B";
            Plansza[1, 0] = "pionek1B";
            Plansza[1, 1] = "pionek2B";
            Plansza[1, 2] = "pionek3B";
            Plansza[1, 3] = "pionek4B";
            Plansza[1, 4] = "pionek5B";
            Plansza[1, 5] = "pionek6B";
            Plansza[1, 6] = "pionek7B";
            Plansza[1, 7] = "pionek8B";
            Plansza[7, 0] = "wieza1C";
            Plansza[7, 1] = "kon1C";
            Plansza[7, 2] = "goniec1C";
            Plansza[7, 3] = "krolowaC";
            Plansza[7, 4] = "krolC";
            Plansza[7, 5] = "goniec2C";
            Plansza[7, 6] = "kon2C";
            Plansza[7, 7] = "wieza2C";
            Plansza[6, 0] = "pionek1C";
            Plansza[6, 1] = "pionek2C";
            Plansza[6, 2] = "pionek3C";
            Plansza[6, 3] = "pionek4C";
            Plansza[6, 4] = "pionek5C";
            Plansza[6, 5] = "pionek6C";
            Plansza[6, 6] = "pionek7C";
            Plansza[6, 7] = "pionek8C";
        }
        public static void OznaczSzachy()
        {

        }
        public static void NarysujPlansze()
        {
            Console.Write("\n\t    A       B       C       D       E       F       G       H\n");
            //\t = 8 *'-'
            //TODO: dodać rysowanie w drugą stronę i numerowanie pól!
            //65 - "-----------------------------------------------------------------" (7*8 + 9)
            if (Program.playerTeam == 'C')
            {
                for (int linia = 0, x = 0; linia <= 7; linia++)
                {
                    Console.WriteLine("\t-----------------------------------------------------------------");
                    Console.Write("\t|");
                    while (x < 8)
                    {
                        if (Plansza[linia, x] == "KrolB")
                        {
                            if (SzachyC[linia, x] == true)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.Write(" szach! |");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            else
                            {
                                Console.Write("       |");
                            }
                        }
                        else
                        {
                            Console.Write("       |");
                        }
                        x++;
                    }
                    x = 0;
                    Console.Write("\n");
                    Console.Write($"    {linia + 1}   |");
                    while (x < 8)
                    {
                        if (Plansza[linia, x] != null)
                        {
                            NapiszPionek(linia, x);
                            Console.Write("|");
                        }
                        else
                        {
                            Console.Write("       |");
                        }
                        x++;
                    }
                    Console.Write("\n");
                    x = 0;
                    Console.Write("\t|");
                    while (x < 8)
                    {
                        if (Plansza[linia, x] == "KrolC")
                        {
                            if (SzachyC[linia, x] == true)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write(" szach! |");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            else
                            {
                                Console.Write(" szach! |");
                            }
                        }
                        else
                        {
                            Console.Write("       |");
                        }
                        x++;
                    }
                    x = 0;
                    Console.Write("\n");
                }
                //Console.WriteLine("|       |       |       |       |       |       |       |       |");
                //if()
            }
            else
            {
                for (int linia = 7, x = 0; linia > -1; linia--)
                {
                    Console.WriteLine("\t-----------------------------------------------------------------");
                    Console.Write("\t|");
                    while (x < 8)
                    {
                        if (Plansza[linia, x] == "KrolB")
                        {
                            if (SzachyC[linia, x] == true)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.Write(" szach! |");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            else
                            {
                                Console.Write("       |");
                            }
                        }
                        else
                        {
                            Console.Write("       |");
                        }
                        x++;
                    }
                    x = 0;
                    Console.Write("\n");
                    Console.Write($"    {linia + 1}   |");
                    while (x < 8)
                    {
                        if (Plansza[linia, x] != null)
                        {
                            NapiszPionek(linia, x);
                            Console.Write("|");
                        }
                        else
                        {
                            Console.Write("       |");
                        }
                        x++;
                    }
                    Console.Write("\n");
                    x = 0;
                    Console.Write("\t|");
                    while (x < 8)
                    {
                        if (Plansza[linia, x] == "KrolC")
                        {
                            if (SzachyC[linia, x] == true)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write(" szach! |");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            else
                            {
                                Console.Write(" szach! |");
                            }
                        }
                        else
                        {
                            Console.Write("       |");
                        }
                        x++;
                    }
                    x = 0;
                    Console.Write("\n");
                    //TODO: dodać rysowanie w drugą stronę i numerowanie pól!
                }
            }
            Console.WriteLine("\t-----------------------------------------------------------------");

        }
        public static void NapiszPionek(int pos1, int pos2)
        {
            switch (Plansza[pos1, pos2].TrimEnd('1', '2', '3', '4', '5', '6', '7', '8', 'C', 'B'))
            {
                case "pionek":
                    if (Plansza[pos1, pos2].Last() == 'C')
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    Console.Write("pionek ");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case "wieza":
                    if (Plansza[pos1, pos2].Last() == 'C')
                    {
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                    }
                    Console.Write(" wieza ");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case "kon":
                    if (Plansza[pos1, pos2].Last() == 'C')
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    Console.Write("  kon  ");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case "goniec":
                    if (Plansza[pos1, pos2].Last() == 'C')
                    {
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                    }
                    Console.Write("goniec ");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case "krolowa":
                    if (Plansza[pos1, pos2].Last() == 'C')
                    {
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                    }
                    Console.Write("krolowa");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case "krol":
                    if (Plansza[pos1, pos2].Last() == 'C')
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                    Console.Write(" krol  ");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
        }
        public static void WykonajRuch(Pozycja Pozycja1, Pozycja Pozycja2)
        {
            string figureType = Plansza[Pozycja1.Pos1, Pozycja1.Pos2].TrimEnd('1', '2', '3', '4', '5', '6', '7', '8', 'B', 'C');
            switch (figureType)
            {
                case "Pionek":
                    RuchPionem(Pozycja1, Pozycja2);
                    break;
                case "Kon":
                    RuchKoniem(Pozycja1, Pozycja2);
                    break;
                case "Goniec":
                    RuchGońcem(Pozycja1, Pozycja2);
                    break;
                case "Wieza":
                    RuchWieżą(Pozycja1, Pozycja2);
                    break;
                case "Krol":
                    RuchKrólem(Pozycja1, Pozycja2);
                    break;
                case "Krolowa":
                    RuchKrólową(Pozycja1, Pozycja2);
                    break;
            }
        }
        public static void RuchPionem(Pozycja Pozycja1, Pozycja Pozycja2)
        {
            switch (Program.playerTeam)
            {

            }
        }
        public static void RuchKoniem(Pozycja Pozycja1, Pozycja Pozycja2)
        {
            switch (Program.playerTeam)
            {

            }
        }
        public static void RuchGońcem(Pozycja Pozycja1, Pozycja Pozycja2)
        {
            switch (Program.playerTeam)
            {

            }
        }
        public static void RuchWieżą(Pozycja Pozycja1, Pozycja Pozycja2)
        {
            switch (Program.playerTeam)
            {

            }
        }

        public static void RuchKrólową(Pozycja Pozycja1, Pozycja Pozycja2)
        {

        }
        public static void RuchKrólem(Pozycja Pozycja1, Pozycja Pozycja2)
        {
            switch (Program.playerTeam)
            {

            }
        }
        public static void WyślijWiadomość(string currentfolder, string wiadomosc, int ID)
        {
            Program.isSending = true;
            if (!Program.client.IsConnected)
            {
                Program.client.Connect();
            }
            Program.AppendText($"{Program.Nick}: {wiadomosc}", currentfolder, Convert.ToString(ID));
        }
    }
    public class Program
    {
        public static Random rand = new Random();
        public static bool hasOtherPlayerJoined = false;
        public static bool isApppending = false;
        public static bool wasContentReceived = false;
        public static bool isContentBeingReceived = false;
        public static int lastCountChat = 0;
        public static List<string> receivedMessages = new List<string>();
        public static Process chat = new Process();
        public static List<string> lines = new List<string>();
        public static bool odebranoWiad = false;
        public static List<string> chatLog = null;
        public static bool isChatOpen;
        public static string currentfolder = "StartingSessions";
        public static string Nick;
        public static string enemyNick;
        public static char playerTeam;
        public static char enemyTeam;
        public static int lastCount;
        public static bool isSending = false;
        public static string ID;
        public static bool isOdbierz;
        public static bool BasicIO;
        public static bool odswiezlobby;
        public static bool wykonajconnect = true;
        public static string nazwasesji;
        public static List<string> GetContent(string folder, string id)
        {
            var tmp = client.OpenRead($"/SzachySerwer/{folder}/{id}.txt");
            byte[] buffer = new byte[tmp.Length];
            tmp.Read(buffer, 0, (Int32)tmp.Length);
            string str = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            List<string> lines = str.Split('\n').ToList();
            return lines;
        }
        public static void AppendText(string txt, string Folder, string ID)
        {
            isApppending = true;
            txt = txt + "\n";
        writetofile:
            try
            {
                var tmp = client.OpenAppend($"/SzachySerwer/{Folder}/{ID}.txt");
                var bytes = Encoding.UTF8.GetBytes(txt);
                tmp.Write(bytes, 0, bytes.Count());
                client.Disconnect();
                isApppending = false;
            }
            catch
            {
                client.Disconnect();
                Thread.Sleep(rand.Next(140, 201));
                goto writetofile;
            }
            isApppending = false;
        }
        public static void CreateSessionFile(string ID, bool IsPublic, string name, string nick)
        {
            var tmp = client.OpenWrite($"/SzachySerwer/StartingSessions/{ID}.txt");
            string txt = "";
            if (IsPublic) txt += "public\n";
            if (!IsPublic) txt += "private\n";
            txt += $"{name}\n";
            txt += $"{nick}\n";
            int playerTeamInt = rand.Next(1, 3);
            if (playerTeamInt == 2)
            {
                playerTeam = 'B';
            }
            else
            {
                playerTeam = 'C';
            }
            txt += $"{playerTeam}\n";
            var bytes = Encoding.UTF8.GetBytes(txt);
            tmp.Write(bytes, 0, bytes.Length);
        }

        public static FtpClient client = null;
        public static bool czy_odbierac = false;
        public static int SkonwertujLitere(char litera)
        {
            switch (litera)
            {
                case 'A':
                    return 0;
                case 'B':
                    return 1;
                case 'C':
                    return 2;
                case 'D':
                    return 3;
                case 'E':
                    return 4;
                case 'F':
                    return 5;
                case 'G':
                    return 6;
                case 'H':
                    return 7;
                default:
                    Console.WriteLine("Damian co ty zrobiles to nie powinno nigdy sie stac");
                    throw new Exception_2_rozne_od_2();
            }
        }
        public static void Odbierz()
        {
            client.Connect();
            isOdbierz = true;
            lines = GetContent(currentfolder, Convert.ToString(ID));
            if (lastCount < lines.Count() || isApppending == false)
            {
                //"Nick: [wydarzenie]"
                wasContentReceived = true;
                int i = lastCount + 1;
                if (!lines[i].StartsWith(Nick))
                {
                    while (i <= lines.Count)
                    {
                        lines[i] = lines[i].Substring(enemyNick.Length + 2);
                        if (lines[i].Contains(">>"))
                        {
                            //"A1 >> A2"
                            string[] moveArgs = new string[2];
                            moveArgs[0] = lines[i].Substring(0, 2);
                            moveArgs[1] = lines[i].Substring(6, 2);
                            Pozycja Pozycja1 = new Pozycja();
                            Pozycja Pozycja2 = new Pozycja();
                            Pozycja1.Pos1 = SkonwertujLitere(moveArgs[0].First());
                            Pozycja1.Pos2 = moveArgs[0].Last();
                            Pozycja2.Pos1 = SkonwertujLitere(moveArgs[1].First());
                            Pozycja2.Pos2 = moveArgs[1].Last();
                        }
                        else if (lines[i].Contains("CZAT"))
                        {
                            odebranoWiad = true;
                            //"CZAT {wiad}"
                            lines[i] = lines[i].Remove(0, 5);
                            lines[i] = lines[i].Trim();
                            lines[i] = lines[i].Insert(0, $"{enemyNick}: ");
                            receivedMessages.Add(lines[i]);
                        }
                        else if (lines[i].Contains("CLOSING"))
                        {
                            AppendText("OK", "StartingSessions", ID);
                            currentfolder = "ActiveSessions";
                        }
                        else if (lines[i].Contains("OK"))
                        {
                            //"OK"
                            client.MoveFile($"/SzachySerwer/StartingSessions/{ID}.txt", $"/SzachySerwer/ActiveSessions/{ID}.txt");
                        }
                        else if (lines[i].Contains("JOIN"))
                        {
                            enemyNick = lines[i].Remove(0, 5).Trim();
                            hasOtherPlayerJoined = true;
                        }
                        i++;
                    } 
                }
                lastCount = lines.Count;
            }
            if (!isSending)
            {
                client.Disconnect();
            }
            lastCount = lines.Count;
            isOdbierz = false;
        }
        public static void OtwórzCzat()
        {
            File.CreateText($@".\Chat\Logs\Log{ID}.txt");
            ProcessStartInfo psi = new ProcessStartInfo()
            {
                CreateNoWindow = false,
                UseShellExecute = true,
                FileName = $@"{Directory.GetCurrentDirectory()}\Chat\SzachyChat.exe",
                WindowStyle = ProcessWindowStyle.Normal,
                Arguments = $"{ID}"
            };
            chat.StartInfo = psi;
            chat.Start();
            isChatOpen = true;
        }
        static void Chat()
        {
            StreamReader chatReader = new StreamReader($@".\Chat\Logs\Log{ID}.txt");
            StreamWriter chatWriter = new StreamWriter($@".\Chat\Logs\Log{ID}.txt");
            chatLog = null;
            chatLog.AddRange(chatReader.ReadToEnd().Split('\n'));
            int countNow = chatLog.Count();
            if (countNow > lastCountChat)
            {
                isSending = true;
                //usun z listy poprzednie linijki lub uzyj petli for
                List<string> sentMessages = new List<string>();
                for (int i = lastCountChat + 1; i <= countNow; i++)
                {
                    sentMessages.Add($"{Nick}: {chatLog[i]}");
                }
                if (client.IsConnected == false)
                {
                    client.Connect();
                }
                foreach (string msg in sentMessages)
                {
                    AppendText(msg, currentfolder, Convert.ToString(ID));
                }
                sentMessages = null;
                lastCountChat = chatLog.Count();
                if (isOdbierz == false)
                {
                    client.Disconnect();
                }
                isSending = false;
                chatReader.Close();
            }
            if (odebranoWiad)
            {
                chatWriter.Close();
                //sw = chat.StandardInput;
                foreach (string receivedMessage in receivedMessages)
                {
                    chatWriter.WriteLine($"{enemyNick}: {receivedMessage}");
                    receivedMessages.Remove(receivedMessage);
                }
                odebranoWiad = false;
                chatWriter.Close();
            }
        }

        public static void KlientOdbierajacy()
        {

            while (true)
            {
                int repeati = 0;
                while (czy_odbierac == true)
                {
                    try
                    {
                        Odbierz();
                        repeati = 0;
                        Thread.Sleep(700);
                    }
                    catch
                    {
                        if (repeati >= 4)
                        {
                            Console.WriteLine("Nie udalo sie polaczyc z serwerem, sprobowac ponownie?\n//Mozliwe odpowiedzi: Tak, Nie\n(Odpowiedz \"Nie\" przerywa sesje, nie jest to wskazane.");
                        connrepeatgoto:
                            string connrepeat = Console.ReadLine();
                            if (connrepeat.ToLower().Contains("tak"))
                            {
                                repeati++;
                            }
                            else if (connrepeat.ToLower().Contains("nie"))
                            {
                                Console.WriteLine("Konczenie sesji... cofanie do menu glownego");
                                czy_odbierac = false;
                            }
                            else
                            {
                                Console.WriteLine($"Niepoprawna odpowiedz: {connrepeat}");
                                goto connrepeatgoto;
                            }
                        }
                        repeati++;
                    }
                }
            }
        }
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                if (args[0] == "true")
                {
                    BasicIO = true;
                }
            }
        menuglowne:
            Console.WriteLine($"Obecny czas: {DateTime.Now}/{(Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds}");
            Console.WriteLine("Polaczyc sie z serwerem? Tak/Nie");
            string czy_polaczyc = Console.ReadLine().ToLower();
            if (czy_polaczyc.Contains("tak"))
            {
                Console.WriteLine("Podaj swoj nick:");
                Nick = Console.ReadLine();
                string jsontxt;
                using (StreamReader r = new StreamReader("credentials.json"))
                {
                    jsontxt = r.ReadToEnd();
                }
                JObject jsonObject = JObject.Parse(jsontxt);
                client = new FtpClient();
                Console.WriteLine(jsonObject);
                client.Host = (string)jsonObject["host"];
                client.Port = (int)jsonObject["port"];
                client.Credentials = new NetworkCredential((string)jsonObject["login"], (string)jsonObject["password"]);
            polacz:
                try
                {
                    Console.WriteLine("Laczenie...");
                    client.Connect();
                }
                catch
                {
                    Console.WriteLine("Nie udalo sie polaczyc z serwerem, sprobowac ponownie?\n//Mozliwe odpowiedzi: Tak, Nie");
                    string connrepeat = Console.ReadLine().ToLower();
                    if (connrepeat.Contains("tak"))
                    {
                        goto polacz;
                    }
                    else if (connrepeat.Contains("nie"))
                    {
                        Console.WriteLine("Cofanie do menu glownego");
                        goto menuglowne;
                    }
                }
            }
            else if (czy_polaczyc.Contains("nie"))
            {
                Console.WriteLine("Cofanie do menu glownego");
                goto menuglowne;
            }
            else
            {
                Console.WriteLine($"Niepoprawna odpowiedz {czy_polaczyc}");
                goto menuglowne;
            }
        lobby:
            try
            {
                bool donotincrement = false;
                if (client.GetWorkingDirectory() != "/SzachySerwer/StartingSessions/")
                {
                    client.SetWorkingDirectory(@"/SzachySerwer/StartingSessions/");
                }
                wykonajconnect = false;
                odswiezlobby = false;
                Console.WriteLine("---Lobby------------------------------------------------------------------------");
                Console.WriteLine("Lp. Nazwa pokoju               - ID");
                int i = 1;
                foreach (var item in client.GetListing(client.GetWorkingDirectory(), FtpListOption.Recursive))
                {
                    donotincrement = false;
                    var isFile = client.FileExists(item.FullName);
                    if (isFile == true)
                    {
                        var tmp = client.OpenRead($"/SzachySerwer/StartingSessions/{item.Name}");
                        byte[] buffer = new byte[tmp.Length];
                        tmp.Read(buffer, 0, (Int32)tmp.Length);
                        string str = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                        List<string> lines = str.Split('\n').ToList();
                        tmp.Close();
                        if (lines[0] == "public")
                        {
                            if (lines[1].Length > 25)
                            {
                                lines[1] = lines[1].Remove(25, lines[1].Length - 25);
                            }
                            Console.WriteLine($"{i}.  \"{lines[1]}\"" + new String(' ', 25 - lines[1].Length) + $"- {item.Name.TrimEnd('t', 'x', '.')}");
                        }
                        else
                        {
                            donotincrement = true;
                        }
                    }
                    if (donotincrement == false)
                    {
                        i++;
                    }
                }
                client.SetWorkingDirectory(@"/SzachySerwer/");
                client.Disconnect();
            }
            catch (Exception e)
            {
                client.Disconnect();
                Console.WriteLine(e);
                Console.WriteLine("Blad odbierania lobby. Sprobowac ponownie? y/n");
                string tryagainlobby = Console.ReadLine();
                if (tryagainlobby.ToLower().Contains("y"))
                {
                    goto lobby;
                }
            }
            odswiezlobby = false;
            wykonajconnect = false;
            bool isCancel = false;
            Console.WriteLine("\nPodaj ID sesji publicznej (z lobby) lub prywatnej (otrzymanej od znajomego) aby dolaczyc do niej. Utworz nowa sesje poleceniem \"create\". Odswiez lobby poleceniem \"refresh\"");
            string connect_to_id = Console.ReadLine();
            if (connect_to_id.ToLower().Contains("create"))
            {
                bool isPublic;
            podajnazwesesji:
                Console.Write("\nPodaj nazwe sesji, ktora chcesz stworzyc lub napisz \"cancel\" aby wrocic do lobby");
                nazwasesji = Console.ReadLine();
                if (nazwasesji.ToLower() == "cancel")
                {
                    goto lobby;
                }
                if (nazwasesji.Length > 25)
                {
                    Console.WriteLine("Nazwa sesji jest zbyt dluga. Wpisz krotsza nazwe (max 25 znakow)");
                    goto podajnazwesesji;
                }
            ispublicgoto:
                if (isCancel == false)
                {
                    Console.WriteLine("\nCzy sesja ma byc prywatna? Tak/Nie");
                    string ispublicstr = Console.ReadLine();
                    if (ispublicstr.ToLower().Contains("tak"))
                    {
                        isPublic = true;
                    }
                    else if (ispublicstr.ToLower().Contains("nie"))
                    {
                        isPublic = false;
                    }
                    else
                    {
                        Console.WriteLine("Niepoprawna odpowiedz:" + ispublicstr);
                        goto ispublicgoto;
                    }
                    Console.WriteLine($"Tworzenie sesji o nazwie \"{nazwasesji}\", kontynuowac? Tak/Nie");
                    string napewno = Console.ReadLine();
                    if (napewno.ToLower().Contains("tak"))
                    {
                        ID = Convert.ToString((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);

                        client.Connect();
                        CreateSessionFile(ID, isPublic, nazwasesji, Nick);
                        client.Disconnect();
                        InitKlient();
                        Console.WriteLine("Czekanie na drugiego gracza...");
                        while (!hasOtherPlayerJoined)
                        {
                            Thread.Sleep(TimeSpan.FromSeconds(5));
                        }

                        Rozgrywka();
                        //(Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds
                    }
                    else if (napewno.ToLower().Contains("nie"))
                    {
                        Console.WriteLine("\nWracanie do lobby...\n");
                        wykonajconnect = false;
                        odswiezlobby = true;
                    }
                }
            }
            else if (connect_to_id.ToLower().Contains("refresh"))
            {
                Console.WriteLine("\nOdswiezanie lobby...\n");
                wykonajconnect = false;
                odswiezlobby = true;
                client.Connect();

            }
            try
            {
                if (isCancel == false)
                {
                    ID = connect_to_id;
                    wykonajconnect = true;
                }
            }
            catch (InvalidCastException)
            {
                Console.WriteLine("Niepoprawna odpowiedz:" + connect_to_id);
                Console.WriteLine("Cofanie do lobby...\n");
                wykonajconnect = false;
                goto lobby;
            }
            if (wykonajconnect == true)
            {
                foreach (var item in client.GetNameListing())
                {
                    //connect, setdirectory
                    var isFile = client.FileExists(item);
                    if (isFile == true)
                    {
                        if (item.GetFtpFileName().TrimEnd('.', 't', 'x') == connect_to_id)
                        {
                            Console.Clear();
                            Console.SetWindowSize(80, 50);
                            ID = connect_to_id;
                            AppendText($"JOIN: {Nick}", currentfolder, connect_to_id);
                            InitKlient();
                            nazwasesji = lines[0];
                            enemyNick = lines[2];
                            enemyTeam = Convert.ToChar(lines[3]);
                            switch(enemyTeam)
                            {
                                case 'B':
                                    playerTeam = 'C';
                                    break;
                                case 'C':
                                    playerTeam = 'B';
                                    break;
                            }
                            Rozgrywka();
                        }
                    }
                }
            }
        }
        static void SizeCheckThread()
        {
            if (Console.WindowWidth != 80)
            {
                Console.SetWindowSize(80, Console.WindowHeight);
            }
        }
        static void SizeCheck()
        {
            while (true)
            {
                SizeCheckThread();
                Thread.Sleep(3000);
            }
        }

        static void InitKlient()
        {
            Thread Size = new Thread(new ThreadStart(SizeCheck));
            Size.Start();
            if (BasicIO == true)
            {
                OtwórzCzat();
                Thread chat = new Thread(new ThreadStart(ChatThread));
                chat.Start();
            }
            Thread t = new Thread(new ThreadStart(KlientOdbierajacy));
            t.Start();
        }
        static void ChatThread()
        {
            while (true)
            {
                Chat();
                Thread.Sleep(500);
            }
        }
        static public string Przekazywacz(string przekażCzat)
        {

            //if ruch przekaż do rozgrywka
            return "abc";
        }
        static void Rozgrywka()
        {
            switch(playerTeam)
            {
                case 'B':
                    //napisz w czacie $"Gracz {Nick} gra białymi"
                    AppendText($"Gracz {Nick} gra białymi",currentfolder,ID);
                    receivedMessages.Add($"Gracz {Nick} gra białymi");
                    odebranoWiad = true;
                    break;
                case 'C':
                    break;
            }
        }
    }
}