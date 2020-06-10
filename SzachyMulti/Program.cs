using Credentials;
using FluentFTP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace SzachyMulti
{
    public static class ArrayExtension
    {
        /// <summary>
        /// Used to copy 2dimensional arrays as normal copying with = only makes a set of reference values.
        /// </summary>
        /// <typeparam name="T"> Used this to make it work for any type of array, using Array class is not working because it does not have [] indexing as it is only a base for all array types</typeparam>
        /// <param name="destination"> This is the array values will be copied to </param>
        public static void CopyD<T>(this T[,] arr, ref T[,] destination)
        {
            if(arr.Rank != destination.Rank)
            {
                throw new ArgumentException("The arrays had different numbers of dimensions.");
            }
            for(int i = 0; i < arr.Rank; i++)
            {
                if(arr.GetLength(i) != destination.GetLength(i))
                {
                    throw new ArgumentException($"The lenght of arrays' dimension {i} did not match.");
                }
            }
            for(int i = arr.GetLowerBound(0); i <= arr.GetUpperBound(0); i++)
            {
                for(int i2 = arr.GetLowerBound(1); i2 <= arr.GetUpperBound(1); i2++)
                {
                    destination.SetValue(arr.GetValue(i,i2),i,i2);
                }
            }
        }
    }
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
     class InvalidMoveException : Exception
    {
        private string _Message;
        private string _Details;
        private bool _WasEmpty;
        private bool _WasOtherTeam;
        private bool _WasKing;
        private bool _WasSameTeam;
        public string Details
        {
            get => _Details;
        }
        public override string Message
        {
            get => _Message;
        }
        public bool WasEmpty
        {
            get => _WasEmpty;
        }
        public bool WasOtherTeam
        {
            get => _WasOtherTeam;
        }
        public bool WasKing
        {
            get => _WasKing;
        }
        public bool WasSameTeam
        {
            get => _WasSameTeam;
        }
        public InvalidMoveException()
        {
            _Message = "Client has sent an invalid move"; //Add information about the move here!
        }
        public InvalidMoveException(String message) : base(message)
        {
            _Message = "Client has sent an invalid move: " + message;
            _Details = Message;
        }
        /// <summary>
        /// Constructor intended for special conditions
        /// </summary>
        /// <param name="message"> Error message </param>
        /// <param name="cond">
        /// Was other team: "other"  Was empty: "empty"   Was a king: "king"   Was the same team: "same"</param>
        public InvalidMoveException(String message, String cond)
        {
            _Message = "Client has sent an invalid move: " + message;
            _Details = Message;
            if(cond == "empty")
            {
                _WasEmpty = true;
            }
            if(cond == "other")
            {
                _WasOtherTeam = true;
            }
            if(cond == "king")
            {
                _WasKing = true;
            }
            if(cond == "same")
            {
                _WasSameTeam = true;
            }
        }
        public InvalidMoveException(String message, Exception innerException) : base(message, innerException)
        {
            _Message = message;
            _Details = message;
        }
    }
    public struct Pozycja
    {
        int _pos1;
        int _pos2;
        public Pozycja(char pos1, char pos2)
        {
            _pos2 = Program.SkonwertujLitere(Char.ToUpper(pos1));
            _pos1 = (int)char.GetNumericValue(pos2) - 1;
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
        public override string ToString()
        {
            string str = $"{Program.SkonwertujCyfre(this.Pos2)}{this.Pos1+1}";
            return str;
        }
    };
    [Serializable]
    public static class Szachy
    {
        /// <summary>
        /// ISerializable replacement
        /// </summary>
        /// <param name="path">
        /// This param gives the folder the board will be saved to. Default = @".\SavedBoards\"
        /// </param>
        static bool SerializeBoard(string path = @".\SavedBoards\")
        {
            List<List<string>> plansze = new List<List<string>>();
            List<string> pola = new List<string>();
            foreach(var item in Plansza)
            {
                pola.Add(((int)item).ToString());
            }
            plansze.Add(pola);
            pola.Clear();
            foreach(var item in SzachyBC)
            {
                pola.Add(((int)item).ToString());
            }
            plansze.Add(pola);
            pola.Clear();
            foreach(var item in HiddenSzachyBC)
            {
                pola.Add(((int)item).ToString());
            }
            plansze.Add(pola);
            pola.Clear();
            foreach(var item in PossibleEnPassants)
            {
                pola.Add(((int)item).ToString());
            }
            plansze.Add(pola);
            pola.Clear();
            foreach(var item in BackupPlansza)
            {
                pola.Add(((int)item).ToString());
            }
            plansze.Add(pola);
            pola.Clear();
            foreach(var item in BackupSzachyBC)
            {
                pola.Add(((int)item).ToString());
            }
            plansze.Add(pola);
            pola.Clear();
            foreach(var item in BackupHiddenSzachyBC)
            {
                pola.Add(((int)item).ToString());
            }
            plansze.Add(pola);
            pola.Clear();
            foreach(var item in BackupPossibleEnPassants)
            {
                pola.Add(((int)item).ToString());
            }
            plansze.Add(pola);
            pola.Clear();
            try 
            {
                using(StreamWriter sw = new StreamWriter(path + $"{DateTime.Now.ToString().Replace('/','-').Replace(' ', '_')}.txt"))
                {
                    foreach(var plansza in plansze)
                    {
                        sw.WriteLine(plansza.ToArray().Join(","));
                        sw.WriteLine();
                    }
                }
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine("An exception has occured when serializing the boards: " + ex.ToString());
                return false;
            }
        }
        [Flags]
        public enum ChessPiece
        {
            None = 0b_0000_0000,
            TeamB = 0b_0100_0000,
            TeamC = 0b_1000_0000,
            King = 0b_0010_0000,
            Queen = Rook | Bishop,
            Rook = 0b_0000_1000,
            Knight = 0b_0000_0100,
            Bishop = 0b_0000_0010,
            Pawn = 0b_0000_0001,
            AllPieces = King | Queen | Rook | Knight | Bishop | Pawn,
            BothTeams = TeamB | TeamC
        }
        public static volatile ChessPiece[,] Plansza = new ChessPiece[8, 8];
        public static volatile ChessPiece[,] SzachyBC = new ChessPiece[8, 8];
        public static volatile ChessPiece[,] HiddenSzachyBC = new ChessPiece[8, 8];
        public static volatile ChessPiece[,] BackupPlansza = new ChessPiece[8, 8];
        public static volatile ChessPiece[,] BackupSzachyBC = new ChessPiece[8, 8];
        public static volatile ChessPiece[,] BackupHiddenSzachyBC = new ChessPiece[8, 8];
        public static volatile ChessPiece[,] PossibleEnPassants = new ChessPiece[8, 8];
        public static volatile ChessPiece[,] BackupPossibleEnPassants = new ChessPiece[8, 8];
        public static void PostawPionki()
        {
            //pierwsze - linia, drugie, kolumna
            //Ily my pieces
            Plansza[0, 0] = ChessPiece.Rook | ChessPiece.TeamB;
            Plansza[0, 1] = ChessPiece.Knight | ChessPiece.TeamB;
            Plansza[0, 2] = ChessPiece.Bishop | ChessPiece.TeamB;
            Plansza[0, 3] = ChessPiece.Queen | ChessPiece.TeamB;
            Plansza[0, 4] = ChessPiece.King | ChessPiece.TeamB;
            Plansza[0, 5] = ChessPiece.Bishop | ChessPiece.TeamB;
            Plansza[0, 6] = ChessPiece.Knight | ChessPiece.TeamB;
            Plansza[0, 7] = ChessPiece.Rook | ChessPiece.TeamB;
            Plansza[1, 0] = ChessPiece.Pawn | ChessPiece.TeamB;
            Plansza[1, 1] = ChessPiece.Pawn | ChessPiece.TeamB;
            Plansza[1, 2] = ChessPiece.Pawn | ChessPiece.TeamB;
            Plansza[1, 3] = ChessPiece.Pawn | ChessPiece.TeamB;
            Plansza[1, 4] = ChessPiece.Pawn | ChessPiece.TeamB;
            Plansza[1, 5] = ChessPiece.Pawn | ChessPiece.TeamB;
            Plansza[1, 6] = ChessPiece.Pawn | ChessPiece.TeamB;
            Plansza[1, 7] = ChessPiece.Pawn | ChessPiece.TeamB;
            //DEBUG 
            // Ily ❤︎
            //Plansza[4, 2] = ChessPiece.Pawn | ChessPiece.TeamC; //"pionek1C" :heart:
            //Plansza[3, 3] = ChessPiece.King | ChessPiece.TeamB; //"krol2B" :heart:
            //Plansza[2, 0] = ChessPiece.King | ChessPiece.TeamC; //"krolC" :heart:
            //Plansza[2, 2] = ChessPiece.King | ChessPiece.TeamC; //"krolC" :heart:
            //Plansza[4, 7] = ChessPiece.Knight | ChessPiece.TeamC; //"kon123C" :heart:
            //Plansza[3, 5] = ChessPiece.King | ChessPiece.TeamB; //"krolB" :heart:
            //Plansza[3, 7] = ChessPiece.King | ChessPiece.TeamC; //"krolC" :heart:
            //Plansza[2, 5] = ChessPiece.Knight | ChessPiece.TeamB; //"kon123B" :heart:
            //Plansza[3, 0] = ChessPiece.King | ChessPiece.TeamC; //"krolC" :heart:
            //Plansza[5, 2] = ChessPiece.Bishop | ChessPiece.TeamB; //Plansza[5, 2] = "goniecB"; //B :heart:
            //Plansza[2, 4] = ChessPiece.King | ChessPiece.TeamC;
            //Plansza[4, 4] = ChessPiece.Queen | ChessPiece.TeamB;
            //Plansza[2, 6] = ChessPiece.King | ChessPiece.TeamC;
            //Plansza[4, 2] = ChessPiece.King | ChessPiece.TeamC;
            // ❤︎
            Plansza[3, 4] = ChessPiece.King | ChessPiece.TeamC;
            Plansza[3, 1] = ChessPiece.Bishop | ChessPiece.TeamC;
            //DEBUG 
            Plansza[7, 0] = ChessPiece.Rook | ChessPiece.TeamC;
            Plansza[7, 1] = ChessPiece.Knight | ChessPiece.TeamC;
            Plansza[7, 2] = ChessPiece.Bishop | ChessPiece.TeamC;
            Plansza[7, 3] = ChessPiece.Queen | ChessPiece.TeamC;
            Plansza[7, 4] = ChessPiece.King | ChessPiece.TeamC;
            Plansza[7, 5] = ChessPiece.Bishop | ChessPiece.TeamC;
            Plansza[7, 6] = ChessPiece.Knight | ChessPiece.TeamC;
            Plansza[7, 7] = ChessPiece.Rook | ChessPiece.TeamC;
            Plansza[6, 0] = ChessPiece.Pawn | ChessPiece.TeamC;
            Plansza[6, 1] = ChessPiece.Pawn | ChessPiece.TeamC;
            Plansza[6, 2] = ChessPiece.Pawn | ChessPiece.TeamC;
            Plansza[6, 3] = ChessPiece.Pawn | ChessPiece.TeamC;
            Plansza[6, 4] = ChessPiece.Pawn | ChessPiece.TeamC;
            Plansza[6, 5] = ChessPiece.Pawn | ChessPiece.TeamC;
            Plansza[6, 6] = ChessPiece.Pawn | ChessPiece.TeamC;
            Plansza[6, 7] = ChessPiece.Pawn | ChessPiece.TeamC;
        }
        ///<summary>
        /// A function for clearing the board 
        /// </summary>
        /// <remarks> It should be used after the match is complete to allow easy starting another one. 
        ///  My feelings stop me from clearing the backups. I guess I will do without it. </remarks>
        public static void WyczyscPlansze()
        {
            for(int i = 0; i < 8; i++)
            {
                for(int i2 = 0; i2 < 8; i2++)
                {
                    Plansza[i, i2] = ChessPiece.None;
                    SzachyBC[i, i2] = ChessPiece.None;
                    HiddenSzachyBC[i, i2] = ChessPiece.None;
                }
            }
        }
		///
		///	<oldcomments>
		///		IMPORTANT: Moved to above function.
        ///		<summary>
        /// 	This is checking for whether a king got threatened as a result of a move. The base logic is: foreach var item
        /// 	</summary>
        /// </oldcomments>
		///
		/// <summary>
        /// This function is a replacement for checking at the end of OznaczSzachy() to use after the move.
        /// It is checking whether the King of the same team as the param was threatened in the last turn.
        /// </summary>
        /// <param name="Team">
        /// This parameter represents the team that has done the move.
        /// </param>
        public static void OznaczSzachyPoRuchu(char Team)
        {
            OznaczSzachy();
            //DONE TODO: ADD SOME CHECKING HERE IDK
            for(int i = 0, i2 = 0; i<8; i2++)
            {
                if(i2 == 8)
                {
                    i2 = 0;
                    i++;
                    if(i >= 8)
                    {
                        continue;
                    }
                }
                //TODO: MAKE THIS CHECK BETTER
                switch(Team)
                {
                    case 'B':
                        if(SzachyBC[i, i2].HasFlag(ChessPiece.TeamC))
                        {
                            if(!(BackupSzachyBC[i, i2].HasFlag(ChessPiece.TeamC)))
                            {
                                throw new InvalidMoveException("Gracz B spowodowal szacha u siebie");
                            }
                        }
                        break;
                    case 'C':
                        if(SzachyBC[i, i2].HasFlag(ChessPiece.TeamB))
                        {
                            if(!(BackupSzachyBC[i, i2].HasFlag(ChessPiece.TeamB)))
                            {
                                throw new InvalidMoveException("Gracz C spowodowal szacha u siebie");
                            }
                        }
                        break;
                }
            }
        }
        public static void OznaczSzachy()
        {
            //DONE: MAKE BACKUPS BEFORE REDEFINING AND CHANGE BOARDS TO THE MERGED ONES
            Array.Copy(SzachyBC, BackupSzachyBC, SzachyBC.Length);
            Array.Copy(HiddenSzachyBC, BackupHiddenSzachyBC, HiddenSzachyBC.Length);
            SzachyBC = new ChessPiece[8, 8];
            HiddenSzachyBC = new ChessPiece[8, 8];
            for(int i = 0, i2 = 0; i < 8; i2++)
            {
                if(i2 == 8)
                {
                    i2 = 0;
                    i++;
                    if(i >= 8)
                    {
                        continue;
                    }
                }
                if(!(Plansza[i, i2] == ChessPiece.None))
                {
                    //in memory :heart: switch (Plansza[i, i2].TrimEnd('1', '2', '3', '4', '5', '6', '7', '8', 'B', 'C'))
                    if(Plansza[i, i2].HasFlag(ChessPiece.Pawn))
                    {
                        // switch (Plansza[i, i2].Last()) :heart:
                        if(Plansza[i, i2].HasFlag(ChessPiece.TeamB))
                        {
                            if(!(i+1 >= 8))
                            {
                                if(!(i2-1 <= -1))
                                {
                                    if(!(Plansza[i+1, i2-1] == ChessPiece.None))
                                    {
                                        if(Plansza[i+1, i2-1].HasFlag(ChessPiece.King | ChessPiece.TeamC))
                                        {
                                            SzachyBC[i+1, i2-1] |= ChessPiece.TeamB;
                                        }
                                    }
                                    else
                                    {
                                        HiddenSzachyBC[i+1, i2-1] |= ChessPiece.TeamB;
                                    }
                                }
                                if(!(i2+1 >= 8))
                                {
                                    if(!(Plansza[i+1, i2+1] == ChessPiece.None))
                                    {
                                        if(Plansza[i+1, i2+1].HasFlag(ChessPiece.King | ChessPiece.TeamC))
                                        {
                                            SzachyBC[i+1, i2+1] |= ChessPiece.TeamB;
                                        }
                                    }
                                    else
                                    {
                                        HiddenSzachyBC[i+1, i2+1] |= ChessPiece.TeamB;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if(!(i-1 <= -1))
                            {
                                if(!(i2+1 >= 8))
                                {
                                    if(!(Plansza[i-1, i2+1] == ChessPiece.None))
                                    {
                                        if(Plansza[i-1, i2+1].HasFlag(ChessPiece.King | ChessPiece.TeamB))
                                        {
                                            SzachyBC[i-1, i2+1] |= ChessPiece.TeamC;
                                        }
                                    }
                                    else
                                    {
                                        HiddenSzachyBC[i-1, i2+1] |= ChessPiece.TeamC;
                                    }
                                }
                                if(!(i2-1 <= -1))
                                {
                                    if(!(Plansza[i-1, i2-1] == ChessPiece.None))
                                    {
                                        if(Plansza[i-1, i2-1].HasFlag(ChessPiece.King | ChessPiece.TeamB))
                                        {
                                            SzachyBC[i-1, i2-1] |= ChessPiece.TeamC;
                                        }
                                    }
                                    else
                                    {
                                        HiddenSzachyBC[i-1, i2-1] |= ChessPiece.TeamC;
                                    }
                                }
                            }
                        }
                    }
                    if(Plansza[i, i2].HasFlag(ChessPiece.Knight))
                    {
                        //switch (Plansza[i, i2].Last())
                        if(Plansza[i, i2].HasFlag(ChessPiece.TeamB))
                        {
                            if(!(i-2 <= -1))
                            {
                                if(!(i2-1 <= -1))
                                {
                                    if(!(Plansza[i-2, i2-1] == ChessPiece.None))
                                    {
                                        if(Plansza[i-2, i2-1].HasFlag(ChessPiece.King | ChessPiece.TeamC))
                                        {
                                            SzachyBC[i-2, i2-1] |= ChessPiece.TeamB;
                                        }
                                    }
                                    else
                                    {
                                        HiddenSzachyBC[i-2, i2-1] |= ChessPiece.TeamB;
                                    }
                                }
                                if(!(i2+1 >= 8))
                                {
                                    if(!(Plansza[i-2, i2+1] == ChessPiece.None))
                                    {
                                        if(Plansza[i-2, i2+1].HasFlag(ChessPiece.King | ChessPiece.TeamC))
                                        {
                                            SzachyBC[i-2, i2+1] |= ChessPiece.TeamB;
                                        }
                                    }
                                    else
                                    {
                                        HiddenSzachyBC[i-2, i2+1] |= ChessPiece.TeamB;
                                    }
                                }
                            }
                            if(!(i-1 <= -1))
                            {
                                if(!(i2-2 <= -1))
                                {
                                    if(!(Plansza[i-1, i2-2] == ChessPiece.None))
                                    {
                                        if(Plansza[i-1, i2-2].HasFlag(ChessPiece.King | ChessPiece.TeamC))
                                        {
                                            SzachyBC[i-1, i2-2] = ChessPiece.TeamB;
                                        }
                                    }
                                    else
                                    {
                                        HiddenSzachyBC[i-1, i2-2] |= ChessPiece.TeamB;
                                    }
                                }
                                if(!(i2+2 >= 8))
                                {
                                    if(!(Plansza[i-1, i2+2] == ChessPiece.None))
                                    {
                                        if(Plansza[i-1, i2+2].HasFlag(ChessPiece.King | ChessPiece.TeamC))
                                        {
                                            SzachyBC[i-1, i2+2] |= ChessPiece.TeamB;
                                        }
                                    }
                                    else
                                    {
                                        HiddenSzachyBC[i-1, i2+2] |= ChessPiece.TeamB;
                                    }
                                }
                            }
                            if(!(i+2 >= 8))
                            {
                                if(!(i2-1 <= -1))
                                {
                                    if(!(Plansza[i+2, i2-1] == ChessPiece.None))
                                    {
                                        if(Plansza[i+2, i2-1].HasFlag(ChessPiece.King | ChessPiece.TeamC))
                                        {
                                            SzachyBC[i+2, i2-1] |= ChessPiece.TeamB;
                                        }
                                    }
                                    else
                                    {
                                        HiddenSzachyBC[i+2, i2-1] |= ChessPiece.TeamB;
                                    }
                                }
                                if(!(i2+1 >= 8))
                                {
                                    if(!(Plansza[i+2, i2+1] == ChessPiece.None))
                                    {
                                        if(Plansza[i+2, i2+1].HasFlag(ChessPiece.King | ChessPiece.TeamC))
                                        {
                                            SzachyBC[i+2, i2+1] |= ChessPiece.TeamB;
                                        }
                                    }
                                    else
                                    {
                                        HiddenSzachyBC[i+2, i2+1] |= ChessPiece.TeamB;
                                    }
                                }
                            }
                            if(!(i+1 >= 8))
                            {
                                if(!(i2-2 <= -1))
                                {
                                    if(!(Plansza[i+1, i2-2] == ChessPiece.None))
                                    {
                                        if(Plansza[i+1, i2-2].HasFlag(ChessPiece.King | ChessPiece.TeamC))
                                        {
                                            SzachyBC[i+1, i2-2] |= ChessPiece.TeamB;
                                        }
                                    }
                                    else
                                    {
                                        HiddenSzachyBC[i+1, i2-2] |= ChessPiece.TeamB;
                                    }
                                }
                                if(!(i2+2 >= 8))
                                {
                                    if(!(Plansza[i+1, i2+2] == ChessPiece.None))
                                    {
                                        if(Plansza[i+1, i2+2].HasFlag(ChessPiece.King | ChessPiece.TeamC))
                                        {
                                            SzachyBC[i+1, i2+2] |= ChessPiece.TeamB;
                                        }
                                    }
                                    else
                                    {
                                        HiddenSzachyBC[i+1, i2+2] |= ChessPiece.TeamB;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if(!(i-2 <= -1))
                            {
                                if(!(i2-1 <= -1))
                                {
                                    if(!(Plansza[i-2, i2-1] == ChessPiece.None))
                                    {
                                        if(Plansza[i-2, i2-1].HasFlag(ChessPiece.King | ChessPiece.TeamB))
                                        {
                                            SzachyBC[i-2, i2-1] |= ChessPiece.TeamC;
                                        }
                                    }
                                    else
                                    {
                                        HiddenSzachyBC[i-2, i2-1] |= ChessPiece.TeamC;
                                    }
                                }
                                if(!(i2+1 >= 8))
                                {
                                    if(!(Plansza[i-2, i2+1] == ChessPiece.None))
                                    {
                                        if(Plansza[i-2, i2+1].HasFlag(ChessPiece.King | ChessPiece.TeamB))
                                        {
                                            SzachyBC[i-2, i2+1] |= ChessPiece.TeamC;
                                        }
                                    }
                                    else
                                    {
                                        SzachyBC[i-2, i2+1] |= ChessPiece.TeamC;
                                    }
                                }
                            }
                            if(!(i-1 <= -1))
                            {
                                if(!(i2-2 <= -1))
                                {
                                    if(!(Plansza[i-1, i2-2] == ChessPiece.None))
                                    {
                                        if(Plansza[i-1, i2-2].HasFlag(ChessPiece.King | ChessPiece.TeamB))
                                        {
                                            SzachyBC[i-1, i2-2] |= ChessPiece.TeamC;
                                        }
                                    }
                                    else
                                    {
                                        HiddenSzachyBC[i-1, i2-2] |= ChessPiece.TeamC;
                                    }
                                }
                                if(!(i2+2 >= 8))
                                {
                                    if(!(Plansza[i-1, i2+2] == ChessPiece.None))
                                    {
                                        if(Plansza[i-1, i2+2].HasFlag(ChessPiece.King | ChessPiece.TeamB))
                                        {
                                            SzachyBC[i-1, i2+2] |= ChessPiece.TeamC;
                                        }
                                    }
                                    else
                                    {
                                        HiddenSzachyBC[i-1, i2+2] |= ChessPiece.TeamC;
                                    }
                                }
                            }
                            if(!(i+2 >= 8))
                            {
                                if(!(i2-1 <= -1))
                                {
                                    if(!(Plansza[i+2, i2-1] == ChessPiece.None))
                                    {
                                        if(Plansza[i+2, i2-1].HasFlag(ChessPiece.King | ChessPiece.TeamB))
                                        {
                                            SzachyBC[i+2, i2-1] |= ChessPiece.TeamC;
                                        }
                                    }
                                    else
                                    {
                                        HiddenSzachyBC[i+2, i2-1] |= ChessPiece.TeamC;
                                    }
                                }
                                if(!(i2+1 >= 8))
                                {
                                    if(!(Plansza[i+2, i2+1] == ChessPiece.None))
                                    {
                                        if(Plansza[i+2, i2+1].HasFlag(ChessPiece.King | ChessPiece.TeamB))
                                        {
                                            SzachyBC[i+2, i2+1] |= ChessPiece.TeamC;
                                        }
                                    }
                                    else
                                    {
                                        HiddenSzachyBC[i+2, i2+1] |= ChessPiece.TeamC;
                                    }
                                }
                            }
                            if(!(i+1 >= 8))
                            {
                                if(!(i2-2 <= -1))
                                {
                                    if(!(Plansza[i+1, i2-2] == ChessPiece.None))
                                    {
                                        if(Plansza[i+1, i2-2].HasFlag(ChessPiece.King | ChessPiece.TeamB))
                                        {
                                            SzachyBC[i+1, i2-2] |= ChessPiece.TeamC;
                                        }
                                    }
                                    else
                                    {
                                        HiddenSzachyBC[i+1, i2-2] |= ChessPiece.TeamC;
                                    }
                                }
                                if(!(i2+2 >= 8))
                                {
                                    if(!(Plansza[i+1, i2+2] == ChessPiece.None))
                                    {
                                        if(Plansza[i+1, i2+2].HasFlag(ChessPiece.King | ChessPiece.TeamB))
                                        {
                                            SzachyBC[i+1, i2+2] |= ChessPiece.TeamC;
                                        }
                                    }
                                    else
                                    {
                                        HiddenSzachyBC[i+1, i2+2] |= ChessPiece.TeamC;
                                    }
                                }
                            }
                        }
                    }
                    if(Plansza[i, i2].HasFlag(ChessPiece.Bishop))
                    {
                        void CheckBishopRightUp(int _i, int _i2, char Team)
                        {
                            if((_i > -1) && (_i2 < 8))
                            {
                                if(!(Plansza[_i, _i2] == ChessPiece.None))
                                {
                                    if(Plansza[_i, _i2].HasFlag(ChessPiece.King))
                                    {
                                        switch(Team)
                                        {
                                            case 'B':
                                                if(Plansza[_i, _i2].HasFlag(ChessPiece.TeamC))
                                                {
                                                    SzachyBC[_i, _i2] |= ChessPiece.TeamB;
                                                }
                                                break;
                                            case 'C':
                                                if(Plansza[_i, _i2].HasFlag(ChessPiece.TeamB))
                                                {
                                                    SzachyBC[_i, _i2] |= ChessPiece.TeamC;
                                                }
                                                break;
                                        }
                                    }
                                }
                                else
                                {
                                    switch(Team)
                                    {
                                        case 'B':
                                            HiddenSzachyBC[_i, _i2] |= ChessPiece.TeamB;
                                            break;
                                        case 'C':
                                            HiddenSzachyBC[_i, _i2] |= ChessPiece.TeamC;
                                            break;
                                    }
                                    CheckBishopRightUp(_i-1, _i2+1, Team);
                                }
                            }
                            return;
                        }
                        void CheckBishopRightDown(int _i, int _i2, char Team)
                        {
                            if(!(_i >= 8) && !(_i2 >= 8))
                            {
                                if(!(Plansza[_i, _i2] == ChessPiece.None))
                                {
                                    if(Plansza[_i, _i2].HasFlag(ChessPiece.King))
                                    {
                                        switch(Team)
                                        {
                                            case 'B':
                                                if(Plansza[_i, _i2].HasFlag(ChessPiece.TeamC))
                                                {
                                                    SzachyBC[_i, _i2] |= ChessPiece.TeamB;
                                                }
                                                break;
                                            case 'C':
                                                if(Plansza[_i, _i2].HasFlag(ChessPiece.TeamB))
                                                {
                                                    SzachyBC[_i, _i2] |= ChessPiece.TeamC;
                                                }
                                                break;
                                        }
                                    }
                                }
                                else
                                {
                                    switch(Team)
                                    {
                                        case 'B':
                                            HiddenSzachyBC[_i, _i2] |= ChessPiece.TeamB;
                                            break;
                                        case 'C':
                                            HiddenSzachyBC[_i, _i2] |= ChessPiece.TeamC;
                                            break;
                                    }
                                    CheckBishopRightDown(_i+1, _i2+1, Team);
                                }
                            }
                            return;
                        }
                        void CheckBishopLeftUp(int _i, int _i2, char Team)
                        {
                            if(!(_i <= -1) && !(_i2 <= -1))
                            {
                                if(!(Plansza[_i, _i2] == ChessPiece.None))
                                {
                                    if(Plansza[_i, _i2].HasFlag(ChessPiece.King))
                                    {
                                        switch(Team)
                                        {
                                            case 'B':
                                                if(Plansza[_i, _i2].HasFlag(ChessPiece.TeamC))
                                                {
                                                    SzachyBC[_i, _i2] |= ChessPiece.TeamB;
                                                }
                                                break;
                                            case 'C':
                                                if(Plansza[_i, _i2].HasFlag(ChessPiece.TeamB))
                                                {
                                                    SzachyBC[_i, _i2] |= ChessPiece.TeamC;
                                                }
                                                break;
                                        }
                                    }
                                }
                                else
                                {
                                    switch(Team)
                                    {
                                        case 'B':
                                            HiddenSzachyBC[_i, _i2] |= ChessPiece.TeamB;
                                            break;
                                        case 'C':
                                            HiddenSzachyBC[_i, _i2] |= ChessPiece.TeamC;
                                            break;
                                    }
                                    CheckBishopLeftUp(_i-1, _i2-1, Team);
                                }
                            }
                            return;
                        }
                        void CheckBishopLeftDown(int _i, int _i2, char Team)
                        {
                            //costam
                            //CheckBishopLeftDown() aż nie będzie można
                            if(!(_i >= 8) && !(_i2 <= -1))
                            {
                                if(!(Plansza[_i, _i2] == ChessPiece.None))
                                {
                                    if(Plansza[_i, _i2].HasFlag(ChessPiece.King)) //Plansza[_i, _i2].StartsWith("krol")
                                    {
                                        switch(Team)
                                        {
                                            case 'B':
                                                if(Plansza[_i, _i2].HasFlag(ChessPiece.TeamC)) //Plansza[_i, _i2].Last() == 'C'
                                                {
                                                    SzachyBC[_i, _i2] |= ChessPiece.TeamB;
                                                }
                                                break;
                                            case 'C':
                                                if(Plansza[_i, _i2].HasFlag(ChessPiece.TeamB)) //Plansza[_i, _i2].Last() == 'B'
                                                {
                                                    SzachyBC[_i, _i2] |= ChessPiece.TeamC;
                                                }
                                                break;
                                        }
                                    }
                                }
                                else
                                {
                                    switch(Team)
                                    {
                                        case 'B':
                                            HiddenSzachyBC[_i, _i2] |= ChessPiece.TeamB;
                                            break;
                                        case 'C':
                                            HiddenSzachyBC[_i, _i2] |= ChessPiece.TeamC;
                                            break;
                                    }
                                    CheckBishopLeftDown(_i+1, _i2-1, Team);
                                }
                            }
                            return;
                        }
                        if(Plansza[i, i2].HasFlag(ChessPiece.TeamB))
                        {
                            // I did not understand async when writing this comment so it's kinda irrelevant
                            // 
                            // Guess who can't use async
                            // uwu
                            Thread CheckRightUpB = new Thread(() => CheckBishopRightUp(i-1, i2+1, 'B'));
                            Thread CheckRightDownB = new Thread(() => CheckBishopRightDown(i+1, i2+1, 'B'));
                            Thread CheckLeftUpB = new Thread(() => CheckBishopLeftUp(i-1, i2-1, 'B'));
                            Thread CheckLeftDownB = new Thread(() => CheckBishopLeftDown(i+1, i2-1, 'B'));
                            CheckRightUpB.Start();
                            CheckRightDownB.Start();
                            CheckLeftUpB.Start();
                            CheckLeftDownB.Start();
                            CheckRightUpB.Join();
                            CheckRightDownB.Join();
                            CheckLeftUpB.Join();
                            CheckLeftDownB.Join();
                        }
                        else if(Plansza[i, i2].HasFlag(ChessPiece.TeamC))
                        {
                            Thread CheckRightUpC = new Thread(() => CheckBishopRightUp(i-1, i2+1, 'C'));
                            Thread CheckRightDownC = new Thread(() => CheckBishopRightDown(i+1, i2+1, 'C'));
                            Thread CheckLeftUpC = new Thread(() => CheckBishopLeftUp(i-1, i2-1, 'C'));
                            Thread CheckLeftDownC = new Thread(() => CheckBishopLeftDown(i+1, i2-1, 'C'));
                            CheckRightUpC.Start();
                            CheckRightDownC.Start();
                            CheckLeftUpC.Start();
                            CheckLeftDownC.Start();
                            CheckRightUpC.Join();
                            CheckRightDownC.Join();
                            CheckLeftUpC.Join();
                            CheckLeftDownC.Join();
                        }
                    }
                    if(Plansza[i, i2].HasFlag(ChessPiece.Rook))
                    {
                        void CheckRookUp(int _i, int _i2, char Team)
                        {
                            if(_i > -1)
                            {
                                if(!(Plansza[_i, _i2] == ChessPiece.None))
                                {
                                    if(Plansza[_i, _i2].HasFlag(ChessPiece.King))
                                    {
                                        switch(Team)
                                        {
                                            case 'B':
                                                if(Plansza[_i, _i2].HasFlag(ChessPiece.TeamC))
                                                {
                                                    SzachyBC[_i, _i2] |= ChessPiece.TeamB;
                                                }
                                                break;
                                            case 'C':
                                                if(Plansza[_i, _i2].HasFlag(ChessPiece.TeamB))
                                                {
                                                    SzachyBC[_i, _i2] |= ChessPiece.TeamC;
                                                }
                                                break;
                                        }
                                    }
                                }
                                else
                                {
                                    switch(Team)
                                    {
                                        case 'B':
                                            HiddenSzachyBC[_i, _i2] |= ChessPiece.TeamB;
                                            break;
                                        case 'C':
                                            HiddenSzachyBC[_i, _i2] |= ChessPiece.TeamC;
                                            break;
                                    }
                                    CheckRookUp(_i-1, _i2, Team);
                                }
                            }
                            return;
                        }
                        void CheckRookRight(int _i, int _i2, char Team)
                        {
                            if(_i2 < 8)
                            {
                                if(!(Plansza[_i, _i2] == ChessPiece.None))
                                {
                                    if(Plansza[_i, _i2].HasFlag(ChessPiece.King))
                                    {
                                        switch(Team)
                                        {
                                            case 'B':
                                                if(Plansza[_i, _i2].HasFlag(ChessPiece.TeamC))
                                                {
                                                    SzachyBC[_i, _i2] |= ChessPiece.TeamB;
                                                }
                                                break;
                                            case 'C':
                                                if(Plansza[_i, _i2].HasFlag(ChessPiece.TeamB))
                                                {
                                                    SzachyBC[_i, _i2] |= ChessPiece.TeamC;
                                                }
                                                break;
                                        }
                                    }
                                }
                                else
                                {
                                    switch(Team)
                                    {
                                        case 'B':
                                            HiddenSzachyBC[_i, _i2] |= ChessPiece.TeamB;
                                            break;
                                        case 'C':
                                            HiddenSzachyBC[_i, _i2] |= ChessPiece.TeamC;
                                            break;
                                    }
                                    CheckRookRight(_i, _i2+1, Team);
                                }
                            }
                            return;
                        }
                        void CheckRookLeft(int _i, int _i2, char Team)
                        {
                            if(_i2 > -1)
                            {
                                if(!(Plansza[_i, _i2] == ChessPiece.None))
                                {
                                    if(Plansza[_i, _i2].HasFlag(ChessPiece.King))
                                    {
                                        switch(Team)
                                        {
                                            case 'B':
                                                if(Plansza[_i, _i2].HasFlag(ChessPiece.TeamC))
                                                {
                                                    SzachyBC[_i, _i2] |= ChessPiece.TeamB;
                                                }
                                                break;
                                            case 'C':
                                                if(Plansza[_i, _i2].HasFlag(ChessPiece.TeamB))
                                                {
                                                    SzachyBC[_i, _i2] |= ChessPiece.TeamC;
                                                }
                                                break;
                                        }
                                    }
                                }
                                else
                                {
                                    switch(Team)
                                    {
                                        case 'B':
                                            HiddenSzachyBC[_i, _i2] |= ChessPiece.TeamB;
                                            break;
                                        case 'C':
                                            HiddenSzachyBC[_i, _i2] |= ChessPiece.TeamC;
                                            break;
                                    }
                                    CheckRookLeft(_i, _i2-1, Team);
                                }
                            }
                            return;
                        }
                        void CheckRookDown(int _i, int _i2, char Team)
                        {
                            if(_i < 8)
                            {
                                if(!(Plansza[_i, _i2] == ChessPiece.None))
                                {
                                    if(Plansza[_i, _i2].HasFlag(ChessPiece.King))
                                    {
                                        switch(Team)
                                        {
                                            case 'B':
                                                if(Plansza[_i, _i2].HasFlag(ChessPiece.TeamC))
                                                {
                                                    SzachyBC[_i, _i2] |= ChessPiece.TeamB;
                                                }
                                                break;
                                            case 'C':
                                                if(Plansza[_i, _i2].HasFlag(ChessPiece.TeamB))
                                                {
                                                    SzachyBC[_i, _i2] |= ChessPiece.TeamC;
                                                }
                                                break;
                                        }
                                    }
                                }
                                else
                                {
                                    switch(Team)
                                    {
                                        case 'B':
                                            HiddenSzachyBC[_i, _i2] |= ChessPiece.TeamB;
                                            break;
                                        case 'C':
                                            HiddenSzachyBC[_i, _i2] |= ChessPiece.TeamC;
                                            break;
                                    }
                                    CheckRookDown(_i+1, _i2, Team);
                                }
                            }
                            return;
                        }
                        if(Plansza[i, i2].HasFlag(ChessPiece.TeamC))
                        {
                            Thread CheckUp = new Thread(() => CheckRookUp(i-1, i2, 'C'));
                            Thread CheckRight = new Thread(() => CheckRookRight(i, i2+1, 'C'));
                            Thread CheckLeft = new Thread(() => CheckRookLeft(i, i2-1, 'C'));
                            Thread CheckDown = new Thread(() => CheckRookDown(i+1, i2, 'C'));
                            CheckUp.Start();
                            CheckRight.Start();
                            CheckLeft.Start();
                            CheckDown.Start();
                            CheckUp.Join();
                            CheckRight.Join();
                            CheckLeft.Join();
                            CheckDown.Join();
                        }
                        else
                        {
                            Thread CheckUp = new Thread(() => CheckRookUp(i-1, i2, 'B'));
                            Thread CheckRight = new Thread(() => CheckRookRight(i, i2+1, 'B'));
                            Thread CheckLeft = new Thread(() => CheckRookLeft(i, i2-1, 'B'));
                            Thread CheckDown = new Thread(() => CheckRookDown(i+1, i2, 'B'));
                            CheckUp.Start();
                            CheckRight.Start();
                            CheckLeft.Start();
                            CheckDown.Start();
                            CheckUp.Join();
                            CheckRight.Join();
                            CheckLeft.Join();
                            CheckDown.Join();
                        }
                    }
                    if(Plansza[i, i2].HasFlag(ChessPiece.King))
                    {
                        if(Plansza[i, i2].HasFlag(ChessPiece.TeamC))
                        {
                            //Check up
                            if(i-1 > -1)
                            {
                                if(Plansza[i-1, i2] == ChessPiece.None)
                                {
                                    HiddenSzachyBC[i-1, i2] |= ChessPiece.TeamC;
                                }
                            }
                            //Check right-up
                            if((i-1 > -1) && (i2+1 < 8))
                            {
                                if(Plansza[i-1, i2+1] == ChessPiece.None)
                                {
                                    HiddenSzachyBC[i-1, i2+1] |= ChessPiece.TeamC;
                                }
                            }
                            //Check right
                            if(i2 + 1 < 8)
                            {
                                if(Plansza[i, i2+1] == ChessPiece.None)
                                {
                                    HiddenSzachyBC[i, i2+1] |= ChessPiece.TeamC;
                                }
                            }
                            //Check right-down
                            if((i+1 < 8) && (i2+1 < 8))
                            {
                                if(Plansza[i+1, i2+1] == ChessPiece.None)
                                {
                                    HiddenSzachyBC[i+1, i2+1] |= ChessPiece.TeamC;
                                }
                            }
                            //Check down
                            if(i+1 < 8)
                            {
                                if(Plansza[i+1, i2] == ChessPiece.None)
                                {
                                    HiddenSzachyBC[i+1, i2] |= ChessPiece.TeamC;
                                }
                            }
                            //Chech left-down
                            if((i+1 < 8) && (i2-1 > -1))
                            {
                                if(Plansza[i+1, i2-1] == ChessPiece.None)
                                {
                                    HiddenSzachyBC[i+1, i2-1] |= ChessPiece.TeamC;
                                }
                            }
                            //Check left
                            if(i2-1 > -1)
                            {
                                if(Plansza[i, i2-1] == ChessPiece.None)
                                {
                                    HiddenSzachyBC[i, i2-1] |= ChessPiece.TeamC;
                                }
                            }
                            //Check left-up
                            if((i-1 > -1) && (i2-1 > -1))
                            {
                                if(Plansza[i-1, i2-1] == ChessPiece.None)
                                {
                                    HiddenSzachyBC[i-1, i2-1] |= ChessPiece.TeamC;
                                }
                            }
                        }
                        else
                        {
                            //Check up
                            if(i-1 > -1)
                            {
                                if(Plansza[i-1, i2] == ChessPiece.None)
                                {
                                    HiddenSzachyBC[i-1, i2] |= ChessPiece.TeamB;
                                }
                            }
                            //Check right-up
                            if((i-1 > -1) && (i2+1 < 8))
                            {
                                if(Plansza[i-1, i2+1] == ChessPiece.None)
                                {
                                    HiddenSzachyBC[i-1, i2+1] |= ChessPiece.TeamB;
                                }
                            }
                            //Check right
                            if(i2+1 < 8)
                            {
                                if(Plansza[i, i2+1] == ChessPiece.None)
                                {
                                    HiddenSzachyBC[i, i2+1] |= ChessPiece.None;
                                }
                            }
                            //Check right-down
                            if((i+1 < 8) && (i2+1 < 8))
                            {
                                if(Plansza[i+1, i2+1] == ChessPiece.None)
                                {
                                    HiddenSzachyBC[i, i2+1] |= ChessPiece.None;
                                }
                            }
                            //Check down
                            if(i+1 < 8)
                            {
                                if(Plansza[i+1, i2] == ChessPiece.None)
                                {
                                    HiddenSzachyBC[i+1, i2] |= ChessPiece.None;
                                }
                            }
                            //Check left-down
                            if((i+1 < 8) && (i2-1 > -1))
                            {
                                if(Plansza[i+1, i2-1] == ChessPiece.None)
                                {
                                    HiddenSzachyBC[i+1, i2-1] |= ChessPiece.None;
                                }
                            }
                            //Check left
                            if(i2-1 > -1)
                            {
                                if(Plansza[i, i2-1] == ChessPiece.None)
                                {
                                    HiddenSzachyBC[i, i2-1] |= ChessPiece.None;
                                }
                            }
                            //Check left-up
                            if((i-1 > -1) && (i2-1 > -1))
                            {
                                if(Plansza[i-1, i2-1] == ChessPiece.None)
                                {
                                    HiddenSzachyBC[i-1, i2-1] |= ChessPiece.None;
                                }
                            }
                            //DONE TODO: Finish this without copying!
                        }
                    }
					
                    /// <summary> 
                    /// Queen is now marked as Rook | Bishop, therefore she does not have a separate case/if
                    /// </summary>
                    /*
                        case "krolowa":
                        switch (Plansza[i, i2].Last())
                        {
                            case 'B':
                                break;
                            case 'C':
                                break;
                        }
                        break;*/
                }
                ///<remarks>
                ///TODO: PAMIĘTAJ O SPRAWDZANIU CZY KRÓL MA SZACHA U OBU DRUŻYN
                ///TODO: JEŚLI TAK TO PRZYWRÓĆ BACKUP I GŁÓWNEGO I HIDDEN
                ///</remarks>
            }
        }
        public static void NarysujPlansze()
        {
            Console.Write("\n\t    A       B       C       D       E       F       G       H\n");
            //\t = 8 *'-'
            //TODO: dodać rysowanie w drugą stronę i numerowanie pól! Done but keeping this for love and affection ❤︎
            //65 - "-----------------------------------------------------------------" (7*8 + 9)
            if(Program.playerTeam == 'C')
            {
                for(int linia = 0, x = 0; linia <= 7; linia++)
                {
                    Console.WriteLine("\t-----------------------------------------------------------------");
                    Console.Write("\t|");
                    while(x < 8)
                    {
                        if(Plansza[linia, x].HasFlag(ChessPiece.King | ChessPiece.TeamC))
                        {
                            if(SzachyBC[linia, x].HasFlag(ChessPiece.TeamB))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write(" szach!");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write("|");
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
                    while(x < 8)
                    {
                        if(!(Plansza[linia, x] == ChessPiece.None))
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
                    while(x < 8)
                    {
                        if(Plansza[linia, x].HasFlag(ChessPiece.King | ChessPiece.TeamB))
                        {
                            if(SzachyBC[linia, x].HasFlag(ChessPiece.TeamC))
                            {
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.Write(" szach!");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write("|");
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
                }
                //Console.WriteLine("|       |       |       |       |       |       |       |       |");
            }
            else
            {
                for(int linia = 7, x = 0; linia > -1; linia--)
                {
                    Console.WriteLine("\t-----------------------------------------------------------------");
                    Console.Write("\t|");
                    while(x < 8)
                    {
                        if(Plansza[linia, x].HasFlag(ChessPiece.King | ChessPiece.TeamB))
                        {
                            if(SzachyBC[linia, x].HasFlag(ChessPiece.TeamC))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write(" szach!");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write("|");
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
                    while(x < 8)
                    {
                        if(!(Plansza[linia, x] == ChessPiece.None))
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
                    while(x < 8)
                    {
                        if(Plansza[linia, x].HasFlag(ChessPiece.King | ChessPiece.TeamC))
                        {
                            if(SzachyBC[linia, x].HasFlag(ChessPiece.TeamB))
                            {
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.Write(" szach!");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write("|");
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
                }
            }
            Console.WriteLine("\t-----------------------------------------------------------------");

        }
        public static void NapiszPionek(int pos1, int pos2)
        {
            switch(Plansza[pos1, pos2] & ChessPiece.AllPieces) // In memory of the dear Pieces ❤︎ (Plansza[pos1, pos2].TrimEnd('1', '2', '3', '4', '5', '6', '7', '8', 'C', 'B'))
            {
                case ChessPiece.Pawn: //"pionek"
                    Console.ForegroundColor = (ConsoleColor)((int)ConsoleColor.DarkGreen + (Plansza[pos1, pos2].HasFlag(ChessPiece.TeamB) ? 8 : 0));
                    Console.Write("pionek ");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case ChessPiece.Rook: //"wieza"
                    Console.ForegroundColor = (ConsoleColor)((int)ConsoleColor.DarkBlue + (Plansza[pos1, pos2].HasFlag(ChessPiece.TeamB) ? 8 : 0));
                    Console.Write(" wieza ");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case ChessPiece.Knight: //"kon"
                    Console.ForegroundColor = (ConsoleColor)((int)ConsoleColor.DarkYellow + (Plansza[pos1, pos2].HasFlag(ChessPiece.TeamB) ? 8 : 0));
                    Console.Write("  kon  ");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case ChessPiece.Bishop: //"goniec"
                    Console.ForegroundColor = (ConsoleColor)((int)ConsoleColor.DarkCyan + (Plansza[pos1, pos2].HasFlag(ChessPiece.TeamB) ? 8 : 0));
                    Console.Write("goniec ");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case ChessPiece.Queen: //"krolowa"
                    Console.ForegroundColor = (ConsoleColor)((int)ConsoleColor.DarkMagenta + (Plansza[pos1, pos2].HasFlag(ChessPiece.TeamB) ? 8 : 0));
                    Console.Write("krolowa");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case ChessPiece.King: //"krol"
                    Console.ForegroundColor = (ConsoleColor)((int)ConsoleColor.DarkGray + (Plansza[pos1, pos2].HasFlag(ChessPiece.TeamB) ? -1 : 0));
                    Console.Write(" krol  ");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
        }
        /// <summary>
        /// The point of this method being semi-separate is to raise an alarm after the enemy client let trough an invalid move.
        /// </summary>
        /// <returns>Either returns an exception or a void if move is incorrect</returns>
        public static void WykonajISprawdzRuchPrzeciwnika(Pozycja Pozycja1, Pozycja Pozycja2)
        {
            try
            {
                WykonajRuch(Pozycja1, Pozycja2, Program.enemyNick, Program.enemyTeam);
                Program.hasEnemyMoved = true;
                return;
            }
            catch(InvalidMoveException e)
            {
                //InvalidMoveException will be thrown to here if the move is invalid
                throw;
            }
        }
        public static void WykonajRuch(Pozycja Pozycja1, Pozycja Pozycja2, string nick, char curTeam)
        {
            if(Pozycja1.Pos1 == Pozycja2.Pos1 && Pozycja1.Pos2 == Pozycja2.Pos2)
            {
                throw new InvalidMoveException("Pionek sprobowal porzuszyc sie na to samo pole");
            }
            if(Plansza[Pozycja1.Pos1, Pozycja1.Pos2] == ChessPiece.None)
            {
                throw new InvalidMoveException("Na tym polu nie ma zadnego pionka", "empty");
            }
            char Pos1team = (Plansza[Pozycja1.Pos1, Pozycja1.Pos2] & ChessPiece.BothTeams) == ChessPiece.TeamB ? 'B' : 'C'; // hasflag ? yes : no
            if(!(Pos1team == curTeam))
            {
                throw new InvalidMoveException("Pionek nie byl tej samej druzyny", "other");
            }
            Array.Copy(PossibleEnPassants, BackupPossibleEnPassants, PossibleEnPassants.Length);
            //Ily pieces, in memory I'm keeping this. string figureType = Plansza[Pozycja1.Pos1, Pozycja1.Pos2].TrimEnd('1', '2', '3', '4', '5', '6', '7', '8', 'B', 'C');
            try
            {
                //switch(Plansza[Pozycja1.Pos1, Pozycja1.Pos2] & ChessPiece.AllPieces)
                //These functions should throw the InvalidEnemyMoveExcpetion(MoveInfo) with some info about the error that will get logged and told to the player
                /*try
                {
                    if(Plansza[Pozycja1.Pos1, Pozycja1.Pos2].HasFlag(ChessPiece.Queen)) //"krolowa"
                    {
                        RuchKrólową(Pozycja1, Pozycja2, Pos1team);
                    }
                    else
                    {
                        throw new InvalidMoveException();
                    }
                }
                catch
                {
                }*/
                try
                {
                    if(Plansza[Pozycja1.Pos1, Pozycja1.Pos2].HasFlag(ChessPiece.Bishop)) //case ChessPiece.Bishop: //"goniec"
                    {
                        RuchGońcem(Pozycja1, Pozycja2, Pos1team);
                    }
                    else
                    {
                        throw new InvalidMoveException();
                    }
                }
                catch
                {
                    try
                    {
                        //case ChessPiece.Rook: //"wieza"
                        if(Plansza[Pozycja1.Pos1, Pozycja1.Pos2].HasFlag(ChessPiece.Rook))
                        {
                            RuchWieżą(Pozycja1, Pozycja2, Pos1team);
                        }
                        else
                        {
                            throw;
                        }
                    }
                    catch
                    {
                        try
                        {
                            if(Plansza[Pozycja1.Pos1, Pozycja1.Pos2].HasFlag(ChessPiece.Pawn)) //"pionek"
                            {
                                RuchPionem(Pozycja1, Pozycja2, Pos1team);
                            }
                            else
                            {
                                throw;
                            }
                        }
                        catch
                        {
                            try
                            {
                                //case ChessPiece.Knight: //"kon"
                                if(Plansza[Pozycja1.Pos1, Pozycja1.Pos2].HasFlag(ChessPiece.Knight))
                                {
                                    RuchKoniem(Pozycja1, Pozycja2, Pos1team);
                                }
                                else
                                {
                                    throw;
                                }
                            }
                            catch
                            {
                                if(Plansza[Pozycja1.Pos1, Pozycja1.Pos2].HasFlag(ChessPiece.King)) //"krol"
                                {
                                    RuchKrólem(Pozycja1, Pozycja2, Pos1team);
                                }
                                else
                                {
                                    throw;
                                }
                            }
                        }
                    }
                }
                OznaczSzachyPoRuchu(Pos1team);
                //TODO: After validation Program.AppendText($"{nick}: {Pozycja1.ToString()} >> {Pozycja2.ToString()}", Program.currentfolder, Program.ID);
                if(nick == Program.Nick)
                {
                    Program.AppendText($"{nick}: {Pozycja1.ToString()} >> {Pozycja2.ToString()}", Program.currentfolder, Program.ID);
                }
                return;
            }
            catch(InvalidMoveException InvalidMove)
            {
                //TODO: Wyslij do jakiegoś logu błędów na serwerze i powiadom użytkownika, jakoś cofnij ruch może/przerwij sesję
                Array.Copy(BackupPlansza, Plansza, Plansza.Length);
                Array.Copy(BackupSzachyBC, SzachyBC, SzachyBC.Length);
                Array.Copy(BackupHiddenSzachyBC, HiddenSzachyBC, HiddenSzachyBC.Length);
                Array.Copy(BackupPossibleEnPassants, PossibleEnPassants, PossibleEnPassants.Length);
                throw;
            }
        }
        public static void RuchPionem(Pozycja Pozycja1, Pozycja Pozycja2, Char Team)
        {
            Array.Copy(Plansza, BackupPlansza, Plansza.Length);
            switch(Team)
            {
                case 'B':
                    if(Pozycja1.Pos1+1 < 8)
                    {
                        //Warunek dla ruchu o 2
                        if(Pozycja2.Pos1 == Pozycja1.Pos1+2)
                        {
                            if(Pozycja1.Pos1 == 1)
                            {
                                if(Pozycja1.Pos2 == Pozycja2.Pos2)
                                {
                                    if(Plansza[Pozycja1.Pos1+1,Pozycja1.Pos2] == ChessPiece.None)
                                    {
                                        if(Plansza[Pozycja1.Pos1+2,Pozycja1.Pos2] == ChessPiece.None)
                                        {
                                            //Wykonaj ruch i potem sprawdz czy szacha ni ma
                                            Plansza[Pozycja2.Pos1, Pozycja2.Pos2] = Plansza[Pozycja1.Pos1, Pozycja1.Pos2];
                                            Plansza[Pozycja1.Pos1, Pozycja1.Pos2] = ChessPiece.None;
                                            PossibleEnPassants[Pozycja2.Pos1,Pozycja2.Pos2] = ChessPiece.TeamB;
                                            //TODO: Usuń możliwy en passant kiedy znowu jest tura białego zaraz na początku tury forem!
                                        }
                                        else
                                        {
                                            throw new InvalidMoveException("Pionek B probowal poruszyc sie o 2 pola w miejsce gdzie stoi inny pionek");
                                        }
                                    }
                                    else
                                    {
                                        throw new InvalidMoveException("Pionek B probowal poruszyc sie o 2 pola przez pionek");
                                    }
                                }
                                else
                                {
                                    throw new InvalidMoveException("Pionek B probowal sie ruszyc o 2 pola i w bok");
                                }
                            }
                            else
                            {   
                                throw new InvalidMoveException("Pionek B probowal sie ruszyc o 2 pola nie stojac w pozycji startowej");
                            }
                        }
                        //Warunek dla bicia
                        else if(Pozycja2.Pos2 == Pozycja1.Pos2 - 1 || Pozycja2.Pos2 == Pozycja1.Pos2 + 1)
                        {
                            if(Pozycja2.Pos1 == Pozycja1.Pos1 + 1)
                            {
                                //W lewo
                                if(Pozycja2.Pos2 == Pozycja1.Pos2 + 1)
                                {
                                    if(Pozycja2.Pos2 < 8)
                                    {
                                        if(Plansza[Pozycja2.Pos1, Pozycja2.Pos2] != ChessPiece.None)
                                        {
                                            if(!Plansza[Pozycja2.Pos1, Pozycja2.Pos2].HasFlag(ChessPiece.King))
                                            {
                                                //Zbij
                                                Plansza[Pozycja2.Pos1, Pozycja2.Pos2] = Plansza[Pozycja1.Pos1, Pozycja1.Pos2];
                                                Plansza[Pozycja1.Pos1, Pozycja1.Pos2] = ChessPiece.None;
                                            }
                                            else
                                            {
                                                throw new InvalidMoveException("Pionek B probowal zbic krola w lewo");
                                            }
                                        }
                                        else if(PossibleEnPassants[Pozycja2.Pos1-1, Pozycja2.Pos2] == ChessPiece.TeamC)
                                        {
                                            //Wykonaj en passant
                                            Plansza[Pozycja2.Pos1, Pozycja2.Pos2] = Plansza[Pozycja1.Pos1, Pozycja1.Pos2];
                                            Plansza[Pozycja1.Pos1, Pozycja1.Pos2] = ChessPiece.None;
                                            Plansza[Pozycja2.Pos1-1, Pozycja2.Pos2] = ChessPiece.None;
                                            PossibleEnPassants[Pozycja2.Pos1-1, Pozycja2.Pos2] = ChessPiece.None;
                                        }
                                        else
                                        {
                                            throw new InvalidMoveException("Pionek B probowal sie ruszyc po skos bez zbijania (takze bez en passant)");
                                        } 
                                    }
                                    else
                                    {
                                        throw new InvalidMoveException("Pionek B probowal sie ruszyc po skosie poza plansze w lewo (Pozycja2.Pos2 > 8)");
                                    }
                                }
                                //W prawo
                                else if(Pozycja2.Pos2 == Pozycja1.Pos2 - 1)
                                {
                                    if(Pozycja2.Pos2 > -1)
                                    {
                                        if(Plansza[Pozycja2.Pos1, Pozycja2.Pos2] != ChessPiece.None)
                                        {
                                            if(!Plansza[Pozycja2.Pos1, Pozycja2.Pos2].HasFlag(ChessPiece.King))
                                            {
                                                //Zbij
                                                Plansza[Pozycja2.Pos1, Pozycja2.Pos2] = Plansza[Pozycja1.Pos1, Pozycja1.Pos2];
                                                Plansza[Pozycja1.Pos1, Pozycja1.Pos2] = ChessPiece.None;
                                            }
                                            else
                                            {
                                                throw new InvalidMoveException("Pionek B probowal zbic krola w prawo");
                                            }
                                        }
                                        else if(PossibleEnPassants[Pozycja2.Pos1-1, Pozycja2.Pos2] == ChessPiece.TeamC)
                                        {
                                            if(Plansza[Pozycja2.Pos1, Pozycja2.Pos2] == ChessPiece.None)
                                            {
                                                //Wykonaj en passant
                                                Plansza[Pozycja2.Pos1, Pozycja2.Pos2] = Plansza[Pozycja1.Pos1, Pozycja1.Pos2];
                                                Plansza[Pozycja1.Pos1, Pozycja1.Pos2] = ChessPiece.None;
                                                Plansza[Pozycja2.Pos1-1, Pozycja2.Pos2] = ChessPiece.None;
                                                PossibleEnPassants[Pozycja2.Pos1-1, Pozycja2.Pos2] = ChessPiece.None; 
                                            }
                                            else
                                            {
                                                throw new InvalidMoveException("Pionek B probowal zrobic en passant w miejsce gdzie stoi inny pionek w prawo");
                                            }
                                        }
                                        else
                                        {
                                            throw new InvalidMoveException("Pionek B probowal sie ruszyc po skos bez zbijania (takze bez en passant)");
                                        }
                                    }
                                    else
                                    {
                                        throw new InvalidMoveException("Pionek B probowal sie ruszyc po skosie poza plansze w prawo (Pozycja2.Pos2 < -1)");
                                    }
                                }
                            }
                            else
                            {
                                throw new InvalidMoveException("Pionek B probowal sie ruszyc w bok bez ruszania sie do przodu");
                            }
                        }
                        //Warunek dla ruchu do przodu o jeden
                        else if(Pozycja2.Pos1 == Pozycja1.Pos1+1)
                        {
                            if(Pozycja2.Pos2 == Pozycja1.Pos2)
                            {
                                if(Plansza[Pozycja2.Pos1, Pozycja2.Pos2] == ChessPiece.None)
                                {
                                    Plansza[Pozycja2.Pos1, Pozycja2.Pos2] = Plansza[Pozycja1.Pos1, Pozycja1.Pos2];
                                    Plansza[Pozycja1.Pos1, Pozycja1.Pos2] = ChessPiece.None; 
                                }
                                else
                                {
                                    throw new InvalidMoveException($"Pionek B probowal sie ruszyc o jedno pole w miejsce gdzie stoi inny pionek");
                                }
                            }
                            else
                            {
                                throw new InvalidMoveException($"Pionek B probowal sie ruszyc o jedno pole w bok bez zbijania i to nie powinno sie nigdy zdarzyc bo to bylo sprawdzane wczesniej w elseifie");
                            }
                        }
                        else
                        {
                            throw new InvalidMoveException($"Jakis dziwny ruch piona B: {Pozycja1}, {Pozycja2}");
                        }
                    }
                    else
                    {
                        throw new InvalidMoveException("Pionek B probowal sie ruszyc poza plansze (Pos1+1 > 8)");
                    }
                    break;
                case 'C':
                    if(Pozycja1.Pos1-1 > -1)
                    {
                        //Warunek dla ruchu o 2
                        if(Pozycja2.Pos1 == Pozycja1.Pos1-2)
                        {
                            if(Pozycja1.Pos1 == 6)
                            {
                                if(Pozycja1.Pos2 == Pozycja2.Pos2)
                                {
                                    if(Plansza[Pozycja1.Pos1-1,Pozycja1.Pos2] == ChessPiece.None)
                                    {
                                        if(Plansza[Pozycja1.Pos1-2, Pozycja1.Pos2] == ChessPiece.None)
                                        {
                                            Plansza[Pozycja2.Pos1,Pozycja2.Pos2] = Plansza[Pozycja1.Pos1, Pozycja1.Pos2];
                                            Plansza[Pozycja1.Pos1, Pozycja1.Pos2] = ChessPiece.None;
                                            PossibleEnPassants[Pozycja2.Pos1, Pozycja2.Pos2] = ChessPiece.TeamC;
                                        }
                                        else
                                        {
                                            throw new InvalidMoveException("Pionek C probowal sie poruszyc o 2 pola w miejsce gdzie stoi pionek");
                                        }
                                    }
                                    else
                                    {
                                        throw new InvalidMoveException("Pionek C probowal poruszyc sie o 2 pola przez pionek");
                                    }
                                }
                                else
                                {
                                    throw new InvalidMoveException("Pionek C probowal sie ruszyc o 2 pola i w bok");
                                }
                            }
                            else
                            {
                                throw new InvalidMoveException("Pionek C probowal sie ruszyc o 2 pola nie stojac w pozycji startowej");
                            }
                        }
                        //Warunek dla bicia
                        else if(Pozycja2.Pos2 == Pozycja1.Pos2 - 1 || Pozycja2.Pos2 == Pozycja1.Pos2 + 1)
                        {
                            if(Pozycja2.Pos1 == Pozycja1.Pos1 - 1)
                            {
                                //W lewo
                                if(Pozycja2.Pos2 == Pozycja1.Pos2 - 1)
                                {
                                    if(Pozycja2.Pos2 > -1)
                                    {
                                        if(Plansza[Pozycja2.Pos1, Pozycja2.Pos2] != ChessPiece.None)
                                        {
                                            if(!Plansza[Pozycja2.Pos1, Pozycja2.Pos2].HasFlag(ChessPiece.King))
                                            {
                                                //Zbij
                                                Plansza[Pozycja2.Pos1, Pozycja2.Pos2] = Plansza[Pozycja1.Pos1, Pozycja1.Pos2];
                                                Plansza[Pozycja1.Pos1, Pozycja1.Pos2] = ChessPiece.None; 
                                            }
                                            else
                                            {
                                                throw new InvalidMoveException("Pionek C probowal zbic krola w lewo");
                                            }
                                        }
                                        else if(PossibleEnPassants[Pozycja2.Pos1+1, Pozycja2.Pos2] == ChessPiece.TeamB)
                                        {
                                            if(Plansza[Pozycja2.Pos1, Pozycja2.Pos2] == ChessPiece.None)
                                            {
                                                //Wykonaj en passant
                                                Plansza[Pozycja2.Pos1, Pozycja2.Pos2] = Plansza[Pozycja1.Pos1, Pozycja1.Pos2];
                                                Plansza[Pozycja1.Pos1, Pozycja1.Pos2] = ChessPiece.None;
                                                Plansza[Pozycja2.Pos1+1, Pozycja2.Pos2] = ChessPiece.None;
                                                PossibleEnPassants[Pozycja2.Pos1+1, Pozycja2.Pos2] = ChessPiece.None; 
                                            }
                                            else
                                            {
                                                throw new InvalidMoveException("Pionek C probowal zrobic en passant w miejsce gdzie stoi inny pionek w lewo");
                                            }
                                        }
                                        else
                                        {
                                            //Rzuc blad
                                            throw new InvalidMoveException("Pionek C probowal sie ruszyc po skos bez zbijania (takze bez en passant)");
                                        }
                                    }
                                    else
                                    {
                                        throw new InvalidMoveException("Pionek C probowal sie ruszyc po skosie poza plansze w lewo (Pozycja2.Pos2 <= -1");
                                    }
                                }
                                //W prawo
                                else if(Pozycja2.Pos2 == Pozycja1.Pos2 + 1)
                                {
                                    if(Plansza[Pozycja2.Pos1, Pozycja2.Pos2] != ChessPiece.None)
                                    {
                                        if(!Plansza[Pozycja2.Pos1, Pozycja2.Pos2].HasFlag(ChessPiece.King))
                                        {
                                            //Zbij
                                            Plansza[Pozycja2.Pos1, Pozycja2.Pos2] = Plansza[Pozycja1.Pos1, Pozycja1.Pos2];
                                            Plansza[Pozycja1.Pos1, Pozycja1.Pos2] = ChessPiece.None; 
                                        }
                                        else
                                        {
                                            throw new InvalidMoveException("Pionek C probowal zbic krola w prawo");
                                        }
                                    }
                                    else if(PossibleEnPassants[Pozycja2.Pos1+1, Pozycja2.Pos2] == ChessPiece.TeamB)
                                    {
                                        if(Plansza[Pozycja2.Pos1, Pozycja2.Pos2] == ChessPiece.None)
                                        {
                                            //Wykonaj en passant
                                            Plansza[Pozycja2.Pos1, Pozycja2.Pos2] = Plansza[Pozycja1.Pos1, Pozycja1.Pos2];
                                            Plansza[Pozycja1.Pos1, Pozycja1.Pos2] = ChessPiece.None;
                                            Plansza[Pozycja2.Pos1+1, Pozycja2.Pos2] = ChessPiece.None;
                                            PossibleEnPassants[Pozycja2.Pos1+1, Pozycja2.Pos2] = ChessPiece.None; 
                                        }
                                        else
                                        {
                                            throw new InvalidMoveException("Pionek C probowal zrobic en passant w miejsce gdzie stoi inny pionek w prawo");
                                        }
                                    }
                                    else
                                    {
                                        //Rzuc blad
                                        throw new InvalidMoveException("Pionek C probowal sie ruszyc po skos bez zbijania (takze bez en passant)");
                                    }
                                } 
                            }
                            else
                            {
                                throw new InvalidMoveException("Pionek C probowal ruszyc sie o w bok bez ruszania sie do przodu");
                            }
                        }
                        //Warunek dla ruchu do przodu o jeden
                        else if(Pozycja2.Pos1 == Pozycja1.Pos1 - 1)
                        {
                            if(Pozycja2.Pos2 == Pozycja1.Pos2)
                            {
                                if(Plansza[Pozycja2.Pos1,Pozycja2.Pos2] == ChessPiece.None)
                                {
                                    Plansza[Pozycja2.Pos1,Pozycja2.Pos2] = Plansza[Pozycja1.Pos1,Pozycja1.Pos2];
                                    Plansza[Pozycja1.Pos1,Pozycja1.Pos2] = ChessPiece.None;
                                }
                                else
                                {
                                    throw new InvalidMoveException($"Pionek C probowal sie ruszyc o jedno pole w miejsce gdzie stoi inny pionek");
                                }
                            }
                            else
                            {
                                throw new InvalidMoveException($"Pionek C probowal sie ruszyc o jedno pole w bok bez zbijania i to nie powinno sie nigdy zdarzyc bo to bylo sprawdzane wczesniej w elseifie");
                            }
                        }
                        else
                        {
                            throw new InvalidMoveException($"Jakis dziwny ruch piona C: {Pozycja1}, {Pozycja2}");
                        }
                    }
                    else
                    {
                        throw new InvalidMoveException("Pionek C probowal sie ruszyc poza plansze(Pos1-1 < -1");
                    }
                    break;
            }
        }
        public static void RuchKoniem(Pozycja Pozycja1, Pozycja Pozycja2, Char Team)
        {
            if(Pozycja1.Pos1 + 2 < 8 || Pozycja1.Pos1 + 1 < 8 || Pozycja1.Pos1 - 1 > -1 || Pozycja1.Pos1 - 2 > -1)
            {
                if(Pozycja2.Pos1 == Pozycja1.Pos1 + 2 || Pozycja2.Pos1 == Pozycja1.Pos1 + 1 || Pozycja2.Pos1 == Pozycja1.Pos1 - 1 || Pozycja2.Pos1 == Pozycja1.Pos1 - 2)
                {
                    // W dół o dwa
                    if(Pozycja2.Pos1 == Pozycja1.Pos1 + 2)
                    {
                        if(Pozycja1.Pos1 + 2 < 8)
                        {
                            //W lewo
                            if(Pozycja2.Pos2 == Pozycja1.Pos2 - 1)
                            {
                                if(Pozycja2.Pos2 > -1)
                                {
                                    //Zbij
                                    if(Plansza[Pozycja2.Pos1, Pozycja2.Pos2] != ChessPiece.None)
                                    {
                                        if(!Plansza[Pozycja2.Pos1, Pozycja2.Pos2].HasFlag(ChessPiece.King))
                                        {
                                            if(Plansza[Pozycja2.Pos1, Pozycja2.Pos2].HasFlag(Team == 'B' ? ChessPiece.TeamC : ChessPiece.TeamB))
                                            {
                                                Plansza[Pozycja2.Pos1, Pozycja2.Pos2] = Plansza[Pozycja1.Pos1, Pozycja1.Pos2];
                                                Plansza[Pozycja1.Pos1, Pozycja1.Pos2] = ChessPiece.None;
                                            }
                                            else
                                            {
                                                throw new InvalidMoveException($"Kon {Team} probowal zbic pionka swojej druzyny", "same");
                                            }
                                        }
                                        else
                                        {
                                            throw new InvalidMoveException($"Kon {Team} probowal zbic krola", "king");
                                        }
                                    }
                                    //Normalny ruch
                                    else
                                    {
                                        Plansza[Pozycja2.Pos1, Pozycja2.Pos2] = Plansza[Pozycja1.Pos1, Pozycja1.Pos2];
                                        Plansza[Pozycja1.Pos1, Pozycja1.Pos2] = ChessPiece.None;
                                    } 
                                }
                                else
                                {
                                    throw new InvalidMoveException($"Kon {Team} probowal poruszyc sie poza plansze w lewo");
                                }
                            }
                            //W prawo
                            else if(Pozycja2.Pos2 == Pozycja1.Pos2 + 1)
                            {
                                if(Pozycja2.Pos2 < 8)
                                {
                                    //Zbij
                                    if(Plansza[Pozycja2.Pos1, Pozycja2.Pos2] != ChessPiece.None)
                                    {
                                        if(!Plansza[Pozycja2.Pos1, Pozycja2.Pos2].HasFlag(ChessPiece.King))
                                        {
                                            if(Plansza[Pozycja2.Pos1, Pozycja2.Pos2].HasFlag(Team == 'B' ? ChessPiece.TeamC : ChessPiece.TeamB))
                                            {
                                                Plansza[Pozycja2.Pos1, Pozycja2.Pos2] = Plansza[Pozycja1.Pos1, Pozycja1.Pos2];
                                                Plansza[Pozycja1.Pos1, Pozycja1.Pos2] = ChessPiece.None;
                                            }
                                            else
                                            {
                                                throw new InvalidMoveException($"Kon {Team} probowal zbic pionka swojej druzyny", "same");
                                            }
                                        }
                                        else
                                        {
                                            throw new InvalidMoveException($"Kon {Team} probowal zbic krola", "king");
                                        }
                                    }
                                    //Normalny ruch
                                    else
                                    {
                                        Plansza[Pozycja2.Pos1, Pozycja2.Pos2] = Plansza[Pozycja1.Pos1, Pozycja1.Pos2];
                                        Plansza[Pozycja1.Pos1, Pozycja1.Pos2] = ChessPiece.None;
                                    }

                                }
                                else
                                {
                                    throw new InvalidMoveException($"Kon {Team} probowal poruszyc sie poza plansze w prawo");
                                }
                            }
                            else
                            {
                                throw new InvalidMoveException($"Kon {Team} probowal poruszyc sie bez ruszania sie w bok");
                            }
                        }
                    }
                    else if(Pozycja2.Pos1 == Pozycja1.Pos1 + 1)
                    {
                        if(Pozycja1.Pos1 + 1 < 8)
                        {
                            //W lewo
                            if(Pozycja2.Pos2 == Pozycja1.Pos2 - 2)
                            {
                                if(Pozycja2.Pos2 > -1)
                                {
                                    //Zbij
                                    if(Plansza[Pozycja2.Pos1, Pozycja2.Pos2] != ChessPiece.None)
                                    {
                                        if(!Plansza[Pozycja2.Pos1, Pozycja2.Pos2].HasFlag(ChessPiece.King))
                                        {
                                            if(Plansza[Pozycja2.Pos1, Pozycja2.Pos2].HasFlag(Team == 'B' ? ChessPiece.TeamC : ChessPiece.TeamB))
                                            {
                                                Plansza[Pozycja2.Pos1, Pozycja2.Pos2] = Plansza[Pozycja1.Pos1, Pozycja1.Pos2];
                                                Plansza[Pozycja1.Pos1, Pozycja1.Pos2] = ChessPiece.None;
                                            }
                                            else
                                            {
                                                throw new InvalidMoveException($"Kon {Team} probowal zbic pionka swojej druzyny", "same");
                                            }
                                        }
                                        else
                                        {
                                            throw new InvalidMoveException($"Kon {Team} probowal zbic krola", "king");
                                        }
                                    }
                                    //Normalny ruch
                                    else
                                    {
                                        Plansza[Pozycja2.Pos1, Pozycja2.Pos2] = Plansza[Pozycja1.Pos1, Pozycja1.Pos2];
                                        Plansza[Pozycja1.Pos1, Pozycja1.Pos2] = ChessPiece.None;
                                    }
                                }
                                else
                                {
                                    throw new InvalidMoveException($"Kon {Team} probowal poruszyc sie poza plansze w lewo");
                                }
                            }
                            //W prawo
                            else if(Pozycja2.Pos2 == Pozycja1.Pos2 + 2)
                            {
                                if(Pozycja2.Pos2 < 8)
                                {
                                    //Zbij
                                    if(Plansza[Pozycja2.Pos1, Pozycja2.Pos2] != ChessPiece.None)
                                    {
                                        if(!Plansza[Pozycja2.Pos1, Pozycja2.Pos2].HasFlag(ChessPiece.King))
                                        {
                                            if(Plansza[Pozycja2.Pos1, Pozycja2.Pos2].HasFlag(Team == 'B' ? ChessPiece.TeamC : ChessPiece.TeamB))
                                            {
                                                Plansza[Pozycja2.Pos1, Pozycja2.Pos2] = Plansza[Pozycja1.Pos1, Pozycja1.Pos2];
                                                Plansza[Pozycja1.Pos1, Pozycja1.Pos2] = ChessPiece.None;
                                            }
                                            else
                                            {
                                                throw new InvalidMoveException($"Kon {Team} probowal zbic pionka swojej druzyny", "same");
                                            }
                                        }
                                        else
                                        {
                                            throw new InvalidMoveException($"Kon {Team} probowal zbic krola", "king");
                                        }
                                    }
                                    //Normalny ruch
                                    else
                                    {
                                        Plansza[Pozycja2.Pos1, Pozycja2.Pos2] = Plansza[Pozycja1.Pos1, Pozycja1.Pos2];
                                        Plansza[Pozycja1.Pos1, Pozycja1.Pos2] = ChessPiece.None;
                                    }
                                }
                                else
                                {
                                    throw new InvalidMoveException($"Kon {Team} probowal poruszyc sie poza plansze w prawo");
                                }
                            }
                            else
                            {
                                throw new InvalidMoveException($"Kon {Team} probowal poruszyc sie bez ruszania sie w bok");
                            }
                        }
                    }
                    else if(Pozycja2.Pos1 == Pozycja1.Pos1 - 1)
                    {
                        if(Pozycja1.Pos1 - 1 > -1)
                        {
                            //W lewo
                            if(Pozycja2.Pos2 == Pozycja1.Pos2 - 2)
                            {
                                if(Pozycja2.Pos2 > -1)
                                {
                                    //Zbij
                                    if(Plansza[Pozycja2.Pos1, Pozycja2.Pos2] != ChessPiece.None)
                                    {
                                        if(!Plansza[Pozycja2.Pos1, Pozycja2.Pos2].HasFlag(ChessPiece.King))
                                        {
                                            if(Plansza[Pozycja2.Pos1, Pozycja2.Pos2].HasFlag(Team == 'B' ? ChessPiece.TeamC : ChessPiece.TeamB))
                                            {
                                                Plansza[Pozycja2.Pos1, Pozycja2.Pos2] = Plansza[Pozycja1.Pos1, Pozycja1.Pos2];
                                                Plansza[Pozycja1.Pos1, Pozycja1.Pos2] = ChessPiece.None;
                                            }
                                            else
                                            {
                                                throw new InvalidMoveException($"Kon {Team} probowal zbic pionka swojej druzyny", "same");
                                            }
                                        }
                                        else
                                        {
                                            throw new InvalidMoveException($"Kon {Team} probowal zbic krola", "king");
                                        }
                                    }
                                    //Normalny ruch
                                    else
                                    {
                                        Plansza[Pozycja2.Pos1, Pozycja2.Pos2] = Plansza[Pozycja1.Pos1, Pozycja1.Pos2];
                                        Plansza[Pozycja1.Pos1, Pozycja1.Pos2] = ChessPiece.None;
                                    }
                                }
                                else
                                {
                                    throw new InvalidMoveException($"Kon {Team} probowal poruszyc sie poza plansze w lewo");
                                }
                            }
                            //W prawo
                            else if(Pozycja2.Pos2 == Pozycja1.Pos2 + 2)
                            {
                                if(Pozycja2.Pos2 < 8)
                                {
                                    //Zbij
                                    if(Plansza[Pozycja2.Pos1, Pozycja2.Pos2] != ChessPiece.None)
                                    {
                                        if(!Plansza[Pozycja2.Pos1, Pozycja2.Pos2].HasFlag(ChessPiece.King))
                                        {
                                            if(Plansza[Pozycja2.Pos1, Pozycja2.Pos2].HasFlag(Team == 'B' ? ChessPiece.TeamC : ChessPiece.TeamB))
                                            {
                                                Plansza[Pozycja2.Pos1, Pozycja2.Pos2] = Plansza[Pozycja1.Pos1, Pozycja1.Pos2];
                                                Plansza[Pozycja1.Pos1, Pozycja1.Pos2] = ChessPiece.None;
                                            }
                                            else
                                            {
                                                throw new InvalidMoveException($"Kon {Team} probowal zbic pionka swojej druzyny", "same");
                                            }
                                        }
                                        else
                                        {
                                            throw new InvalidMoveException($"Kon {Team} probowal zbic krola", "king");
                                        }
                                    }
                                    //Normalny ruch
                                    else
                                    {
                                        Plansza[Pozycja2.Pos1, Pozycja2.Pos2] = Plansza[Pozycja1.Pos1, Pozycja1.Pos2];
                                        Plansza[Pozycja1.Pos1, Pozycja1.Pos2] = ChessPiece.None;
                                    }
                                }
                                else
                                {
                                    throw new InvalidMoveException($"Kon {Team} probowal poruszyc sie poza plansze w lewo");
                                }
                            }
                            else
                            {
                                throw new InvalidMoveException($"Kon {Team} probowal poruszyc sie bez ruszania sie w bok");
                            }
                        }
                    }
                    else if(Pozycja2.Pos1 == Pozycja1.Pos1 - 2)
                    {
                        if(Pozycja1.Pos1 - 2 > -1)
                        {
                            //W lewo
                            if(Pozycja2.Pos2 == Pozycja1.Pos2 - 1)
                            {
                                if(Pozycja2.Pos2 > -1)
                                {
                                    //Zbij
                                    if(Plansza[Pozycja2.Pos1, Pozycja2.Pos2] != ChessPiece.None)
                                    {
                                        if(!Plansza[Pozycja2.Pos1, Pozycja2.Pos2].HasFlag(ChessPiece.King))
                                        {
                                            if(Plansza[Pozycja2.Pos1, Pozycja2.Pos2].HasFlag(Team == 'B' ? ChessPiece.TeamC : ChessPiece.TeamB))
                                            {
                                                Plansza[Pozycja2.Pos1, Pozycja2.Pos2] = Plansza[Pozycja1.Pos1, Pozycja1.Pos2];
                                                Plansza[Pozycja1.Pos1, Pozycja1.Pos2] = ChessPiece.None;
                                            }
                                            else
                                            {
                                                throw new InvalidMoveException($"Kon {Team} probowal zbic pionka swojej druzyny", "same");
                                            }
                                        }
                                        else
                                        {
                                            throw new InvalidMoveException($"Kon {Team} probowal zbic krola", "king");
                                        }
                                    }
                                    //Normalny ruch
                                    else
                                    {
                                        Plansza[Pozycja2.Pos1, Pozycja2.Pos2] = Plansza[Pozycja1.Pos1, Pozycja1.Pos2];
                                        Plansza[Pozycja1.Pos1, Pozycja1.Pos2] = ChessPiece.None;
                                    }
                                }
                                else
                                {
                                    throw new InvalidMoveException($"Kon {Team} probowal poruszyc sie poza plansze w lewo");
                                }
                            }
                            //W prawo
                            else if(Pozycja2.Pos2 == Pozycja1.Pos2 + 1)
                            {
                                if(Pozycja2.Pos2 < 8)
                                {
                                    //Zbij
                                    if(Plansza[Pozycja2.Pos1, Pozycja2.Pos2] != ChessPiece.None)
                                    {
                                        if(!Plansza[Pozycja2.Pos1, Pozycja2.Pos2].HasFlag(ChessPiece.King))
                                        {
                                            if(Plansza[Pozycja2.Pos1, Pozycja2.Pos2].HasFlag(Team == 'B' ? ChessPiece.TeamC : ChessPiece.TeamB))
                                            {
                                                Plansza[Pozycja2.Pos1, Pozycja2.Pos2] = Plansza[Pozycja1.Pos1, Pozycja1.Pos2];
                                                Plansza[Pozycja1.Pos1, Pozycja1.Pos2] = ChessPiece.None;
                                            }
                                            else
                                            {
                                                throw new InvalidMoveException($"Kon {Team} probowal zbic pionka swojej druzyny", "same");
                                            }
                                        }
                                        else
                                        {
                                            throw new InvalidMoveException($"Kon {Team} probowal zbic krola", "king");
                                        }
                                    }
                                    //Normalny ruch
                                    else
                                    {
                                        Plansza[Pozycja2.Pos1, Pozycja2.Pos2] = Plansza[Pozycja1.Pos1, Pozycja1.Pos2];
                                        Plansza[Pozycja1.Pos1, Pozycja1.Pos2] = ChessPiece.None;
                                    }
                                }
                                else
                                {
                                    throw new InvalidMoveException($"Kon {Team} probowal poruszyc sie poza plansze w lewo");
                                }
                            }
                            else
                            {
                                throw new InvalidMoveException($"Kon {Team} probowal poruszyc sie bez ruszania sie w bok");
                            }
                        }
                    }
                    else
                    {
                        throw new InvalidMoveException($"Kon {Team} nie wykonal zadnego poprawnego ruchu");
                    }
                }
                else
                {
                    throw new InvalidMoveException($"Ruch konia {Team} nie spelnial zadnego warunku ruchu");
                }
            }
            else
            {
                throw new InvalidMoveException($"Ruch konia {Team} nie spelnial zadnego warunku odleglosci od konca co jest niemozliwe");
            }
        }
        public static void RuchGońcem(Pozycja Pozycja1, Pozycja Pozycja2, Char Team)
        {

        }
        public static void RuchWieżą(Pozycja Pozycja1, Pozycja Pozycja2, Char Team)
        {
			//
			// TODO: ADD CHECKS FOR IF < 8/IF > -1 HERE!!!
			//
            // Sprawdza czy ruch jest w poprawnym kierunku
            // Warunki:
            // 1 - W górę
            // 2 - W lewo
            // 3 - W prawo
            // 4 - W dół
            if((Pozycja2.Pos1 < Pozycja1.Pos1 && Pozycja2.Pos2 == Pozycja1.Pos2)||(Pozycja2.Pos1 == Pozycja1.Pos1 && Pozycja2.Pos2 < Pozycja1.Pos2)||(Pozycja2.Pos1 == Pozycja1.Pos1 && Pozycja2.Pos2 > Pozycja1.Pos2)||(Pozycja2.Pos1 > Pozycja1.Pos1 && Pozycja2.Pos2 == Pozycja1.Pos2))
            {
                //1 - W górę
                if(Pozycja2.Pos1 < Pozycja1.Pos1 && Pozycja2.Pos2 == Pozycja1.Pos2)
                {
					int i = Pozycja1.Pos1;
                    // Sprawdza czy po drodze nie ma żadnego pionka
					while(i > Pozycja2.Pos1)
					{
						if(Plansza[i, Pozycja1.Pos2] == ChessPiece.None)
						{
                            i--;
                            continue;
                        }
						else
						{
                            throw new InvalidMoveException($"Wieza {Team} probowala poruszyc sie przez innego pionka");
						}
					}
                    // To wykonuje się tylko jeśli po drodze nie ma pionka
                    if(Plansza[Pozycja2.Pos1, Pozycja2.Pos2] != ChessPiece.None)
                    {
                        if(Plansza[Pozycja2.Pos1, Pozycja2.Pos2].HasFlag(Team == 'B' ? ChessPiece.TeamB : ChessPiece.TeamC))
                        {
                            throw new InvalidMoveException($"Wieza {Team} sprobowala zbic pionka swojej druzyny", "same");
                        }
                    }
                    Plansza[Pozycja2.Pos1, Pozycja2.Pos2] = Plansza[Pozycja1.Pos1, Pozycja1.Pos2];
                    Plansza[Pozycja1.Pos1, Pozycja1.Pos2] = ChessPiece.None;
                }
                //2 - W lewo
                else if(Pozycja2.Pos1 == Pozycja1.Pos1 && Pozycja2.Pos2 < Pozycja1.Pos2)
                {
					
                }
                //3 - W prawo
                else if(Pozycja2.Pos1 == Pozycja1.Pos1 && Pozycja2.Pos2 > Pozycja1.Pos2)
                {
					
                }
                //4 - W dół
                if(Pozycja2.Pos1 > Pozycja1.Pos1 && Pozycja2.Pos2 == Pozycja1.Pos2)
                {
					
                }
            }
			else
			{
				throw new InvalidMoveException("Ruch wiezy nie spelnial zadnego warunku");
			}
        }
        /*public static void RuchKrólową(Pozycja Pozycja1, Pozycja Pozycja2, Char Team)
        {

        }*/
        public static void RuchKrólem(Pozycja Pozycja1, Pozycja Pozycja2, Char Team)
        {
            switch(Team)
            {

            }
        }
        public static void ClearEnPassants(Char Team)
        {
            var Flag = (Team == 'B' ? ChessPiece.TeamB : ChessPiece.TeamC);
            for(int i = 0; i < 8; i++)
            {
                for(int i2 = 0; i < 8; i2++)
                {
                    if(PossibleEnPassants[i, i2].HasFlag(Flag))
                    {
                        PossibleEnPassants[i, i2] = ChessPiece.None;
                    }
                }
            }
        }
        public static void Rozgrywka()
        {
            Szachy.PostawPionki();
            bool koniecrozgrywki = false;
            switch(Program.playerTeam)
            {
                case 'B':
                    //napisz w czacie $"Gracz {Nick} gra białymi"
                    Program.AppendText($"EVENT Gracz {Program.Nick} gra białymi", Program.currentfolder, Program.ID);
                    Program.receivedMessages.Add($"Gracz {Program.Nick} gra białymi");
                    Program.odebranoWiad = true;
                    break;
                case 'C':
                    break;
            }
            for(int tura = 1; !koniecrozgrywki; tura++)
            {
                if(tura.ToString().Length == 1)
                {
                    Console.WriteLine(new String('-', 36) + $" Tura {tura} " + new String('-', 36));
                }
                else if(tura.ToString().Length == 2)
                {
                    Console.WriteLine(new String('-', 35) + $" Tura {tura} " + new String('-', 36));
                }
                else if(tura.ToString().Length == 3)
                {
                    Console.WriteLine(new String('-', 35) + $" Tura {tura} " + new String('-', 35));
                }
                else
                {
                    //może jakiś algorytm na to here
                    //if costam % 2 == 0 musi być, kiedyś to napiszę
                }
                switch(Program.playerTeam)
                {
                    case 'B':
                        ClearEnPassants('B');
                        NarysujPlansze();
                        OznaczSzachy();
                        Console.Write("Podaj akcje do wykonania: \tRuch\tRoszada\n>");
                        string akcja = Console.ReadLine();
                        if(akcja.ToLower().StartsWith("ruch"))
                        {
                            Console.Write("Podaj pozycje pionka, który ma sie poruszyc (np. A1)\n>");
                            Regex posregex = new Regex("^[A-H][1-8]$");
                            podajpozycje:
                            string pionekDoRuszenia = Console.ReadLine().ToUpper();
                            if(posregex.IsMatch(pionekDoRuszenia))
                            {
                                Pozycja Pozycja1 = new Pozycja(pionekDoRuszenia.First(), pionekDoRuszenia.Last());
                                if(Plansza[Pozycja1.Pos1, Pozycja1.Pos2] == ChessPiece.None)
                                {
                                    Console.Write("Niepoprawna pozycja\n>");
                                    goto podajpozycje;
                                }
                                Console.Write("Podaj pozycje na ktora pionek ma sie poruszyc\n>");
                                podajpozycjedocelowa:
                                string pozycjaDocelowa = Console.ReadLine().ToUpper();
                                if(posregex.IsMatch(pozycjaDocelowa))
                                {
                                    Pozycja Pozycja2 = new Pozycja(pozycjaDocelowa.First(), pozycjaDocelowa.Last());
                                    try
                                    {
                                        WykonajRuch(Pozycja1, Pozycja2, Program.Nick, Program.playerTeam);
                                    }
                                    catch
                                    {
                                        //Invalid move handler here
                                    }
                                }
                                else
                                {
                                    Console.Write("Niepoprawna pozycja\n>");
                                    goto podajpozycjedocelowa;
                                }
                            }
                            else
                            {
                                Console.Write("Niepoprawna pozycja\n>");
                                goto podajpozycje;
                            } 
                        }
                        else if(akcja.ToLower().StartsWith("roszada"))
                        {
                            //TODO: Wykonaj roszadę
                        }
                        //else if(){ }
                        else
                        {
                            //TODO: Cofnij do wyboru
                        }
                        break;
                    case 'C':
                        Szachy.NarysujPlansze();
                        while(!Program.hasEnemyMoved)
                        {
                            Thread.Sleep(1000);
                        }
                        Program.hasEnemyMoved = false;
                        //Wykonaj turę przeciwnika
                        
                        //wykonaj swoją turę
                        //przeciwnik wykonuje ture
                        break;
                        //ZANIM NAPISZESZ RESZTĘ DOKOŃCZ OZNACZSZACHY()
                }
            }
        }
    }
    public class Program
    {
        public static Random rand = new Random();
        public static volatile bool hasOtherPlayerJoined = false;
        public static volatile bool isApppending = false;
        public static volatile bool wasContentReceived = false;
        public static volatile bool isContentBeingReceived = false;
        public static int lastCountChat = 0;
        public static List<string> receivedMessages = new List<string>();
        public static Process chat = new Process();
        public static List<string> lines = new List<string>();
        public static bool odebranoWiad = false;
        public static List<string> chatLog = new List<string>(1);
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
        public static bool wykonajconnect = true;
        public static string nazwasesji;
        public static bool hasEnemyMoved;
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
            if(IsPublic) txt += "public\n";
            if(!IsPublic) txt += "private\n";
            txt += $"{name}\n";
            txt += $"{nick}\n";
            int playerTeamInt = rand.Next(1, 3);
            if(playerTeamInt == 2)
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

        public static FtpClient client = new FtpClient();
        public static bool czy_odbierac = false;
        public static int SkonwertujLitere(char litera)
        {
            switch(litera)
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
        public static char SkonwertujCyfre(int cyfra)
        {
            switch(cyfra)
            {
                case 0:
                    return 'A';
                case 1:
                    return 'B';
                case 2:
                    return 'C';
                case 3:
                    return 'D';
                case 4:
                    return 'E';
                case 5:
                    return 'F';
                case 6:
                    return 'G';
                case 7:
                    return 'H';
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
            if(lastCount < lines.Count() || isApppending == false)
            {
                //"Nick: [wydarzenie]"
                wasContentReceived = true;
                int i = lastCount + 1;
                while(i <= lines.Count)
                {
                    if(!hasOtherPlayerJoined)
                    {
                        if(lines[i].StartsWith("JOIN"))
                        {
                            lines[i] = lines[i].Substring(enemyNick.Length + 2);
                            enemyNick = lines[i].Remove(0, 5).Trim();
                            hasOtherPlayerJoined = true;
                        }
                    }
                    if(!lines[i].StartsWith(Nick))
                    {
                        lines[i] = lines[i].Substring(enemyNick.Length + 2);
                        if(lines[i].StartsWith("CZAT"))
                        {
                            lines[i] = lines[i].Substring(enemyNick.Length + 2);
                            odebranoWiad = true;
                            //"CZAT {wiad}"
                            lines[i] = lines[i].Remove(0, 5);
                            lines[i] = lines[i].Trim();
                            lines[i] = lines[i].Insert(0, $"{enemyNick}: ");
                            receivedMessages.Add(lines[i]);
                        }
                        else if(lines[i].Contains(">>"))
                        {
                            lines[i] = lines[i].Substring(enemyNick.Length + 2);
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
                            Szachy.WykonajISprawdzRuchPrzeciwnika(Pozycja1, Pozycja2);
                        }
                        else if(lines[i].StartsWith("CLOSING"))
                        {
                            lines[i] = lines[i].Substring(enemyNick.Length + 2);
                            AppendText("OK", "StartingSessions", ID);
                            currentfolder = "ActiveSessions";
                        }
                        else if(lines[i].StartsWith("OK"))
                        {
                            lines[i] = lines[i].Substring(enemyNick.Length + 2);
                            //"OK"
                            client.MoveFile($"/SzachySerwer/StartingSessions/{ID}.txt", $"/SzachySerwer/ActiveSessions/{ID}.txt");
                            currentfolder = "ActiveSessions";
                        }
                        else if(lines[i].StartsWith("EVENT"))
                        {
                            receivedMessages.Add(lines[i].Substring(0, 6));
                        }
                    }
                    i++;
                }
                lastCount = lines.Count;
            }
            if(!isSending)
            {
                client.Disconnect();
            }
            lastCount = lines.Count;
            isOdbierz = false;
        }
        public static void OtwórzCzat()
        {
            void chat_Exited(object sender, EventArgs e)
            {
                AppendText($"Gracz {Nick} opuscil sesje", currentfolder, ID);
                czy_odbierac = false;

            }
            if(!Directory.Exists(@".\Chat\Logs"))
            {
                Directory.CreateDirectory(@".\Chat\Logs");
            }
            StreamWriter sw = File.CreateText($@".\Chat\Logs\Log{ID}.txt");
            sw.Dispose();
            ProcessStartInfo psi = new ProcessStartInfo()
            {
                CreateNoWindow = false,
                UseShellExecute = true,
                FileName = $@"{Directory.GetCurrentDirectory()}\Chat\SzachyChat.exe",
                WindowStyle = ProcessWindowStyle.Normal,
                Arguments = $"{ID} {Nick}"
            };
            chat.StartInfo = psi;
            chat.Start();
            chat.EnableRaisingEvents = true;
            chat.Exited += new EventHandler(chat_Exited);
            isChatOpen = true;
        }
        public static void WyślijWiadomość(string currentfolder, string wiadomosc, int ID)
        {
            Program.isSending = true;
            if(!Program.client.IsConnected)
            {
                Program.client.Connect();
            }
            Program.AppendText($"{Program.Nick}: {wiadomosc}", currentfolder, Convert.ToString(ID));
            Program.isSending = false;
        }
        static void Chat()
        {
            StreamReader chatReader = new StreamReader($@".\Chat\Logs\Log{ID}.txt");
            chatLog.Clear();
            chatLog.AddRange(chatReader.ReadToEnd().Split('\n'));
            int countNow = chatLog.Count();
            if(countNow != 0)
            {
                if(countNow > lastCountChat)
                {
                    isSending = true;
                    List<string> sentMessages = new List<string>();
                    for(int i = lastCountChat; i < countNow; i++)
                    {
                        if(!chatLog[i].StartsWith(Nick))
                        {
                            sentMessages.Add($"{Nick}: {chatLog[i]}");
                        }
                    }
                    if(client.IsConnected == false)
                    {
                        client.Connect();
                    }
                    foreach(string msg in sentMessages)
                    {
                        AppendText(msg, currentfolder, Convert.ToString(ID));
                    }
                    lastCountChat = chatLog.Count();
                    if(isOdbierz == false)
                    {
                        client.Disconnect();
                    }
                    isSending = false;
                    chatReader.Close();
                }
            }
            if(odebranoWiad)
            {
                StreamWriter chatWriter = new StreamWriter($@".\Chat\Logs\Log{ID}.txt");
                //sw = chat.StandardInput;
                foreach(string receivedMessage in receivedMessages)
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
            int repeati = 0;
            while(czy_odbierac == true)
            {
                try
                {
                    Odbierz();
                    repeati = 0;
                    Thread.Sleep(7000);
                }
                catch
                {
                    if(repeati >= 4)
                    {
                        Console.Write("Nie udalo sie polaczyc z serwerem, sprobowac ponownie? t/n\n(Odpowiedz \"n\" przerywa sesje, nie jest to wskazane.)\n>");
                        connrepeatgoto:
                        string connrepeat = Console.ReadLine().ToLower();
                        if(connrepeat.Contains('t'))
                        {
                            repeati++;
                        }
                        else if(connrepeat.Contains('n'))
                        {
                            Console.WriteLine("Konczenie sesji... cofanie do menu glownego");
                            chat.Dispose();
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
        static void Main(string[] args)
        {
            void main_Exited(object sender, EventArgs e)
            {
                if(isChatOpen)
                {
                    chat.Dispose();
                }
            }
            Process.GetCurrentProcess().EnableRaisingEvents = true;
            Process.GetCurrentProcess().Exited += main_Exited;
            Szachy.PostawPionki();
            playerTeam = 'C';
            Szachy.OznaczSzachy();
            Szachy.NarysujPlansze();
            Console.WriteLine("----");
            Pozycja raz = new Pozycja(Console.ReadKey(false).KeyChar, Console.ReadKey(false).KeyChar);
            Console.WriteLine("\n--->");
            Pozycja dwa = new Pozycja(Console.ReadKey(false).KeyChar, Console.ReadKey(false).KeyChar);
            Console.WriteLine("\n----\n");
            Szachy.WykonajRuch(raz, dwa, "kyaaan", Szachy.Plansza[raz.Pos1, raz.Pos2].HasFlag(Szachy.ChessPiece.TeamB) ? 'B' : 'C');
            Console.WriteLine("----");
            Pozycja trzy = new Pozycja(Console.ReadKey(false).KeyChar, Console.ReadKey(false).KeyChar);
            Console.WriteLine("\n--->");
            Pozycja cztery = new Pozycja(Console.ReadKey(false).KeyChar, Console.ReadKey(false).KeyChar);
            Console.WriteLine("\n----\n");
            Szachy.WykonajRuch(trzy, cztery, "kyaaan", Szachy.Plansza[trzy.Pos1, trzy.Pos2].HasFlag(Szachy.ChessPiece.TeamB) ? 'B' : 'C');
            Console.WriteLine("----");
            Pozycja piec = new Pozycja(Console.ReadKey(false).KeyChar, Console.ReadKey(false).KeyChar);
            Console.WriteLine("\n--->");
            Pozycja szesc = new Pozycja(Console.ReadKey(false).KeyChar, Console.ReadKey(false).KeyChar);
            Console.WriteLine("\n----\n");
            Szachy.WykonajRuch(piec, szesc, "kyaaan", Szachy.Plansza[piec.Pos1, piec.Pos2].HasFlag(Szachy.ChessPiece.TeamB) ? 'B' : 'C');
            Console.WriteLine("----");
            Pozycja siedem = new Pozycja(Console.ReadKey(false).KeyChar, Console.ReadKey(false).KeyChar);
            Console.WriteLine("\n--->");
            Pozycja osiem = new Pozycja(Console.ReadKey(false).KeyChar, Console.ReadKey(false).KeyChar);
            Console.WriteLine("\n----");
            Szachy.WykonajRuch(siedem, osiem, "kyaaan", Szachy.Plansza[siedem.Pos1, siedem.Pos2].HasFlag(Szachy.ChessPiece.TeamB) ? 'B' : 'C');
            Szachy.NarysujPlansze();
            playerTeam = 'C';
            Szachy.NarysujPlansze();
            if(args.Length > 0)
            {
                if(args[0] == "true")
                {
                    BasicIO = true;
                }
            }
            menuglowne:
            Console.WriteLine($"Obecny czas: {DateTime.Now}/{(Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds}");
            Console.Write("Polaczyc sie z serwerem? t/n\n>");
            string czy_polaczyc = Console.ReadLine().ToLower();
            if(czy_polaczyc.Contains('t'))
            {
                Console.Write("Podaj swoj nick:\n>");
                Nick = Console.ReadLine();
                client.Host = credentials.host;
                client.Port = credentials.port;
                client.Credentials = new NetworkCredential(credentials.username, credentials.password);
                polacz:
                try
                {
                    Console.WriteLine("Laczenie...");
                    client.Connect();
                }
                catch
                {
                    Console.Write("Nie udalo sie polaczyc z serwerem, sprobowac ponownie? t/n\n>");
                    string connrepeat = Console.ReadLine().ToLower();
                    if(connrepeat.Contains('t'))
                    {
                        goto polacz;
                    }
                    else if(connrepeat.Contains('n'))
                    {
                        Console.WriteLine("Cofanie do menu glownego");
                        goto menuglowne;
                    }
                }
            }
            else if(czy_polaczyc.Contains('n'))
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
                if(client.GetWorkingDirectory() != "/SzachySerwer/StartingSessions/")
                {
                    client.SetWorkingDirectory(@"/SzachySerwer/StartingSessions/");
                }
                wykonajconnect = false;
                Console.WriteLine("---Lobby------------------------------------------------------------------------");
                Console.WriteLine("Lp. Nazwa pokoju               - ID");
                int i = 1;
                foreach(var item in client.GetListing(client.GetWorkingDirectory(), FtpListOption.Recursive))
                {
                    donotincrement = false;
                    var isFile = client.FileExists(item.FullName);
                    if(isFile == true)
                    {
                        var tmp = client.OpenRead($"/SzachySerwer/StartingSessions/{item.Name}");
                        byte[] buffer = new byte[tmp.Length];
                        tmp.Read(buffer, 0, (Int32)tmp.Length);
                        string str = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                        List<string> lines = str.Split('\n').ToList();
                        tmp.Close();
                        if(lines[0] == "public")
                        {
                            if(lines[1].Length > 25)
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
                    if(donotincrement == false)
                    {
                        i++;
                    }
                }
                client.SetWorkingDirectory(@"/SzachySerwer/");
                client.Disconnect();
            }
            catch
            {
                client.Disconnect();
                Console.Write("Blad odbierania lobby. Sprobowac ponownie? t/n\n>");
                string tryagainlobby = Console.ReadLine();
                if(tryagainlobby.ToLower().Contains('t'))
                {
                    goto lobby;
                }
            }
            long cyferkiID = new long();
            wykonajconnect = false;
            bool isCancel = false;
            Console.Write("\nPodaj ID sesji publicznej (z lobby) lub prywatnej (otrzymanej od znajomego) aby dolaczyc do niej. Utworz nowa sesje poleceniem \"create\". Odswiez lobby poleceniem \"refresh\"\n>");
            string connect_to_id = Console.ReadLine().Trim();
            if(connect_to_id.ToLower().Contains("create"))
            {
                bool isPublic;
                podajnazwesesji:
                Console.Write("\nPodaj nazwe sesji, ktora chcesz stworzyc lub napisz \"cancel\" aby wrocic do lobby\n>");
                nazwasesji = Console.ReadLine();
                if(nazwasesji.ToLower() == "cancel")
                {
                    goto lobby;
                }
                if(nazwasesji.Length > 25)
                {
                    Console.WriteLine("Nazwa sesji jest zbyt dluga. Wpisz krotsza nazwe (max 25 znakow)");
                    goto podajnazwesesji;
                }
                if(isCancel == false)
                {
                    ispublicgoto:
                    Console.Write("Czy sesja ma byc prywatna? t/n\n>");
                    string ispublicstr = Console.ReadLine().ToLower();
                    if(ispublicstr.Contains('t'))
                    {
                        isPublic = false;
                    }
                    else if(ispublicstr.Contains('n'))
                    {
                        isPublic = true;
                    }
                    else
                    {
                        Console.WriteLine("Niepoprawna odpowiedz:" + ispublicstr);
                        goto ispublicgoto;
                    }
                    Console.Write($"Tworzenie sesji o nazwie \"{nazwasesji}\", kontynuowac? t/n\n>");
                    string napewno = Console.ReadLine().ToLower();
                    if(napewno.Contains('t'))
                    {
                        ID = Convert.ToString((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
                        client.Connect();
                        CreateSessionFile(ID, isPublic, nazwasesji, Nick);
                        client.Disconnect();
                        InitKlient();
                        Console.WriteLine("Czekanie na drugiego gracza...");
                        while(!hasOtherPlayerJoined)
                        {
                            Thread.Sleep(TimeSpan.FromSeconds(5));
                        }
                        AppendText("CLOSING", currentfolder, ID);
                        while(currentfolder != "ActiveSessions")
                        {
                            Thread.Sleep(TimeSpan.FromSeconds(3));
                        }
                        Szachy.Rozgrywka();
                        //(Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds
                    }
                    else if(napewno.ToLower().Contains('n'))
                    {
                        Console.WriteLine("\nWracanie do lobby...\n");
                        wykonajconnect = false;
                        goto lobby;
                    }
                }
            }
            else if(connect_to_id.ToLower().Contains("refresh"))
            {
                Console.WriteLine("\nOdswiezanie lobby...\n");
                wykonajconnect = false;
                client.Connect();
                goto lobby;
            }
            else if(Int64.TryParse(connect_to_id, out cyferkiID))
            {
                ID = connect_to_id;
                wykonajconnect = true;
            }
            else
            {
                Console.WriteLine("Niepoprawna odpowiedz: " + connect_to_id);
                Console.WriteLine("Cofanie do lobby...\n");
                goto lobby;
            }
            if(wykonajconnect == true)
            {
                foreach(var item in client.GetNameListing())
                {
                    //connect, setdirectory
                    var isFile = client.FileExists(item);
                    if(isFile == true)
                    {
                        if(item.GetFtpFileName().TrimEnd('.', 't', 'x') == connect_to_id)
                        {
                            Console.Clear();
                            Console.SetWindowSize(80, 50);
                            ID = connect_to_id;
                            joingoto:
                            try { AppendText($"JOIN: {Nick}", currentfolder, connect_to_id); } catch { Thread.Sleep(rand.Next(150, 301)); goto joingoto; }
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
                            Szachy.Rozgrywka();
                        }
                    }
                }
            }
        }
        static void SizeCheckThread()
        {
            if(Console.WindowWidth != 80)
            {
                Console.SetWindowSize(80, Console.WindowHeight);
            }
        }
        static void SizeCheck()
        {
            while(true)
            {
                SizeCheckThread();
                Thread.Sleep(1500);
            }
        }

        static void InitKlient()
        {
            Thread Size = new Thread(new ThreadStart(SizeCheck));
            Size.Start();
            if(BasicIO == false)
            {
                OtwórzCzat();
                Thread chat = new Thread(new ThreadStart(ChatThread));
                chat.Start();
            }
            czy_odbierac = true;
            Thread t = new Thread(new ThreadStart(KlientOdbierajacy));
            t.Start();
        }
        static void ChatThread()
        {
            while(isChatOpen)
            {
                Chat();
                Thread.Sleep(500);
            }
        }
    }
} //As of 14:07 2020-05-08 this is line 2014. I love this moment and want this memory to exist.
//As of 14:48 2020-05-08 this is now line 2020!