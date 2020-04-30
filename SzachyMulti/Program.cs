﻿using FluentFTP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Credentials;

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
    public static class Szachy
    {
        [Flags]
        public enum ChessPiece
        {
            None = 0b_0000_0000,
            TeamB = 0b_0100_0000,
            TeamC = 0b_1000_0000,
            King = 0b_0010_0000,
            Queen = 0b_0001_0000,
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
        public static volatile ChessPiece[,] BackupSzachyBC = new ChessPiece[8, 8];
        public static volatile ChessPiece[,] BackupHiddenSzachyBC = new ChessPiece[8, 8];
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
            Plansza[4, 2] = ChessPiece.Pawn | ChessPiece.TeamC; //"pionek1C" :heart:
            Plansza[3, 3] = ChessPiece.King | ChessPiece.TeamB; //"krol2B" :heart:
            Plansza[2, 0] = ChessPiece.King | ChessPiece.TeamC; //"krolC" :heart:
            Plansza[2, 2] = ChessPiece.King | ChessPiece.TeamC; //"krolC" :heart:
            Plansza[4, 7] = ChessPiece.Knight | ChessPiece.TeamC; //"kon123C" :heart:
            Plansza[3, 5] = ChessPiece.King | ChessPiece.TeamB; //"krolB" :heart:
            Plansza[3, 7] = ChessPiece.King | ChessPiece.TeamC; //"krolC" :heart:
            Plansza[2, 5] = ChessPiece.Knight | ChessPiece.TeamB; //"kon123B" :heart:
            Plansza[3, 0] = ChessPiece.King | ChessPiece.TeamC; //"krolC" :heart:
            Plansza[5,2] = ChessPiece.Bishop | ChessPiece.TeamB;
            //Plansza[5, 2] = "goniecB"; //B :heart:
            // ❤︎
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
        public static void OznaczSzachy(/*TODO: REPLACE THIS SINCE IT'S NOT NEEDED, WE HAVE BACKUPS*/bool isTestingAfterMove)
        {
            //TODO: MAKE BACKUPS BEFORE REDEFINING AND CHANGE BOARDS TO THE MERGED ONES
            SzachyB = new bool[8, 8];
            SzachyC = new bool[8, 8];
            HiddenSzachyB = new bool[8, 8];
            HiddenSzachyC = new bool[8, 8];
            for (int i = 0, i2 = 0; i < 8; i2++)
            {
                if (i2 == 8)
                {
                    i2 = 0;
                    i++;
                    if (i >= 8)
                    {
                        continue;
                    }
                }
                if (!(Plansza[i, i2] == null))
                {
                    switch (Plansza[i, i2].TrimEnd('1', '2', '3', '4', '5', '6', '7', '8', 'B', 'C'))
                    {
                        case "pionek":
                            switch (Plansza[i, i2].Last())
                            {
                                case 'B':
                                    if (!(i+1 >= 8))
                                    {
                                        if (!(i2-1 <= -1))
                                        {
                                            if (Plansza[i+1,i2-1] != null)
                                            {
                                                if (Plansza[i+1, i2-1] == "krolC")
                                                {
                                                    SzachyB[i+1, i2-1] = true;
                                                }
                                            }
                                            else
                                            {
                                                HiddenSzachyB[i+1,i2-1] = true;
                                            }
                                        }
                                        if (!(i2+1 >= 8))
                                        {
                                            if (Plansza[i+1,i2+1] != null)
                                            {
                                                if (Plansza[i+1, i2+1] == "krolC")
                                                {
                                                    SzachyB[i+1, i2+1] = true;
                                                }
                                            }
                                            else 
                                            {
                                                HiddenSzachyB[i+1,i2+1] = true;
                                            }
                                        }
                                    }
                                    break;
                                case 'C':
                                    if (!(i-1 <= -1))
                                    {
                                        if (!(i2+1 >= 8))
                                        {
                                            if (Plansza[i-1,i2+1] != null)
                                            {
                                                if (Plansza[i-1, i2+1] == "krolB")
                                                {
                                                    SzachyC[i-1, i2+1] = true;
                                                } 
                                            }
                                            else
                                            {
                                                HiddenSzachyC[i-1,i2+1] = true;
                                            }
                                        }
                                        if (!(i2-1 <= -1))
                                        {
                                            if (Plansza[i-1,i2-1] != null)
                                            {
                                                if (Plansza[i-1, i2-1] == "krolB")
                                                {
                                                    SzachyC[i-1, i2-1] = true;
                                                } 
                                            }
                                            else
                                            {
                                                HiddenSzachyC[i-1,i2-1] = true;
                                            }
                                        }
                                    }
                                    break;
                            }
                            break;
                        case "kon":
                            switch (Plansza[i, i2].Last())
                            {
                                case 'B':
                                    if (!(i-2 <= -1))
                                    {
                                        if (!(i2-1 <= -1))
                                        {
                                            if (Plansza[i-2, i2-1] != null)
                                            {
                                                if (Plansza[i-2, i2-1] == "krolC")
                                                {
                                                    SzachyB[i-2, i2-1] = true;
                                                }
                                            }
                                            else
                                            {
                                               HiddenSzachyB[i-2,i2-1] = true;
                                            }
                                        }
                                        if (!(i2+1 >= 8))
                                        {
                                            if (Plansza[i-2, i2+1] != null)
                                            {
                                                if (Plansza[i-2, i2+1] == "krolC")
                                                {
                                                    SzachyB[i-2, i2+1] = true;
                                                }
                                            }
                                            else
                                            {
                                                HiddenSzachyB[i-2, i2+1] = true;
                                            }
                                        }
                                    }
                                    if (!(i-1 <= -1))
                                    {
                                        if (!(i2-2 <= -1))
                                        {
                                            if (Plansza[i-1,i2-2] != null)
                                            {
                                                if (Plansza[i-1, i2-2] == "krolC")
                                                {
                                                    SzachyB[i-1, i2-2] = true;
                                                } 
                                            }
                                            else
                                            {
                                                HiddenSzachyB[i-1,i2-2] = true;
                                            }
                                        }
                                        if (!(i2+2 >= 8))
                                        {
                                            if (Plansza[i-1,i2+2] != null)
                                            {
                                                if (Plansza[i-1, i2+2] == "krolC")
                                                {
                                                    SzachyB[i-1, i2+2] = true;
                                                } 
                                            }
                                            else
                                            {
                                                HiddenSzachyB[i-1,i2+2] = true;
                                            }
                                        }
                                    }
                                    if (!(i+2 >= 8))
                                    {
                                        if (!(i2-1 <= -1))
                                        {
                                            if (Plansza[i+2,i2-1] != null)
                                            {
                                                if (Plansza[i+2, i2-1] == "krolC")
                                                {
                                                    SzachyB[i+2, i2-1] = true;
                                                } 
                                            }
                                            else
                                            {
                                                HiddenSzachyB[i+2,i2-1] = true;
                                            }
                                        }
                                        if (!(i2+1 >= 8))
                                        {
                                            if (Plansza[i+2,i2+1] != null)
                                            {
                                                if (Plansza[i+2, i2+1] == "krolC")
                                                {
                                                    SzachyB[i+2, i2+1] = true;
                                                } 
                                            }
                                            else
                                            {
                                                HiddenSzachyB[i+2, i2+1] = true;
                                            }
                                        }
                                    }
                                    if (!(i+1 >= 8))
                                    {
                                        if (!(i2-2 <= -1))
                                        {
                                            if (Plansza[i+1,i2-2] != null) 
                                            {
                                                if (Plansza[i+1, i2-2] == "krolC")
                                                {
                                                    SzachyB[i+1, i2-2] = true;
                                                } 
                                            }
                                            else
                                            {
                                                HiddenSzachyB[i+1, i2-2] = true;
                                            }
                                        }
                                        if (!(i2+2 >= 8))
                                        {
                                            if (Plansza[i+1,i2+2] != null)
                                            {
                                                if (Plansza[i+1, i2+2] == "krolC")
                                                {
                                                    SzachyB[i+1, i2+2] = true;
                                                } 
                                            }
                                            else
                                            {
                                                HiddenSzachyB[i+1, i2+2] = true;
                                            }
                                        }
                                    }
                                    break;
                                case 'C':
                                    if (!(i-2 <= -1))
                                    {
                                        if (!(i2-1 <= -1))
                                        {
                                            if (Plansza[i-2,i2-1] != null)
                                            {
                                                if (Plansza[i-2, i2-1] == "krolB")
                                                {
                                                    SzachyC[i-2, i2-1] = false;
                                                } 
                                            }
                                            else
                                            {
                                                HiddenSzachyC[i-2, i2-1] = true;
                                            }
                                        }
                                        if (!(i2+1 >= 8))
                                        {
                                            if (Plansza[i-2,i2+1] != null)
                                            {
                                                if (Plansza[i-2, i2+1] == "krolB")
                                                {
                                                    SzachyC[i-2, i2+1] = false;
                                                } 
                                            }
                                            else
                                            {
                                                SzachyC[i-2, i2+1] = true;
                                            }
                                        }
                                    }
                                    if (!(i-1 <= -1))
                                    {
                                        if (!(i2-2 <= -1))
                                        {
                                            if (Plansza[i-1, i2-2] != null)
                                            {
                                                if (Plansza[i-1, i2-2] == "krolB")
                                                {
                                                    SzachyC[i-1, i2-2] = true;
                                                } 
                                            }
                                            else
                                            {
                                                HiddenSzachyC[i-1, i2-2] = true;
                                            }
                                        }
                                        if (!(i2+2 >= 8))
                                        {
                                            if (Plansza[i-1, i2+2] != null)
                                            {
                                                if (Plansza[i-1, i2+2] == "krolB")
                                                {
                                                    SzachyC[i-1, i2+2] = true;
                                                } 
                                            }
                                            else
                                            {
                                                HiddenSzachyC[i-1, i2+2] = true;
                                            }
                                        }
                                    }
                                    if (!(i+2 >= 8))
                                    {
                                        if (!(i2-1 <= -1))
                                        {
                                            if (Plansza[i+2,i2-1] != null)
                                            {
                                                if (Plansza[i+2, i2-1] == "krolB")
                                                {
                                                    SzachyC[i+2, i2-1] = true;
                                                } 
                                            }
                                            else
                                            {
                                                HiddenSzachyC[i+2,i2-1] = true;
                                            }
                                        }
                                        if (!(i2+1 >= 8))
                                        {
                                            if (Plansza[i+2,i2+1] != null)
                                            {
                                                if (Plansza[i+2, i2+1] == "krolB")
                                                {
                                                    SzachyC[i+2, i2+1] = true;
                                                } 
                                            }
                                            else
                                            {
                                                HiddenSzachyC[i+2,i2+1] = true;
                                            }
                                        }
                                    }
                                    if (!(i+1 >= 8))
                                    {
                                        if (!(i2-2 <= -1))
                                        {
                                            if (Plansza[i+1,i2-2] != null)
                                            {
                                                if (Plansza[i+1, i2-2] == "krolB")
                                                {
                                                    SzachyC[i+1, i2-2] = true;
                                                } 
                                            }
                                            else
                                            {
                                                HiddenSzachyC[i+1, i2-2] = true;
                                            }
                                        }
                                        if (!(i2+2 >= 8))
                                        {
                                            if (Plansza[i+1,i2+2] != null)
                                            {
                                                if (Plansza[i+1, i2+2] == "krolB")
                                                {
                                                    SzachyC[i+1, i2+2] = true;
                                                } 
                                            }
                                            else
                                            {
                                                HiddenSzachyC[i+1,i2+2] = true;
                                            }
                                        }
                                    }
                                    break;
                            }
                            break;
                        case "goniec":
                            void CheckBishopRightUp(int _i, int _i2, char Team)
                            {
                                if((_i > -1) && (_i2 < 8))
                                {
                                    if(Plansza[_i,_i2] != null)
                                    {
                                        if(Plansza[_i, _i2].StartsWith("krol"))
                                        {
                                            switch (Team)
                                            {
                                                case 'B':
                                                    if(Plansza[_i, _i2].Last() == 'C')
                                                    {
                                                        SzachyB[_i, _i2] = true;
                                                    }
                                                    break;
                                                case 'C':
                                                    if(Plansza[_i, _i2].Last() == 'B')
                                                    {
                                                        SzachyC[_i, _i2] = true;
                                                    }
                                                    break;
                                            } 
                                        }
                                    }
                                    else
                                    {
                                        switch (Team)
                                        {
                                            case 'B':
                                                HiddenSzachyB[_i, _i2] = true;
                                                break;
                                            case 'C':
                                                HiddenSzachyC[_i, _i2] = true;
                                                break;
                                        }
                                        CheckBishopRightUp(_i-1,_i2+1,Team);
                                    }
                                }
                                return;
                            }
                            void CheckBishopRightDown(int _i, int _i2, char Team)
                            {
                                if(!(_i >= 8) && !(_i2 >= 8))
                                {
                                    if(Plansza[_i,_i2] != null)
                                    {
                                        if(Plansza[_i,_i2].StartsWith("krol"))
                                        {
                                            switch (Team)
                                            {
                                                case 'B':
                                                    if(Plansza[_i,_i2].Last() == 'C')
                                                    {
                                                        SzachyB[_i, _i2] = true;
                                                    }
                                                    break;
                                                case 'C':
                                                    if(Plansza[_i, _i2].Last() == 'B')
                                                    {
                                                        SzachyC[_i, _i2] = true;
                                                    }
                                                    break;
                                            } 
                                        }
                                    }
                                    else
                                    {
                                        switch (Team)
                                        {
                                            case 'B':
                                                HiddenSzachyB[_i, _i2] = true;
                                                break;
                                            case 'C':
                                                HiddenSzachyC[_i, _i2] = true;
                                                break;
                                        }
                                        CheckBishopRightDown(_i+1,_i2+1,Team);
                                    }
                                }
                                return;
                            }
                            void CheckBishopLeftUp(int _i, int _i2, char Team)
                            {
                                if(!(_i <= -1) && !(_i2 <= -1))
                                {
                                    if(Plansza[_i,_i2] != null)
                                    {
                                        if(Plansza[_i,_i2].StartsWith("krol"))
                                        {
                                            switch(Team)
                                            {
                                                case 'B':
                                                    if(Plansza[_i,_i2].Last() == 'C')
                                                    {
                                                        SzachyB[_i,_i2] = true;
                                                    }
                                                    break;
                                                case 'C':
                                                    if(Plansza[_i,_i2].Last() == 'B')
                                                    {
                                                        SzachyC[_i,_i2] = true;
                                                    }
                                                    break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        switch (Team)
                                        {
                                            case 'B':
                                                HiddenSzachyB[_i, _i2] = true;
                                                break;
                                            case 'C':
                                                HiddenSzachyC[_i, _i2] = true;
                                                break;
                                        }
                                        CheckBishopLeftUp(_i-1,_i2-1,Team);
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
                                    if(Plansza[_i,_i2] != null)
                                    {
                                        if(Plansza[_i,_i2].StartsWith("krol"))
                                        {
                                            switch (Team)
                                            {
                                                case 'B':
                                                    if (Plansza[_i, _i2].Last() == 'C')
                                                    {
                                                        SzachyB[_i, _i2] = true;
                                                    }
                                                    break;
                                                case 'C':
                                                    if (Plansza[_i,_i2].Last() == 'B')
                                                    {
                                                        SzachyC[_i,_i2] = true;
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
                                                HiddenSzachyB[_i,_i2] = true;
                                                break;
                                            case 'C':
                                                HiddenSzachyC[_i,_i2] = true;
                                                break;
                                        }
                                        CheckBishopLeftDown(_i+1,_i2-1,Team);
                                    }
                                }
                                return;
                            }
                            switch (Plansza[i, i2].Last())
                            {
                                case 'B':
                                    //
                                    // Guess who can't use async
                                    // uwu
                                    Thread CheckRightUpB = new Thread(() => CheckBishopRightUp(i-1,i2+1,'B'));
                                    Thread CheckRightDownB = new Thread(() => CheckBishopRightDown(i+1,i2+1,'B'));
                                    Thread CheckLeftUpB = new Thread(() => CheckBishopLeftUp(i-1,i2-1,'B'));
                                    Thread CheckLeftDownB = new Thread(() => CheckBishopLeftDown(i+1,i2-1,'B'));
                                    CheckRightUpB.Start();
                                    CheckRightDownB.Start();
                                    CheckLeftUpB.Start();
                                    CheckLeftDownB.Start();
                                    CheckRightUpB.Join();
                                    CheckRightDownB.Join();
                                    CheckLeftUpB.Join();
                                    CheckLeftDownB.Join();
                                    break;
                                case 'C':
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
                                    break;
                            }
                            break;
                        case "wieza":
                            switch (Plansza[i, i2].Last())
                            {
                                case 'B':
                                    break;
                                case 'C':
                                    break;
                            }
                            break;
                        case "krol":
                            switch (Plansza[i, i2].Last())
                            {
                                case 'B':
                                    break;
                                case 'C':
                                    break;
                            }
                            break;
                        case "krolowa":
                            switch (Plansza[i, i2].Last())
                            {
                                case 'B':
                                    break;
                                case 'C':
                                    break;
                            }
                            break;
                    }
                }
                //PAMIĘTAJ O SPRAWDZANIU CZY KRÓL MA SZACHA U OBU DRUŻYN
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
                for (int linia = 0, x = 0; linia <= 7; linia++)
                {
                    Console.WriteLine("\t-----------------------------------------------------------------");
                    Console.Write("\t|");
                    while (x < 8)
                    {
                        if (Plansza[linia, x].HasFlag(ChessPiece.King | ChessPiece.TeamC))
                        {
                            if (SzachyBC[linia, x].HasFlag(ChessPiece.TeamC))
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
                    while (x < 8)
                    {
                        if (Plansza[linia, x].HasFlag(ChessPiece.None))
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
                        if (Plansza[linia, x].HasFlag(ChessPiece.King | ChessPiece.TeamB))
                        {
                            if (SzachyBC[linia, x].HasFlag(ChessPiece.TeamC))
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
                }
                //Console.WriteLine("|       |       |       |       |       |       |       |       |");
            }
            else
            {
                for (int linia = 7, x = 0; linia > -1; linia--)
                {
                    Console.WriteLine("\t-----------------------------------------------------------------");
                    Console.Write("\t|");
                    while (x < 8)
                    {
                        if (Plansza[linia, x].HasFlag(ChessPiece.King | ChessPiece.TeamB))
                        {
                            if (SzachyBC[linia, x].HasFlag(ChessPiece.TeamC))
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
                    while (x < 8)
                    {
                        if (Plansza[linia, x].HasFlag(ChessPiece.None))
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
                        if (Plansza[linia, x].HasFlag(ChessPiece.King | ChessPiece.TeamC))
                        {
                            if (SzachyBC[linia, x].HasFlag(ChessPiece.TeamC))
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
                case "pionek":
                    RuchPionem(Pozycja1, Pozycja2);
                    break;
                case "kon":
                    RuchKoniem(Pozycja1, Pozycja2);
                    break;
                case "goniec":
                    RuchGońcem(Pozycja1, Pozycja2);
                    break;
                case "wieza":
                    RuchWieżą(Pozycja1, Pozycja2);
                    break;
                case "krol":
                    RuchKrólem(Pozycja1, Pozycja2);
                    break;
                case "krolowa":
                    RuchKrólową(Pozycja1, Pozycja2);
                    break;
            }
            Program.hasEnemyMoved = true;
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
            Program.isSending = false;
        }
        public static void Rozgrywka()
        {
            Szachy.PostawPionki();
            bool koniecrozgrywki = false;
            switch (Program.playerTeam)
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
            for (int tura = 1; !koniecrozgrywki; tura++)
            {
                if (tura.ToString().Length == 1)
                {
                    Console.WriteLine(new String('-', 36) + $" Tura {tura} " + new String('-', 36));
                }
                else if (tura.ToString().Length == 2)
                {
                    Console.WriteLine(new String('-', 35) + $" Tura {tura} " + new String('-', 36));
                }
                else if (tura.ToString().Length == 3)
                {
                    Console.WriteLine(new String('-', 35) + $" Tura {tura} " + new String('-', 35));
                }
                else
                {
                    //może jakiś algorytm na to here
                    //if costam % 2 == 0 musi być, kiedyś to napiszę
                }
                switch (Program.playerTeam)
                {
                    case 'B':
                        Szachy.NarysujPlansze();
                        //oznacz szachy
                        //wykonaj turę
                        break;
                    case 'C':
                        Szachy.NarysujPlansze();
                        while (!Program.hasEnemyMoved)
                        {
                            Thread.Sleep(1000);
                        }
                        Program.hasEnemyMoved = false;
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

        public static FtpClient client = new FtpClient();
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
                while (i <= lines.Count)
                {
                    if (!lines[i].StartsWith(Nick))
                    {
                        lines[i] = lines[i].Substring(enemyNick.Length + 2);
                        if (lines[i].StartsWith("CZAT"))
                        {
                            lines[i] = lines[i].Substring(enemyNick.Length + 2);
                            odebranoWiad = true;
                            //"CZAT {wiad}"
                            lines[i] = lines[i].Remove(0, 5);
                            lines[i] = lines[i].Trim();
                            lines[i] = lines[i].Insert(0, $"{enemyNick}: ");
                            receivedMessages.Add(lines[i]);
                        }
                        else if (lines[i].Contains(">>"))
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
                            Szachy.WykonajRuch(Pozycja1, Pozycja2);
                        }
                        else if (lines[i].StartsWith("CLOSING"))
                        {
                            lines[i] = lines[i].Substring(enemyNick.Length + 2);
                            AppendText("OK", "StartingSessions", ID);
                            currentfolder = "ActiveSessions";
                        }
                        else if (lines[i].StartsWith("OK"))
                        {
                            lines[i] = lines[i].Substring(enemyNick.Length + 2);
                            //"OK"
                            client.MoveFile($"/SzachySerwer/StartingSessions/{ID}.txt", $"/SzachySerwer/ActiveSessions/{ID}.txt");
                        }
                        else if (lines[i].StartsWith("JOIN"))
                        {
                            lines[i] = lines[i].Substring(enemyNick.Length + 2);
                            enemyNick = lines[i].Remove(0, 5).Trim();
                            hasOtherPlayerJoined = true;
                        }
                        else if (lines[i].StartsWith("EVENT"))
                        {
                            receivedMessages.Add(lines[i].Substring(0, 6));
                        }
                    }
                    i++;
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
                List<string> sentMessages = new List<string>();
                for (int i = lastCountChat + 1; i <= countNow; i++)
                {
                    if (!chatLog[i].StartsWith(Nick))
                    {
                        sentMessages.Add($"{Nick}: {chatLog[i]}");
                    }
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
                            Console.Write("Nie udalo sie polaczyc z serwerem, sprobowac ponownie? t/n\n(Odpowiedz \"n\" przerywa sesje, nie jest to wskazane.)\n>");
                        connrepeatgoto:
                            string connrepeat = Console.ReadLine().ToLower();
                            if (connrepeat.Contains('t'))
                            {
                                repeati++;
                            }
                            else if (connrepeat.Contains('n'))
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
            Szachy.PostawPionki();
            Szachy.OznaczSzachy(false);
            Szachy.NarysujPlansze();
            playerTeam = 'C';
            Szachy.NarysujPlansze();
            if (args.Length > 0)
            {
                if (args[0] == "true")
                {
                    BasicIO = true;
                }
            }
        menuglowne:
            Console.WriteLine($"Obecny czas: {DateTime.Now}/{(Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds}");
            Console.Write("Polaczyc sie z serwerem? t/n\n>");
            string czy_polaczyc = Console.ReadLine().ToLower();
            if (czy_polaczyc.Contains('t'))
            {
                Console.WriteLine("Podaj swoj nick:\n>");
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
                    if (connrepeat.Contains('t'))
                    {
                        goto polacz;
                    }
                    else if (connrepeat.Contains('n'))
                    {
                        Console.WriteLine("Cofanie do menu glownego");
                        goto menuglowne;
                    }
                }
            }
            else if (czy_polaczyc.Contains('n'))
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
            catch
            {
                client.Disconnect();
                Console.Write("Blad odbierania lobby. Sprobowac ponownie? t/n\n>");
                string tryagainlobby = Console.ReadLine();
                if (tryagainlobby.ToLower().Contains('t'))
                {
                    goto lobby;
                }
            }
            odswiezlobby = false;
            wykonajconnect = false;
            bool isCancel = false;
            Console.Write("\nPodaj ID sesji publicznej (z lobby) lub prywatnej (otrzymanej od znajomego) aby dolaczyc do niej. Utworz nowa sesje poleceniem \"create\". Odswiez lobby poleceniem \"refresh\"\n>");
            string connect_to_id = Console.ReadLine();
            if (connect_to_id.ToLower().Contains("create"))
            {
                bool isPublic;
            podajnazwesesji:
                Console.Write("\nPodaj nazwe sesji, ktora chcesz stworzyc lub napisz \"cancel\" aby wrocic do lobby\n>");
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
                    Console.WriteLine("\nCzy sesja ma byc prywatna? t/n\n>");
                    string ispublicstr = Console.ReadLine().ToLower();
                    if (ispublicstr.Contains('t'))
                    {
                        isPublic = true;
                    }
                    else if (ispublicstr.Contains('n'))
                    {
                        isPublic = false;
                    }
                    else
                    {
                        Console.WriteLine("Niepoprawna odpowiedz:" + ispublicstr);
                        goto ispublicgoto;
                    }
                    Console.Write($"Tworzenie sesji o nazwie \"{nazwasesji}\", kontynuowac? t/n\n>");
                    string napewno = Console.ReadLine().ToLower();
                    if (napewno.Contains('t'))
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
                        Szachy.Rozgrywka();
                        //(Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds
                    }
                    else if (napewno.ToLower().Contains('n'))
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
                            switch (enemyTeam)
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
                Thread.Sleep(1500);
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
    }
}