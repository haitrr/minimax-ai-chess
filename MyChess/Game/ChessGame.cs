using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;
using System.Collections.Concurrent;
namespace MyChess
{
    public static class ChessGame
    {
        public static System.Windows.Forms.TextBox heuristicValueTextBox, predictedHeuristicValue, searchedDepth;
        public static System.Windows.Forms.PictureBox whiteTurn, blackTurn;
        public static System.Windows.Forms.Label thinking;
        public enum Difficulty { veryEasy,easy,hard,veryHard};
        public static Difficulty difficulty { get; set; }
        public static string convertToAllDigitMoves(string moves)
        {
            List<char> move=moves.ToList<char>();
            for (int i = 0; i < moves.Length;i++)
            {
                if(moves[i]=='P')
                {
                    move[i] = move[i-3];
                    move[i - 1] = '0';
                    move[i - 2] = '1';
                    for(int j=i+1;j<=i+12;j++)
                    {
                        move[j]='9';
                    }
                }
                if(move[i]=='E')
                {
                    move[i] = move[i - 3];
                    move[i - 1] = '0';
                    move[i - 2] = '1';
                }
            }
                return move.ToString();
        }
    }
}
