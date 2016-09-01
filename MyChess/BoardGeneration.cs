using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace MyChess
{
    public class BoardGeneration
    {
        public static void importFEN(String fenString)
        {
            ChessBoard.WP = 0; ChessBoard.WN = 0; ChessBoard.WB = 0;
            ChessBoard.WR = 0; ChessBoard.WQ = 0; ChessBoard.WK = 0;
            ChessBoard.BP = 0; ChessBoard.BN = 0; ChessBoard.BB = 0;
            ChessBoard.BR = 0; ChessBoard.BQ = 0; ChessBoard.BK = 0;
            ChessBoard.CWK = false; ChessBoard.CWQ = false;
            ChessBoard.CBK = false; ChessBoard.CBQ = false;
            int charIndex = 0;
            int boardIndex = 0;
            while (fenString[charIndex] != ' ')
            {
                switch (fenString[charIndex++])
                {
                    case 'P': ChessBoard.WP |= (1ul << boardIndex++);
                        break;
                    case 'p': ChessBoard.BP |= (1ul << boardIndex++);
                        break;
                    case 'N': ChessBoard.WN |= (1ul << boardIndex++);
                        break;
                    case 'n': ChessBoard.BN |= (1ul << boardIndex++);
                        break;
                    case 'B': ChessBoard.WB |= (1ul << boardIndex++);
                        break;
                    case 'b': ChessBoard.BB |= (1ul << boardIndex++);
                        break;
                    case 'R': ChessBoard.WR |= (1ul << boardIndex++);
                        break;
                    case 'r': ChessBoard.BR |= (1ul << boardIndex++);
                        break;
                    case 'Q': ChessBoard.WQ |= (1ul << boardIndex++);
                        break;
                    case 'q': ChessBoard.BQ |= (1ul << boardIndex++);
                        break;
                    case 'K': ChessBoard.WK |= (1ul << boardIndex++);
                        break;
                    case 'k': ChessBoard.BK |= (1ul << boardIndex++);
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
            bool just = (fenString[++charIndex] == 'w');
            charIndex += 2;
            while (fenString[charIndex] != ' ')
            {
                switch (fenString[charIndex++])
                {
                    case '-':
                        break;
                    case 'K': ChessBoard.CWK = true;
                        break;
                    case 'Q': ChessBoard.CWQ = true;
                        break;
                    case 'k': ChessBoard.CBK = true;
                        break;
                    case 'q': ChessBoard.CBQ = true;
                        break;
                    default:
                        break;
                }
            }
            if (fenString[++charIndex] != '-')
            {
                ChessBoard.EP = Moves.FileMasks8[fenString[charIndex++] - 'a'];
            }
        }
        public static void arrayToBitboards(string[,] chessBoard, ref ulong WP, ref ulong WN, ref ulong WB, ref ulong WR, ref ulong WQ, ref ulong WK, ref ulong BP, ref ulong BN, ref ulong BB, ref ulong BR, ref ulong BQ, ref ulong BK)
        {
            string Binary;
            for (int i = 0; i < 64; i++)
            {
                Binary = "0000000000000000000000000000000000000000000000000000000000000000";
                Binary = Binary.Substring(i + 1) + "1" + Binary.Substring(0, i);
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
        }
        public static ulong convertstringToBitboard(string Binary)
        {
            return Convert.ToUInt64(Binary, 2);
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