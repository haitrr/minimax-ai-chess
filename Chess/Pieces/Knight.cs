using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Chess
{
    class Knight:Piece
    {
        private static int[,] pointMaxtrix = new int[8, 8] {
         {-50,-40,-30,-30,-30,-30,-40,-50},
         {-40,-20,  0,  0,  0,  0,-20,-40},
         {-30,  0, 10, 15, 15, 10,  0,-30},
         {-30,  5, 15, 20, 20, 15,  5,-30},
         {-30,  0, 15, 20, 20, 15,  0,-30},
         {-30,  5, 10, 15, 15, 10,  5,-30},
         {-40,-20,  0,  5,  5,  0,-20,-40},
         {-50,-40,-30,-30,-30,-30,-40,-50}
        };
        private static int[,] valueMaxtrix = new int[8, 8]{
{270,280,290,290,290,290,280,270},
{280,300,320,320,320,320,300,280},
{290,320,330,335,335,330,320,290},
{290,325,335,340,340,335,325,290},
{290,320,335,340,340,335,320,290},
{290,325,330,335,335,330,325,290},
{280,300,320,325,325,320,300,280},
{270,280,290,290,290,290,280,270}};
        public override int getPoint(Color c, Point p)
        {
            if (c == Color.White) return 320 + pointMaxtrix[p.X, p.Y];
            else return 320 + pointMaxtrix[7 - p.X, p.Y];
        }
        public Knight(Color c):base(c)
        {
            if (c == Color.Black) evalueMaxtrix = mirrorMaxtrix(valueMaxtrix);
            else evalueMaxtrix = valueMaxtrix;
        }
        public override string getChar()
        {
            if (color == Color.Black) return "n";
            return "N";
        }
        public override List<Move> getAvailableMove(Point source)
        {
            List<Move> availableMove,temp;
            availableMove = new List<Move>();
            temp = new List<Move>();
            temp.Add(new Move(source, new Point(source.X + 1, source.Y+2)));
            temp.Add(new Move(source, new Point(source.X - 1, source.Y-2)));
            temp.Add(new Move(source, new Point(source.X + 1, source.Y - 2)));
            temp.Add(new Move(source, new Point(source.X - 1, source.Y + 2)));
            temp.Add(new Move(source, new Point(source.X + 2, source.Y + 1)));
            temp.Add(new Move(source, new Point(source.X - 2, source.Y + 1)));
            temp.Add(new Move(source, new Point(source.X + 2, source.Y - 1)));
            temp.Add(new Move(source, new Point(source.X - 2, source.Y - 1)));
            foreach(Move move in temp)
            {
                if (move.dest.X >= 0 && move.dest.X < 8 && move.dest.Y >= 0 && move.dest.Y < 8) availableMove.Add(move);
            }
            return availableMove;
        }
        public override List<Point> getMovePath(Point source,Point dest)
        {
            List<Point> availableMove,rs;
            rs = new List<Point>();
            availableMove = new List<Point>();
            availableMove.Add(new Point(source.X + 1, source.Y+2));
            availableMove.Add(new Point(source.X - 1, source.Y-2));
            availableMove.Add(new Point(source.X + 1, source.Y-2));
            availableMove.Add(new Point(source.X - 1, source.Y+2));
            availableMove.Add(new Point(source.X + 2, source.Y +1));
            availableMove.Add(new Point(source.X - 2, source.Y +1));
            availableMove.Add(new Point(source.X + 2, source.Y -1));
            availableMove.Add(new Point(source.X - 2, source.Y -1));
            for (int i = 0; i < availableMove.Count; i++)
                if (availableMove[i] == dest)
                {
                    rs.Add(availableMove[i]);
                    return rs;
                }
            return null;
        }
    }
}
