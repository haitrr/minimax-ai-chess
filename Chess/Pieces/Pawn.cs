using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Chess
{
    class Pawn:Piece
    {
        private static int[,] pointMaxtrix = new int[8, 8] {
         {0,  0,  0,  0,  0,  0,  0,  0},
         {50, 50, 50, 50, 50, 50, 50, 50},
         {10, 10, 20, 30, 30, 20, 10, 10},
         {5,  5, 10, 25, 25, 10,  5,  5},
         {0,  0,  0, 20, 20,  0,  0,  0},
         {5, -5,-10,  0,  0,-10, -5,  5},
         {5, 10, 10,-20,-20, 10, 10,  5},
         {0,  0,  0,  0,  0,  0,  0,  0}
        };
        private static int[,] valueMaxtrix = new int[8, 8]{
{100,100,100,100,100,100,100,100},
{150,150,150,150,150,150,150,150},
{110,110,120,130,130,120,110,110},
{105,105,110,125,125,110,105,105},
{100,100,100,120,120,100,100,100},
{105,95,90,100,100,90,95,105},
{105,110,110,80,80,110,110,105},
{100,100,100,100,100,100,100,100}};
        public override int getPoint(Color c, Point p)
        {
            if (c == Color.White) return 100 + pointMaxtrix[p.X, p.Y];
            else return 100 + pointMaxtrix[7 - p.X, p.Y];
        }
        public Pawn(Color c):base(c)
        {
            if (c == Color.Black) evalueMaxtrix = mirrorMaxtrix(valueMaxtrix);
            else evalueMaxtrix = valueMaxtrix;
        }
        public override List<Move> getAvailableMove(Point source)
        {
            List<Move> availableMove;
            availableMove = new List<Move>();
            if (color == Color.Black)
            {
                if(source.Y<7) availableMove.Add(new Move(source,new Point(source.X + 1, source.Y + 1)));
                if (source.Y > 0) availableMove.Add(new Move(source, new Point(source.X + 1, source.Y - 1)));
                availableMove.Add(new Move(source,new Point(source.X + 1, source.Y)));
                if (source.X == 1) availableMove.Add(new Move(source,new Point(source.X + 2, source.Y)));
            }
            else
            {
                if (source.Y < 7) availableMove.Add(new Move(source, new Point(source.X - 1, source.Y + 1)));
                if (source.Y > 0) availableMove.Add(new Move(source, new Point(source.X - 1, source.Y - 1)));
                availableMove.Add(new Move(source,new Point(source.X - 1, source.Y)));
                if (source.X == 6) availableMove.Add(new Move(source,new Point(source.X - 2, source.Y)));
            }
            return availableMove;
        }
        public override List<Point> getMovePath(Point source, Point dest)
        {
            List<Point> availableMove, rs;
            rs = new List<Point>();
            availableMove = new List<Point>();
            if (color == Color.Black)
            {
                availableMove.Add(new Point(source.X + 1, source.Y + 1));
                availableMove.Add(new Point(source.X + 1, source.Y - 1));
                availableMove.Add(new Point(source.X +1, source.Y ));
                if (source.X == 1) availableMove.Add(new Point(source.X + 2, source.Y));
            }
            else
            {
                availableMove.Add(new Point(source.X - 1, source.Y +1));
                availableMove.Add(new Point(source.X - 1, source.Y - 1));
                availableMove.Add(new Point(source.X - 1, source.Y ));
                if (source.X == 6) availableMove.Add(new Point(source.X - 2, source.Y));
            }
            
            for (int i = 0; i < availableMove.Count; i++)
                if (availableMove[i] == dest)
                {
                    if (availableMove[i].X == source.X + 2) rs.Add(new Point(source.X + 1, source.Y));
                    if (availableMove[i].X == source.X - 2) rs.Add(new Point(source.X - 1, source.Y));
                    rs.Add(availableMove[i]);
                    return rs;
                }
            return null;
        }
        public override string getChar()
        {
            if (color == Color.Black) return "p";
            return "P";
        }
        public override bool isPawn()
        {
            return true;
        }
    }
}
