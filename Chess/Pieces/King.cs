using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Chess
{
    class King:Piece
    {
        private static int[,] pointMaxtrix = new int[8, 8] {
         {-30,-40,-40,-50,-50,-40,-40,-30},
         {-30,-40,-40,-50,-50,-40,-40,-30},
         {-30,-40,-40,-50,-50,-40,-40,-30},
         {-30,-40,-40,-50,-50,-40,-40,-30},
         {-20,-30,-30,-40,-40,-30,-30,-20},
         {-10,-20,-20,-20,-20,-20,-20,-10},
         {20, 20,  0,  0,  0,  0, 20, 20},
         {20, 30, 10,  0,  0, 10, 30, 20}
        };
        private static int[,] valueMaxtrix = new int[8, 8]{
{19970,19960,19960,19950,19950,19960,19960,19970},
{19970,19960,19960,19950,19950,19960,19960,19970},
{19970,19960,19960,19950,19950,19960,19960,19970},
{19970,19960,19960,19950,19950,19960,19960,19970},
{19980,19970,19970,19960,19960,19970,19970,19980},
{19990,19980,19980,19980,19980,19980,19980,19990},
{20020,20020,20000,20000,20000,20000,20020,20020},
{20020,20030,20010,20000,20000,20010,20030,20020}};
        public King(Color c):base(c)
        {

            if (c == Color.Black) evalueMaxtrix = mirrorMaxtrix(valueMaxtrix);
            else evalueMaxtrix = valueMaxtrix;
        }
        public override string getChar()
        {
            if (color == Color.Black) return "k";
            return "K";
        }
        public override int getPoint(Color c, Point p)
        {
            if (c == Color.White) return 20000 + pointMaxtrix[p.X, p.Y];
            else return 20000 + pointMaxtrix[7 - p.X, p.Y];
        }
        public override List<Move> getAvailableMove(Point source)
        {
            List<Move> availableMove,temp;
            availableMove = new List<Move>();
            temp=new List<Move>();
            temp.Add(new Move(source,new Point(source.X + 1, source.Y)));
            temp.Add(new Move(source,new Point(source.X - 1, source.Y)));
            temp.Add(new Move(source,new Point(source.X + 1, source.Y + 1)));
            temp.Add(new Move(source,new Point(source.X - 1, source.Y + 1)));
            temp.Add(new Move(source,new Point(source.X, source.Y + 1)));
            temp.Add(new Move(source,new Point(source.X, source.Y - 1)));
            temp.Add(new Move(source,new Point(source.X + 1, source.Y - 1)));
            temp.Add(new Move(source,new Point(source.X - 1, source.Y - 1)));
            //if(isMoved==false)
            //{
            //    availableMove.Add(new Move(source,new Point(source.X , source.Y+2)));
            //    availableMove.Add(new Move(source, new Point(source.X, source.Y - 2)));
            //}
            foreach (Move move in temp)
            {
                if (move.dest.X >= 0 && move.dest.X < 8 && move.dest.Y >= 0 && move.dest.Y < 8) availableMove.Add(move);
            }
            return availableMove;
        }
        public override List<Point> getMovePath(Point source, Point dest)
        {
            List<Point> availableMove, rs;
            rs = new List<Point>();
            availableMove = new List<Point>();
            availableMove.Add(new Point(source.X + 1, source.Y ));
            availableMove.Add(new Point(source.X - 1, source.Y));
            availableMove.Add(new Point(source.X + 1, source.Y +1));
            availableMove.Add(new Point(source.X - 1, source.Y + 1));
            availableMove.Add(new Point(source.X , source.Y + 1));
            availableMove.Add(new Point(source.X , source.Y - 1));
            availableMove.Add(new Point(source.X + 1, source.Y - 1));
            availableMove.Add(new Point(source.X - 1, source.Y - 1));
            if (isMoved == false)
            {
                availableMove.Add(new Point(source.X, source.Y + 2));
                availableMove.Add(new Point(source.X, source.Y - 2));
            }
            for (int i = 0; i < availableMove.Count; i++)
                if (availableMove[i] == dest)
                {
                    if(availableMove[i].Y-source.Y==2)
                    {
                        rs.Add(new Point(source.X,source.Y+1));
                    }
                    if (availableMove[i].Y - source.Y == -2)
                    {
                        rs.Add(new Point(source.X, source.Y - 1));
                    }
                    rs.Add(availableMove[i]);
                    return rs;
                }
            return null;
        }
        public override bool isKing()
        {
            return true;
        }
    }
}
