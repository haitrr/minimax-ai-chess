using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace BitBoardChessCreating
{
    public class BoardGeneration
    {
        public static void importFEN(String fenString)
        {
            //not chess960 compatible
            Program.WP = 0; Program.WN = 0; Program.WB = 0;
            Program.WR = 0; Program.WQ = 0; Program.WK = 0;
            Program.BP = 0; Program.BN = 0; Program.BB = 0;
            Program.BR = 0; Program.BQ = 0; Program.BK = 0;
            Program.CWK = false; Program.CWQ = false;
            Program.CBK = false; Program.CBQ = false;
            int charIndex = 0;
            int boardIndex = 0;
            while (fenString[charIndex] != ' ')
            {
                switch (fenString[charIndex++])
                {
                    case 'P': Program.WP |= (1ul << boardIndex++);
                        break;
                    case 'p': Program.BP |= (1ul << boardIndex++);
                        break;
                    case 'N': Program.WN |= (1ul << boardIndex++);
                        break;
                    case 'n': Program.BN |= (1ul << boardIndex++);
                        break;
                    case 'B': Program.WB |= (1ul << boardIndex++);
                        break;
                    case 'b': Program.BB |= (1ul << boardIndex++);
                        break;
                    case 'R': Program.WR |= (1ul << boardIndex++);
                        break;
                    case 'r': Program.BR |= (1ul << boardIndex++);
                        break;
                    case 'Q': Program.WQ |= (1ul << boardIndex++);
                        break;
                    case 'q': Program.BQ |= (1ul << boardIndex++);
                        break;
                    case 'K': Program.WK |= (1ul << boardIndex++);
                        break;
                    case 'k': Program.BK |= (1ul << boardIndex++);
                        break;
                    case '/':
                        break;
                    case '1': boardIndex++;
                        break;
                    case '2': boardIndex += 2;
                        break;
                    case '3': boardIndex += 3;
                        break;
                    case '4': boardIndex += 4;
                        break;
                    case '5': boardIndex += 5;
                        break;
                    case '6': boardIndex += 6;
                        break;
                    case '7': boardIndex += 7;
                        break;
                    case '8': boardIndex += 8;
                        break;
                    default:
                        break;
                }
            }
            Program.WhiteToMove = (fenString[++charIndex] == 'w');
            charIndex += 2;
            while (fenString[charIndex] != ' ')
            {
                switch (fenString[charIndex++])
                {
                    case '-':
                        break;
                    case 'K': Program.CWK = true;
                        break;
                    case 'Q': Program.CWQ = true;
                        break;
                    case 'k': Program.CBK = true;
                        break;
                    case 'q': Program.CBQ = true;
                        break;
                    default:
                        break;
                }
            }
            if (fenString[++charIndex] != '-')
            {
                Program.EP = Moves.FileMasks8[fenString[charIndex++] - 'a'];
            }
            //the rest of the fenString is not yet utilized
        }
        public static void initiateStandardChess(ref ulong WP, ref ulong WN, ref ulong WB, ref ulong WR, ref ulong WQ, ref ulong WK, ref ulong BP, ref ulong BN, ref  ulong BB, ref ulong BR, ref ulong BQ, ref ulong BK)
        {
            //ulong WP = 0L, WN = 0L, WB = 0L, WR = 0L, WQ = 0L, WK = 0L, BP = 0L, BN = 0L, BB = 0L, BR = 0L, BQ = 0L, BK = 0L,EP=0L;
            string[,] chessBoard = new string[8, 8]{
                {"r","n","b","q","k","b","n","r"},
                {"p","p","p","p","p","p","p","p"},
                {" "," "," "," "," "," "," "," "},
                {" "," "," "," "," "," "," "," "},
                {" "," "," "," "," "," "," "," "},
                {" "," "," "," "," "," "," "," "},
                {"P","P","P","P","P","P","P","P"},
                {"R","N","B","Q","K","B","N","R"}};
            arrayToBitboards(chessBoard, ref WP, ref WN, ref WB,ref  WR,ref  WQ, ref WK, ref BP, ref BN,ref  BB,ref BR,ref BQ,ref BK);
            //Move.possibleMovesW("", WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK);
            //drawArray(WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK);
        }
        public static void initiateChess960()
        {
            ulong WP = 0L, WN = 0L, WB = 0L, WR = 0L, WQ = 0L, WK = 0L, BP = 0L, BN = 0L, BB = 0L, BR = 0L, BQ = 0L, BK = 0L;
            string[,] chessBoard = new string[8, 8]{
            {" "," "," "," "," "," "," "," "},
            {"p","p","p","p","p","p","p","p"},
            {" "," "," "," "," "," "," "," "},
            {" "," "," "," "," "," "," "," "},
            {" "," "," "," "," "," "," "," "},
            {" "," "," "," "," "," "," "," "},
            {"P","P","P","P","P","P","P","P"},
            {" "," "," "," "," "," "," "," "}};
            //step 1:
            Random random = new Random();
            int random1 = random.Next(0, 8);
            chessBoard[0, random1] = "b";
            chessBoard[7, random1] = "B";
            //step 2:
            int random2 = random.Next(0, 8);
            while (random2 % 2 == random1 % 2)
            {
                random2 = random.Next(0, 8);
            }
            chessBoard[0, random2] = "b";
            chessBoard[7, random2] = "B";
            //step 3:
            int random3 = random.Next(0, 8);
            while (random3 == random1 || random3 == random2)
            {
                random3 = random.Next(0, 8);
            }
            chessBoard[0, random3] = "q";
            chessBoard[7, random3] = "Q";
            //step 4:
            int random4a = random.Next(0, 8);
            int counter = 0;
            int loop = 0;
            while (counter - 1 < random4a)
            {
                if (" ".Equals(chessBoard[0, loop])) { counter++; }
                loop++;
            }
            chessBoard[0, loop - 1] = "n";
            chessBoard[7, loop - 1] = "N";
            int random4b = random.Next(0, 4);
            counter = 0;
            loop = 0;
            while (counter - 1 < random4b)
            {
                if (" ".Equals(chessBoard[0, loop])) { counter++; }
                loop++;
            }
            chessBoard[0, loop - 1] = "n";
            chessBoard[7, loop - 1] = "N";
            //step 5:
            counter = 0;
            while (!" ".Equals(chessBoard[0, counter]))
            {
                counter++;
            }
            chessBoard[0, counter] = "r";
            chessBoard[7, counter] = "R";
            while (!" ".Equals(chessBoard[0, counter]))
            {
                counter++;
            }
            chessBoard[0, counter] = "k";
            chessBoard[7, counter] = "K";
            while (!" ".Equals(chessBoard[0, counter]))
            {
                counter++;
            }
            chessBoard[0, counter] = "r";
            chessBoard[7, counter] = "R";
            //arrayToBitboards(chessBoard, WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK);
        }
        public static void arrayToBitboards(string[,] chessBoard, ref ulong WP,ref ulong WN,ref ulong WB,ref ulong WR,ref ulong WQ,ref ulong WK,ref ulong BP,ref ulong BN,ref ulong BB,ref ulong BR, ref ulong BQ, ref ulong BK)
        {
            string Binary;
            for (int i = 0; i < 64; i++)
            {
                Binary = "0000000000000000000000000000000000000000000000000000000000000000";
                Binary = Binary.Substring(i + 1) + "1" +Binary.Substring(0, i);
                switch (chessBoard[i / 8, i % 8])
                {
                    case "P": WP += convertstringToBitboard(Binary);
                        break;
                    case "N": WN += convertstringToBitboard(Binary);
                        break;
                    case "B": WB += convertstringToBitboard(Binary);
                        break;
                    case "R": WR += convertstringToBitboard(Binary);
                        break;
                    case "Q": WQ += convertstringToBitboard(Binary);
                        break;
                    case "K": WK += convertstringToBitboard(Binary);
                        break;
                    case "p": BP += convertstringToBitboard(Binary);
                        break;
                    case "n": BN += convertstringToBitboard(Binary);
                        break;
                    case "b": BB += convertstringToBitboard(Binary);
                        break;
                    case "r": BR += convertstringToBitboard(Binary);
                        break;
                    case "q": BQ += convertstringToBitboard(Binary);
                        break;
                    case "k": BK += convertstringToBitboard(Binary);
                        break;
                }
            }
            drawArray(WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK);
            //Program.WP=WP; Program.WN=WN; Program.WB=WB;
            //Program.WR=WR; Program.WQ=WQ; Program.WK=WK;
            //Program.BP=BP; Program.BN=BN; Program.BB=BB;
            //Program.BR=BR; Program.BQ=BQ; Program.BK=BK;
        }
        public static ulong convertstringToBitboard(string Binary)
        {
            //if (Binary.ElementAt(0) == '0')
            //{//not going to be a negative number
                return Convert.ToUInt64(Binary, 2);
            //}
            //else
            //{
            //    return Convert.ToUInt64("1" + Binary.Substring(2), 2) * 2;
            //}
        }
        public static void drawArray(ulong WP, ulong WN, ulong WB, ulong WR, ulong WQ, ulong WK, ulong BP, ulong BN, ulong BB, ulong BR, ulong BQ, ulong BK)
        {
            string[,] chessBoard = new string[8, 8];
            for (int i = 0; i < 64; i++)
            {
                chessBoard[i / 8, i % 8] = " ";
            }
            for (int i = 0; i < 64; i++)
            {
                if (((WP >> i) & 1) == 1) { chessBoard[i / 8, i % 8] = "P"; }
                if (((WN >> i) & 1) == 1) { chessBoard[i / 8, i % 8] = "N"; }
                if (((WB >> i) & 1) == 1) { chessBoard[i / 8, i % 8] = "B"; }
                if (((WR >> i) & 1) == 1) { chessBoard[i / 8, i % 8] = "R"; }
                if (((WQ >> i) & 1) == 1) { chessBoard[i / 8, i % 8] = "Q"; }
                if (((WK >> i) & 1) == 1) { chessBoard[i / 8, i % 8] = "K"; }
                if (((BP >> i) & 1) == 1) { chessBoard[i / 8, i % 8] = "p"; }
                if (((BN >> i) & 1) == 1) { chessBoard[i / 8, i % 8] = "n"; }
                if (((BB >> i) & 1) == 1) { chessBoard[i / 8, i % 8] = "b"; }
                if (((BR >> i) & 1) == 1) { chessBoard[i / 8, i % 8] = "r"; }
                if (((BQ >> i) & 1) == 1) { chessBoard[i / 8, i % 8] = "q"; }
                if (((BK >> i) & 1) == 1) { chessBoard[i / 8, i % 8] = "k"; }
            }
            for (int i = 0; i < 8; i++)
            {
                string temp = "[";
                for (int j = 0; j < 8; j++)
                {
                    temp += chessBoard[i, j] + ",";

                }
                temp += "]";
                Console.WriteLine(temp);
            }
            Console.WriteLine();
        }
    }
}