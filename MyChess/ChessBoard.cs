using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using MyChess.Properties;

namespace MyChess
{
    public static class ChessBoard
    {
        public enum Difficulty { veryEasy, easy, hard, veryHard };
        public static Difficulty difficulty { get; set; }
        public static Graphics graphics;
        public static int boardHeight;
        public static int boardWidth;
        public static ulong WP = 0L, WN = 0L, WB = 0L, WR = 0L, WQ = 0L, WK = 0L, BP = 0L,
            BN = 0L, BB = 0L, BR = 0L, BQ = 0L, BK = 0L, EP = 0L;
        public static bool CWK = true, CWQ = true, CBK = true, CBQ = true, blackTurn = false;//true=castle is possible
        public static bool staledraw;
        public static char[,] charBoard;
        public static Stack<ulong[]> boardStack;
        public static Stack<bool[]> flagStack;
        public static Stack<string> moveStack;
        public static int selecting;
        public static bool gameOver;
        public static bool white, computer, lategame;
        public static void init()
        {
            moveStack = new Stack<string>();
            BoardGeneration.importFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
            boardHeight = 560;
            boardWidth = 560;
            gameOver = false;
            staledraw = false;
            lategame = false;
            //charBoard = new char[8, 8]
            //{
            //    {'r','n','b','q','k','b','n','r'},
            //    {'p','p','p','p','p','p','p','p'},
            //    {' ',' ',' ',' ',' ',' ',' ',' '},
            //    {' ',' ',' ',' ',' ',' ',' ',' '},
            //    {' ',' ',' ',' ',' ',' ',' ',' '},
            //    {' ',' ',' ',' ',' ',' ',' ',' '},
            //    {'P','P','P','P','P','P','P','P'},
            //    {'R','N','B','Q','K','B','N','R'}
            //};
            charBoard = new char[8, 8];
            updateCharBoard();
            boardStack = new Stack<ulong[]>();
            selecting = -1;
            flagStack = new Stack<bool[]>();
            white = true;
            computer = true;
            makeBestMoves();
        }
        public static void draw()
        {
            int boxsize = boardHeight / 8;
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    if (i % 2 == 0)
                    {
                        if (j % 2 == 0)
                        {
                            graphics.FillRectangle(new SolidBrush(Color.MintCream), j * boxsize, i * boxsize, boxsize, boxsize);
                        }
                        else
                        {
                            graphics.FillRectangle(new SolidBrush(Color.Brown), j * boxsize, i * boxsize, boxsize, boxsize);
                        }
                    }
                    else
                    {
                        if (j % 2 == 0)
                        {
                            graphics.FillRectangle(new SolidBrush(Color.Brown), j * boxsize, i * boxsize, boxsize, boxsize);
                        }
                        else
                        {
                            graphics.FillRectangle(new SolidBrush(Color.MintCream), j * boxsize, i * boxsize, boxsize, boxsize);
                        }
                    }
                    drawPiece(i, j);
                }
            drawSelecting();
            drawTrack();
        }
        public static void makeMove(string move)
        {
            if (gameOver)
            {
                UnSelect();
                return;
            }
            addStack();
            ulong WRt = WR, WKt = WK, BRt = BR, BKt = BK;
            WR = Moves.makeMoveCastle(WR, WK | BK, move, 'R');
            BR = Moves.makeMoveCastle(BR, WK | BK, move, 'r');
            WP = Moves.makeMove(WP, move, 'P'); WN = Moves.makeMove(WN, move, 'N');
            WB = Moves.makeMove(WB, move, 'B'); WR = Moves.makeMove(WR, move, 'R');
            WQ = Moves.makeMove(WQ, move, 'Q'); WK = Moves.makeMove(WK, move, 'K');
            BP = Moves.makeMove(BP, move, 'p'); BN = Moves.makeMove(BN, move, 'n');
            BB = Moves.makeMove(BB, move, 'b'); BR = Moves.makeMove(BR, move, 'r');
            BQ = Moves.makeMove(BQ, move, 'q'); BK = Moves.makeMove(BK, move, 'k');
            EP = Moves.makeMoveEP(WP | BP, move);
            //WR = Moves.makeMoveCastle(WR, WK | BK, move, 'R');
            //BR = Moves.makeMoveCastle(BR, WK | BK, move, 'r');
            //bool CWKt = CWK, CWQt = CWQ, CBKt = CBK, CBQt = CBQ;
            if (char.IsDigit(move[3]))
            {
                int start = (int)(char.GetNumericValue(move[0]) * 8) + (int)(char.GetNumericValue(move[1]));
                if (((1ul << start) & WKt) != 0) { CWK = false; CWQ = false; }
                else if (((1ul << start) & BKt) != 0) { CBK = false; CBQ = false; }
                else if (((1ul << start) & WRt & (1ul << 63)) != 0) { CWK = false; }
                else if (((1ul << start) & WRt & (1ul << 56)) != 0) { CWQ = false; }
                else if (((1ul << start) & BRt & (1ul << 7)) != 0) { CBK = false; }
                else if (((1ul << start) & BRt & 1ul) != 0) { CBQ = false; }
            }
            updateCharBoard();
            selecting = -1;
            white = !white;
            moveStack.Push(move);
            //draw();
            if (Moves.LegalMoves(WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK, EP, CWK, CWQ, CBK, CBQ, white).Length == 0)
            {
                gameOver = true;
                if ((white && (WK & Moves.unSafeForWhite(WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK)) == 0) ||
                        (!white && (BK & Moves.unSafeForBlack(WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK)) == 0))
                    staledraw = true;
            }
            if (BitHelper.NumberOfSetBits(WN | WR | WK | WQ | WB | BB | BN | BQ | BR | BK) < 7)
                lategame = true;
            else lategame = false;
            //drawTrack(move);
        }
        public static void drawTrack()
        {
            if (moveStack.Count >= 1)
            {
                string move = moveStack.Peek();
                string track = convertAllMovesToDigitMoves(move);
                drawSquare((int)char.GetNumericValue(track[0]), (int)char.GetNumericValue(track[1]), false, true);
                drawSquare((int)char.GetNumericValue(track[2]), (int)char.GetNumericValue(track[3]), false, true);
                drawPiece((int)char.GetNumericValue(track[0]), (int)char.GetNumericValue(track[1]));
                drawPiece((int)char.GetNumericValue(track[2]), (int)char.GetNumericValue(track[3]));
            }
        }
        public static void updateCharBoard()
        {
            for (int i = 0; i < 64; i++)
            {
                charBoard[i / 8, i % 8] = ' ';
            }
            for (int i = 0; i < 64; i++)
            {
                if (((WP >> i) & 1) == 1) { charBoard[i / 8, i % 8] = 'P'; }
                if (((WN >> i) & 1) == 1) { charBoard[i / 8, i % 8] = 'N'; }
                if (((WB >> i) & 1) == 1) { charBoard[i / 8, i % 8] = 'B'; }
                if (((WR >> i) & 1) == 1) { charBoard[i / 8, i % 8] = 'R'; }
                if (((WQ >> i) & 1) == 1) { charBoard[i / 8, i % 8] = 'Q'; }
                if (((WK >> i) & 1) == 1) { charBoard[i / 8, i % 8] = 'K'; }
                if (((BP >> i) & 1) == 1) { charBoard[i / 8, i % 8] = 'p'; }
                if (((BN >> i) & 1) == 1) { charBoard[i / 8, i % 8] = 'n'; }
                if (((BB >> i) & 1) == 1) { charBoard[i / 8, i % 8] = 'b'; }
                if (((BR >> i) & 1) == 1) { charBoard[i / 8, i % 8] = 'r'; }
                if (((BQ >> i) & 1) == 1) { charBoard[i / 8, i % 8] = 'q'; }
                if (((BK >> i) & 1) == 1) { charBoard[i / 8, i % 8] = 'k'; }
            }
        }
        public static void addStack()
        {
            ulong[] temp = new ulong[13];
            temp[0] = WP;
            temp[1] = WN;
            temp[2] = WB;
            temp[3] = WR;
            temp[4] = WQ;
            temp[5] = WK;
            temp[6] = BP;
            temp[7] = BN;
            temp[8] = BB;
            temp[9] = BR;
            temp[10] = BQ;
            temp[11] = BK;
            temp[12] = EP;
            boardStack.Push(temp);
            bool[] temp2 = new bool[4];
            temp2[0] = CWK;
            temp2[1] = CWQ;
            temp2[2] = CBK;
            temp2[3] = CBQ;
            flagStack.Push(temp2);
        }
        public static void drawSelecting()
        {
            string moves = convertAllMovesToDigitMoves();
            int boxSize = boardHeight / 8;
            graphics.DrawImage(Resources.selecting, selecting % 8 * boxSize, selecting / 8 * boxSize);
            for (int i = 0; i < moves.Length; i += 4)
            {
                string move = moves.Substring(i, 4);
                if (char.GetNumericValue(move[0]) == selecting / 8 && char.GetNumericValue(move[1]) == selecting % 8)
                {
                    drawSquare((int)char.GetNumericValue(move[2]), (int)char.GetNumericValue(move[3]), true, false);
                    drawPiece((int)char.GetNumericValue(move[2]), (int)char.GetNumericValue(move[3]));
                }
            }
        }
        public static string convertAllMovesToDigitMoves()
        {
            char[] move;
            move = Moves.LegalMoves(WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK, EP, CWK, CWQ, CBK, CBQ, white).ToCharArray();
            if (white)
            {
                for (int i = 0; i < move.Length; i++)
                {
                    if (move[i] == 'P')
                    {

                        move[i] = move[i - 2];

                        move[i - 2] = move[i - 3];
                        move[i - 3] = '1';
                        move[i - 1] = '0';
                        for (int j = i + 1; j <= i + 12; j++)
                        {
                            move[j] = '9';
                        }
                    }
                    if (move[i] == 'E')
                    {
                        int start, end;
                        start = BitHelper.getNumberOfTrailingZeros(Moves.FileMasks8[move[0] - '0'] & Moves.RankMasks8[3]);
                        end = BitHelper.getNumberOfTrailingZeros(Moves.FileMasks8[move[1] - '0'] & Moves.RankMasks8[2]);
                        move[0] = Convert.ToChar(start / 8);
                        move[1] = Convert.ToChar(start % 8);
                        move[2] = Convert.ToChar(end / 8);
                        move[3] = Convert.ToChar(end % 8);
                    }
                }
            }
            else
            {
                for (int i = 0; i < move.Length; i++)
                {
                    if (move[i] == 'P')
                    {
                        move[i] = move[i - 2];
                        move[i - 2] = move[i - 3];
                        move[i - 1] = '1';
                        move[i - 3] = '0';
                        for (int j = i + 1; j <= i + 12; j++)
                        {
                            move[j] = '9';
                        }
                    }
                    if (move[i] == 'E')
                    {
                        int start, end;
                        start = BitHelper.getNumberOfTrailingZeros(Moves.FileMasks8[move[0] - '0'] & Moves.RankMasks8[4]);
                        end = BitHelper.getNumberOfTrailingZeros(Moves.FileMasks8[move[1] - '0'] & Moves.RankMasks8[5]);
                        move[0] = Convert.ToChar(start / 8);
                        move[1] = Convert.ToChar(start % 8);
                        move[2] = Convert.ToChar(end / 8);
                        move[3] = Convert.ToChar(end % 8);
                    }
                }
            }
            return new string(move);
        }
        public static string convertAllMovesToDigitMoves(string moves)
        {
            char[] move;
            move = move = moves.ToCharArray();
            if (white)
            {
                for (int i = 0; i < move.Length; i++)
                {
                    if (move[i] == 'P')
                    {

                        move[i] = move[i - 2];

                        move[i - 2] = move[i - 3];
                        move[i - 3] = '1';
                        move[i - 1] = '0';
                        for (int j = i + 1; j <= i + 12; j++)
                        {
                            move[j] = '9';
                        }
                    }
                    if (move[i] == 'E')
                    {
                        int start, end;
                        start = BitHelper.getNumberOfTrailingZeros(Moves.FileMasks8[move[0] - '0'] & Moves.RankMasks8[3]);
                        end = BitHelper.getNumberOfTrailingZeros(Moves.FileMasks8[move[1] - '0'] & Moves.RankMasks8[2]);
                        move[0] = Convert.ToChar(start / 8);
                        move[1] = Convert.ToChar(start % 8);
                        move[2] = Convert.ToChar(end / 8);
                        move[3] = Convert.ToChar(end % 8);
                    }
                }
            }
            else
            {
                for (int i = 0; i < move.Length; i++)
                {
                    if (move[i] == 'P')
                    {
                        move[i] = move[i - 2];
                        move[i - 2] = move[i - 3];
                        move[i - 1] = '1';
                        move[i - 3] = '0';
                    }
                    if (move[i] == 'E')
                    {
                        int start, end;
                        start = BitHelper.getNumberOfTrailingZeros(Moves.FileMasks8[move[0] - '0'] & Moves.RankMasks8[4]);
                        end = BitHelper.getNumberOfTrailingZeros(Moves.FileMasks8[move[1] - '0'] & Moves.RankMasks8[5]);
                        move[0] = Convert.ToChar(start / 8);
                        move[1] = Convert.ToChar(start % 8);
                        move[2] = Convert.ToChar(end / 8);
                        move[3] = Convert.ToChar(end % 8);
                    }
                }
            }
            return new string(move);
        }
        public static Color blendColor(Color color, Color backColor, double amount)
        {
            byte r = (byte)((color.R * amount) + backColor.R * (1 - amount));
            byte g = (byte)((color.G * amount) + backColor.G * (1 - amount));
            byte b = (byte)((color.B * amount) + backColor.B * (1 - amount));
            return Color.FromArgb(r, g, b);
        }
        public static void drawPiece(int i, int j)
        {
            int boxsize = boardHeight / 8;
            switch (charBoard[i, j])
            {
                case 'p': graphics.DrawImage(Resources.Pawn_Black, j * boxsize + boxsize / 7, i * boxsize + boxsize / 7); break;
                case 'n': graphics.DrawImage(Resources.Knight_Black, j * boxsize + boxsize / 7, i * boxsize + boxsize / 7); break;
                case 'r': graphics.DrawImage(Resources.Rook_Black, j * boxsize + boxsize / 7, i * boxsize + boxsize / 7); break;
                case 'q': graphics.DrawImage(Resources.Queen_Black, j * boxsize + boxsize / 7, i * boxsize + boxsize / 7); break;
                case 'k': graphics.DrawImage(Resources.King_Black, j * boxsize + boxsize / 7, i * boxsize + boxsize / 7); break;
                case 'b': graphics.DrawImage(Resources.Bishop_Black, j * boxsize + boxsize / 7, i * boxsize + boxsize / 7); break;
                case 'P': graphics.DrawImage(Resources.Pawn_White, j * boxsize + boxsize / 7, i * boxsize + boxsize / 7); break;
                case 'N': graphics.DrawImage(Resources.Knight_White, j * boxsize + boxsize / 7, i * boxsize + boxsize / 7); break;
                case 'R': graphics.DrawImage(Resources.Rook_White, j * boxsize + boxsize / 7, i * boxsize + boxsize / 7); break;
                case 'Q': graphics.DrawImage(Resources.Queen_White, j * boxsize + boxsize / 7, i * boxsize + boxsize / 7); break;
                case 'K': graphics.DrawImage(Resources.King_White, j * boxsize + boxsize / 7, i * boxsize + boxsize / 7); break;
                case 'B': graphics.DrawImage(Resources.Bishop_White, j * boxsize + boxsize / 7, i * boxsize + boxsize / 7); break;
            }
        }
        public static Color getSquareColor(int i, int j)
        {
            if (i % 2 == 0)
            {
                if (j % 2 == 0)
                {
                    return Color.MintCream;
                }
                else
                {
                    return Color.Brown;
                }
            }
            else
            {
                if (j % 2 == 0)
                {
                    return Color.Brown;
                }
                else
                {
                    return Color.MintCream;
                }
            }
        }
        public static void UnSelect()
        {
            if (selecting == -1) return;
            string moves = convertAllMovesToDigitMoves();
            int boxSize = boardHeight / 8;
            for (int i = 0; i < moves.Length; i += 4)
            {
                string move = moves.Substring(i, 4);
            }
            selecting = -1;
        }
        public static void drawSquare(int x, int y, bool possible, bool justMove)
        {
            int squareSize = boardHeight / 8;
            Color color = Color.Black;
            if (possible)
            {
                color = blendColor(getSquareColor(x, y), Color.Green, 0.5);
            }
            else if (justMove)
            {
                color = blendColor(getSquareColor(x, y), Color.Blue, 0.7);
            }
            else color = getSquareColor(x, y);
            graphics.FillRectangle(new SolidBrush(color), y * squareSize, x * squareSize, squareSize, squareSize);
        }
        public static string checkMove(int x, int y)
        {
            string rawPossibleMoves = Moves.LegalMoves(WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK, EP, CWK, CWQ, CBK, CBQ, white);
            string convertedMoves = convertAllMovesToDigitMoves();
            for (int i = 0; i < convertedMoves.Length; i += 4)
            {
                string move = convertedMoves.Substring(i, 4);
                if (char.GetNumericValue(move[0]) == selecting / 8 && char.GetNumericValue(move[1]) == selecting % 8 && char.GetNumericValue(move[2]) == x && char.GetNumericValue(move[3]) == y)
                {
                    return rawPossibleMoves.Substring(i, 4);
                }
            }
            return null;
        }
        public static void makeBestMoves()
        {
            string move = "";
            switch (difficulty)
            {
                case Difficulty.easy:
                    move = Search.interationDeepening(3, 0, 500, WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK, EP, CWK, CWQ, CBK, CBQ, white);
                    break;
                case Difficulty.veryEasy:
                    move = Search.interationDeepening(2, 0, 500, WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK, EP, CWK, CWQ, CBK, CBQ, white);
                    break;
                case Difficulty.hard:
                    move = Search.interationDeepening(4, 2, 1000, WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK, EP, CWK, CWQ, CBK, CBQ, white);
                    break;
                case Difficulty.veryHard:
                    move = Search.interationDeepening(99, 4, 1000, WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK, EP, CWK, CWQ, CBK, CBQ, white);
                    break;
            }
            makeMove(move);
        }
        public static void undoMove()
        {
            if (boardStack.Count != 0)
            {
                gameOver = false;
                staledraw = false;
                if (boardStack.Count == 1 & white != computer) return;
                ulong[] temp = boardStack.Pop();
                WP = temp[0];
                WN = temp[1];
                WB = temp[2];
                WR = temp[3];
                WQ = temp[4];
                WK = temp[5];
                BP = temp[6];
                BN = temp[7];
                BB = temp[8];
                BR = temp[9];
                BQ = temp[10];
                BK = temp[11];
                EP = temp[12];
                bool[] flags = flagStack.Pop();
                CWK = flags[0];
                CWQ = flags[1];
                CBK = flags[2];
                CBQ = flags[3];
                updateCharBoard();
                white = !white;
                moveStack.Pop();
                if (white == computer) undoMove();
            }
        }
    }
}
