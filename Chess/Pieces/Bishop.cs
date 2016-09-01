using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Chess
{
    class Bishop:Piece
    {
        private static int[,] pointMaxtrix = new int[8, 8] {
         {-20,-10,-10,-10,-10,-10,-10,-20},
         {-10,  0,  0,  0,  0,  0,  0,-10},
         {-10,  0,  5, 10, 10,  5,  0,-10},
         {-10,  5,  5, 10, 10,  5,  5,-10},
         { -10,  0, 10, 10, 10, 10,  0,-10},
         {-10, 10, 10, 10, 10, 10, 10,-10},
         {-10,  5,  0,  0,  0,  0,  5,-10},
         {-20,-10,-10,-10,-10,-10,-10,-20}
        };
        private static int[,] valueMaxtrix = new int[8, 8]{
{310,320,320,320,320,320,320,310},
{320,330,330,330,330,330,330,320},
{320,330,335,340,340,335,330,320},
{320,335,335,340,340,335,335,320},
{320,330,340,340,340,340,330,320},
{320,340,340,340,340,340,340,320},
{320,335,330,330,330,330,335,320},
{310,320,320,320,320,320,320,310}};
        public Bishop(Color c):base(c)
        {
            if (c == Color.Black) evalueMaxtrix = mirrorMaxtrix(valueMaxtrix);
            else
            evalueMaxtrix = valueMaxtrix;
        }
        public override int getPoint(Color c, Point p)
        {
            if (c == Color.White) return 330 + pointMaxtrix[p.X, p.Y];
            else return 330 + pointMaxtrix[7 - p.X, p.Y];
        }
        public override List<Move> getAvailableMove(Point source)
        {
            List<Move> rs = new List<Move>();
            foreach(Point p in getAvailableMovePathCrossDownLeft(source))
            {
                rs.Add(new Move(source,p));
            }
            foreach (Point p in getAvailableMovePathCrossDownRight(source))
            {
                rs.Add(new Move(source, p));
            }
            foreach (Point p in getAvailableMovePathCrossUpLeft(source))
            {
                rs.Add(new Move(source, p));
            }
            foreach (Point p in getAvailableMovePathCrossUpRight(source))
            {
                rs.Add(new Move(source, p));
            }
            return rs;
        }
        public override List<Point> getMovePath(Point source, Point dest)
        {
            if (source.X > dest.X)
            {
                if (source.Y > dest.Y) return getMovePathCrossDownLeft(source, dest);
                if (source.Y < dest.Y) return getMovePathCrossDownRight(source, dest); ;
                return null;
            }
            else
            if (source.X < dest.X)
            {
                if (source.Y > dest.Y) return getMovePathCrossUpLeft(source, dest);
                if (source.Y < dest.Y) return getMovePathCrossUpRight(source, dest);
                return null;
            }
            return null;
        }
        public override string getChar()
        {
            if (color == Color.Black) return "b";
            return "B";
        }
    }
}
