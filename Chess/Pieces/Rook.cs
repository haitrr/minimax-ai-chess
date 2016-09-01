using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Chess
{
    class Rook:Piece
    {
        private static int[,] pointMaxtrix = new int[8, 8] {
         {0,  0,  0,  0,  0,  0,  0,  0},
         {5, 10, 10, 10, 10, 10, 10,  5},
         {-5,  0,  0,  0,  0,  0,  0, -5},
         {-5,  0,  0,  0,  0,  0,  0, -5},
         {-5,  0,  0,  0,  0,  0,  0, -5},
         {-5,  0,  0,  0,  0,  0,  0, -5},
         {-5,  0,  0,  0,  0,  0,  0, -5},
         {0,  0,  0,  5,  5,  0,  0,  0}
        };
        private static int[,] valueMaxtrix = new int[8, 8]{
{500,500,500,500,500,500,500,500},
{505,510,510,510,510,510,510,505},
{495,500,500,500,500,500,500,495},
{495,500,500,500,500,500,500,495},
{495,500,500,500,500,500,500,495},
{495,500,500,500,500,500,500,495},
{495,500,500,500,500,500,500,495},
{500,500,500,505,505,500,500,500}};
        public override string getChar()
        {
            if (color == Color.Black) return "r";
            return "R";
        }
        public Rook(Color c):base(c)
        {
            if (c == Color.Black) evalueMaxtrix = mirrorMaxtrix(valueMaxtrix);
            else evalueMaxtrix = valueMaxtrix;
        }
        public override int getPoint(Color c,Point p)
        {
            if (c == Color.White) return 500 + pointMaxtrix[p.X, p.Y];
            else return 500 + pointMaxtrix[7 - p.X, p.Y];
        }
        public override List<Move> getAvailableMove(Point source)
        {
            List<Move> rs = new List<Move>();
            foreach (Point p in getAvailableMovePathRight(source))
            {
                rs.Add(new Move(source, p));
            }
            foreach (Point p in getAvailableMovePathLeft(source))
            {
                rs.Add(new Move(source, p));
            }
            foreach (Point p in getAvailableMovePathUp(source))
            {
                rs.Add(new Move(source, p));
            }
            foreach (Point p in getAvailableMovePathDown(source))
            {
                rs.Add(new Move(source, p));
            }
            return rs;
        }
        public override List<Point> getMovePath(Point source, Point dest)
        {
            if (source.X > dest.X)
            {
                if(source.Y==dest.Y) return getMovePathDown(source, dest);
            }
            if (source.X < dest.X)
            {
                if (source.Y == dest.Y) return getMovePathUp(source, dest);
            }
            if (source.Y > dest.Y) return getMovePathLeft(source, dest);
            if (source.Y < dest.Y) return getMovePathRight(source, dest);
            return null;
        }
    }
}
