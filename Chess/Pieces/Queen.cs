using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Chess
{
    class Queen:Piece
    {
        private static int[,] pointMaxtrix = new int[8, 8] {
         {-20,-10,-10, -5, -5,-10,-10,-20},
         {-10,  0,  0,  0,  0,  0,  0,-10},
         {-10,  0,  5,  5,  5,  5,  0,-10},
         {-5,  0,  5,  5,  5,  5,  0, -5},
         {0,  0,  5,  5,  5,  5,  0, -5},
         {-10,  5,  5,  5,  5,  5,  0,-10},
         {-10,  0,  5,  0,  0,  0,  0,-10},
         {-20,-10,-10, -5, -5,-10,-10,-20}
        };
        private static int[,] valueMaxtrix = new int[8, 8]{
{880,890,890,895,895,890,890,880},
{890,900,900,900,900,900,900,890},
{890,900,905,905,905,905,900,890},
{895,900,905,905,905,905,900,895},
{900,900,905,905,905,905,900,895},
{890,905,905,905,905,905,900,890},
{890,900,905,900,900,900,900,890},
{880,890,890,895,895,890,890,880}};
        public override string getChar()
        {
            if (color == Color.Black) return "q";
            return "Q";
        }
        public Queen(Color c):base(c)
        {
            if (c == Color.Black) evalueMaxtrix = mirrorMaxtrix(valueMaxtrix);
            else evalueMaxtrix = valueMaxtrix;
        }
        public override int getPoint(Color c, Point p)
        {
            if (c == Color.White) return 900 + pointMaxtrix[p.X, p.Y];
            else return 900 + pointMaxtrix[7 - p.X, p.Y];
        }
        public override List<Move> getAvailableMove(Point source)
        {
            List<Move> rs = new List<Move>();
            foreach (Point p in getAvailableMovePathCrossDownLeft(source))
            {
                rs.Add(new Move(source, p));
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
            if(source.X>dest.X)
            {
                if (source.Y > dest.Y) return getMovePathCrossDownLeft(source, dest);
                if (source.Y == dest.Y) return getMovePathDown(source, dest);
                return getMovePathCrossDownRight(source, dest);
            }
            if(source.X<dest.X)
            {
                if (source.Y > dest.Y) return getMovePathCrossUpLeft(source, dest);
                if (source.Y == dest.Y) return getMovePathUp(source, dest);
                return getMovePathCrossUpRight(source, dest);
            }
            if (source.Y > dest.Y) return getMovePathLeft(source, dest);
            if (source.Y < dest.Y) return getMovePathRight(source, dest);
            return null;
        }
    }
}
