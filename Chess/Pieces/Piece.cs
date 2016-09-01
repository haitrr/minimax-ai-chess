using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Chess
{
     abstract class Piece
    {
        public Color color { get;protected set;}
        abstract public int getPoint(Color color,Point pos);
        public abstract List<Point> getMovePath(Point source, Point dest);
        public abstract List<Move> getAvailableMove(Point source);
        public int[,] evalueMaxtrix;
        public bool isMoved { get; set; }
        public override bool Equals(object obj)
        {
            Piece other = obj as Piece;
            if(other.GetType()==GetType())
            {
                if (other.color.Equals(color)) return true;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return color.GetHashCode() + GetType().GetHashCode();

        }
        public abstract string getChar();
        public Piece()
        {
            
        }
        public Piece(Color c)
        {
            color = c;
            isMoved = false;
        }
        public void Dislay(Graphics g,int x,int y)
        {
            Image cicon;
            cicon = new Bitmap(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "PiecesIcons/"+GetType().Name+"_"+color.Name+".png"));
            g.DrawImage(cicon, x, y);
        }
        public List<Point> getMovePathUp(Point source, Point dest)
        {
            return getPath(getAvailableMovePathUp(source), dest);
        }
        public List<Point> getMovePathDown(Point source, Point dest)
        {
            return getPath(getAvailableMovePathDown(source), dest);
        }
        public List<Point> getMovePathRight(Point source, Point dest)
        {
            return getPath(getAvailableMovePathRight(source), dest);
        }
        public List<Point> getMovePathLeft(Point source, Point dest)
        {
            return getPath(getAvailableMovePathLeft(source), dest);
        }
        public List<Point> getMovePathCrossUpRight(Point source, Point dest)
        {
            return getPath(getAvailableMovePathCrossUpRight(source), dest);
        }
        public List<Point> getMovePathCrossUpLeft(Point source, Point dest)
        {
            return getPath(getAvailableMovePathCrossUpLeft(source), dest);
        }
        public List<Point> getMovePathCrossDownRight(Point source, Point dest)
        {
            return getPath(getAvailableMovePathCrossDownRight(source), dest);
        }
        public List<Point> getMovePathCrossDownLeft(Point source, Point dest)
        {
            return getPath(getAvailableMovePathCrossDownLeft(source), dest);
        }

        public List<Point> getPath(List<Point> availableMove, Point dest)
        {
            List<Point> rs;
            rs = new List<Point>();
            for (int i = 0; i < availableMove.Count; i++)
            {
                if (availableMove[i] == dest)
                {
                    for (int j = 0; j <= i; j++) rs.Add(availableMove[j]);
                    return rs;
                }
            }

            return null;
        }
        public List<Point> getAvailableMovePathUp(Point source)
        {
            List<Point> availableMove, rs;
            rs = new List<Point>();
            availableMove = new List<Point>();
            int x = source.X, y = source.Y;
            x++;
            while (x < 8)
            {
                availableMove.Add(new Point(x, y));
                x++;
            }
            return availableMove;
        }
        public List<Point> getAvailableMovePathDown(Point source)
        {
            List<Point> availableMove, rs;
            rs = new List<Point>();
            availableMove = new List<Point>();
            int x = source.X, y = source.Y;
            x--;
            while (x >= 0)
            {
                availableMove.Add(new Point(x, y));
                x--;
            }
            return availableMove;
        }
        public List<Point> getAvailableMovePathRight(Point source)
        {
            List<Point> availableMove, rs;
            rs = new List<Point>();
            availableMove = new List<Point>();
            int x = source.X, y = source.Y;
            y++;
            while (y < 8)
            {
                availableMove.Add(new Point(x, y));
                y++;
            }
            return availableMove;
        }
        public List<Point> getAvailableMovePathLeft(Point source)
        {
            List<Point> availableMove, rs;
            rs = new List<Point>();
            availableMove = new List<Point>();
            int x = source.X, y = source.Y;
            y--;
            while (y >= 0)
            {
                availableMove.Add(new Point(x, y));
                y--;
            }
            return availableMove;
        }
        public List<Point> getAvailableMovePathCrossUpRight(Point source)
        {
            List<Point> availableMove, rs;
            rs = new List<Point>();
            availableMove = new List<Point>();
            int x = source.X, y = source.Y;
            x++;
            y++;
            while (x < 8 && y < 8)
            {
                availableMove.Add(new Point(x, y));
                x++;
                y++;
            }
            return availableMove;
        }
        public List<Point> getAvailableMovePathCrossUpLeft(Point source)
        {
            List<Point> availableMove, rs;
            rs = new List<Point>();
            availableMove = new List<Point>();
            int x = source.X, y = source.Y;
            x++;
            y--;
            while (x < 8 && y >= 0)
            {
                availableMove.Add(new Point(x, y));
                x++;
                y--;
            }
            return availableMove;
        }
        public List<Point> getAvailableMovePathCrossDownRight(Point source)
        {
            List<Point> availableMove, rs;
            rs = new List<Point>();
            availableMove = new List<Point>();
            int x = source.X, y = source.Y;
            x--;
            y++;
            while (y < 8 && x >= 0)
            {
                availableMove.Add(new Point(x, y));
                x--;
                y++;
            }
            return availableMove;
        }
        public List<Point> getAvailableMovePathCrossDownLeft(Point source)
        {
            List<Point> availableMove, rs;
            rs = new List<Point>();
            availableMove = new List<Point>();
            int x = source.X, y = source.Y;
            x--;
            y--;
            while (x >= 0 && y >= 0)
            {
                availableMove.Add(new Point(x, y));
                x--;
                y--;
            }
            return availableMove;
        }
        public virtual bool isPawn()
        {
            return false;
        }
        public virtual bool isKing()
        {
            return false;
        }
         public Piece Clone()
        {
            Piece clone = (Piece)MemberwiseClone();
            return clone;
        }
         public int[,] mirrorMaxtrix(int[,] maxtrix)
         {
             int [,] mirror=new int[8,8];
             for(int i=0;i<8;i++)
                 for(int j=0;j<8;j++)
                 {
                     mirror[i,j]=maxtrix[7-i,j];
                 }
             return mirror;
         }
    }
}
