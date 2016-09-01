using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace Chess
{
    class Box
    {
        public static int boxHeight { get; set; }
        public static int boxWidth { get; set; }
        public Piece piece { get; set; }
        public Color color { get; private set; }
        public bool isJustMove {get;set;}
        public static Color justMoveColor;
        public static Color availableMoveColor;
        public override bool Equals(object obj)
        {
            Box other = obj as Box;
            if(piece==null)
            {
                if (other.piece == null) return true;
                else return false;
            }
            if (other.piece == null)
            {
                if (piece == null) return true;
                else return false;
            }
            if (piece.Equals(other.piece)) return true;
            return false;
        }
        public override int GetHashCode()
        {
            int has;
            if (piece == null) return 0;
            has = piece.GetHashCode() * 15;
            return has;
        }
        public Box(Color c)
        {
            color = c;
            piece = null;
        }
        public Box(Color c,Piece p)
        {
            color = c;
            piece = p;
        }
        public void drawAvailableMove(Graphics g,int x,int y)
        {
            g.FillRectangle(new SolidBrush(blendColor(availableMoveColor, color, 0.5)), y * boxWidth, x * boxHeight, boxHeight, boxWidth);
            if (piece != null) piece.Dislay(g, y * boxWidth+boxWidth/7, x * boxHeight+boxWidth/7);
        }
        public Box()
        {
            isJustMove = false;
        }
        public static void init()
        {
            justMoveColor = Color.Blue;
            availableMoveColor = Color.Green;
        }
        public void  dislay(Graphics g,int x,int y)
        {
            if (isJustMove) g.FillRectangle(new SolidBrush(blendColor(justMoveColor,color,0.2)), y * boxWidth, x * boxHeight, boxHeight, boxWidth);
            else
            g.FillRectangle(new SolidBrush(color), y * boxWidth, x * boxHeight, boxHeight, boxWidth);
            if (piece != null) piece.Dislay(g, y * boxWidth+boxWidth/7, x * boxHeight+boxWidth/7);
        }
        public void drawSelect(Graphics g,int x,int y)
        {
            g.DrawImage(new Bitmap(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "selecting.png")), y * boxWidth, x * boxHeight);
        }
        public bool isContainingPiece()
        {
            if (piece != null) return true;
            return false;
        }
        public Box Clone()
        {
            Box copy = new Box(color);
            if (piece != null)
            {
                copy.piece = piece.Clone();
            }
            return copy;
        }
        public static Color blendColor(Color color, Color backColor, double amount)
        {
            byte r = (byte)((color.R * amount) + backColor.R * (1 - amount));
            byte g = (byte)((color.G * amount) + backColor.G * (1 - amount));
            byte b = (byte)((color.B * amount) + backColor.B * (1 - amount));
            return Color.FromArgb(r, g, b);
        }
    }
}
