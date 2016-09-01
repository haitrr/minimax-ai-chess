using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Chess
{
    class ChessBoard
    {
        public Tracer tracer { get; private set; }
        public Point selecting{get;set;}
        private int boardHeight { get; set; }
        private int boardWidth { get; set; }
        public Box[,] boxes { get; private set; }
        public bool gameOver{get;private set;}
        public Color checking { get; private set; }
        public Color winner {get;private set;}
        public override bool Equals(object obj)
        {
            ChessBoard other=obj as ChessBoard;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (!boxes[i, j].Equals(other.boxes[i, j])) return false;
                }
            }
            return true;
        }
        public override int GetHashCode()
        {
            int has = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    has += boxes[i, j].GetHashCode();
                }
            }
            return has;
        }
        public Color getPieceColor(Point p)
        {
            return boxes[p.X, p.Y].piece.color;
        }
        public ChessBoard(int height, int width)
        {
            boardHeight = height;
            boardWidth = width;
            init();
        }
        public ChessBoard()
        {

        }
        public void init()
        {
            tracer = new Tracer();
            boxes = new Box[8, 8];
            initBoxes(Color.MintCream, Color.Brown);
            selecting = null;
        }
        public void initBoxes(Color light, Color dark)
        {
            //Intitial Box
            Box.boxHeight = boardHeight / 8;
            Box.boxWidth = boardWidth / 8;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (i % 2 == 0)
                    {
                        if (j % 2 == 0) boxes[i, j] = new Box(light);
                        else boxes[i, j] = new Box(dark);
                    }
                    else
                    {
                        if (j % 2 == 0) boxes[i, j] = new Box(dark);
                        else boxes[i, j] = new Box(light);
                    }
                }
            }
            //Add chess Piece
            //Black
            boxes[0, 0].piece = new Rook(Color.Black);
            boxes[0, 1].piece = new Knight(Color.Black);
            boxes[0, 2].piece = new Bishop(Color.Black);
            boxes[0, 3].piece = new Queen(Color.Black);
            boxes[0, 4].piece = new King(Color.Black);
            boxes[0, 5].piece = new Bishop(Color.Black);
            boxes[0, 6].piece = new Knight(Color.Black);
            boxes[0, 7].piece = new Rook(Color.Black);
            for (int i = 0; i < 8; i++)
            {
                boxes[1, i].piece = new Pawn(Color.Black);
            }
            //White
            boxes[7, 0].piece = new Rook(Color.White);
            boxes[7, 1].piece = new Knight(Color.White);
            boxes[7, 2].piece = new Bishop(Color.White);
            boxes[7, 3].piece = new Queen(Color.White);
            boxes[7, 4].piece = new King(Color.White);
            boxes[7, 5].piece = new Bishop(Color.White);
            boxes[7, 6].piece = new Knight(Color.White);
            boxes[7, 7].piece = new Rook(Color.White);
            for (int i = 0; i < 8; i++)
            {
                boxes[6, i].piece = new Pawn(Color.White);
            }
        }
        public bool movePiece(Point source, Point dest, Graphics g,bool needCheckMove)
        {
            if(g!=null) boxes[source.X, source.Y].dislay(g, source.X, source.Y);
            if(needCheckMove && checkMove(source, dest) == false) return false;
            if (gameOver) return false;
            if (boxes[source.X, source.Y].piece.isKing() && Math.Abs(source.Y - dest.Y) > 1)
            {
                if (dest.Y == 6) movePiece(new Point(source.X, 7), new Point(source.X, 5), g,false);
                else movePiece(new Point(source.X, 0), new Point(source.X, 3), g,false);
            }
            if (tracer.isHasValidTrace)
            {
                Move previousMove = tracer.previousMove();
                boxes[previousMove.dest.X, previousMove.dest.Y].isJustMove = false;
                boxes[previousMove.source.X, previousMove.source.Y].isJustMove = false;
                if (g != null)
                {
                    boxes[previousMove.dest.X, previousMove.dest.Y].dislay(g, previousMove.dest.X, previousMove.dest.Y);
                    boxes[previousMove.source.X, previousMove.source.Y].dislay(g, previousMove.source.X, previousMove.source.Y);
                }
            }
            boxes[dest.X, dest.Y].isJustMove = true;
            boxes[source.X, source.Y].isJustMove = true;
            tracer.AddMove(new Move(source, dest), boxes[source.X, source.Y].piece.Clone(), boxes[dest.X, dest.Y].piece==null?null:boxes[dest.X, dest.Y].piece.Clone());
            if (boxes[source.X, source.Y].piece.isKing() && Math.Abs(source.Y - dest.Y) > 1)
            {
                tracer.castlingFlag.Pop();
                tracer.castlingFlag.Push(true);
            }
            boxes[source.X, source.Y].piece.isMoved = true;
            if(boxes[dest.X, dest.Y].isContainingPiece() && boxes[dest.X,dest.Y].piece.isKing())
            {
                gameOver = true;
                if (boxes[dest.X, dest.Y].piece.color == Color.Black) winner = Color.White;
                else winner = Color.Black;
            }
            boxes[dest.X, dest.Y].piece = boxes[source.X, source.Y].piece;
            boxes[source.X, source.Y].piece = null;
            if (g != null) boxes[source.X, source.Y].dislay(g, source.X, source.Y);
            if(boxes[dest.X, dest.Y].piece.isPawn() )
            {
                if (dest.X == 7 && boxes[dest.X, dest.Y].piece.color == Color.Black) promotion(dest);
                else if(dest.X == 0 && boxes[dest.X, dest.Y].piece.color == Color.White) promotion(dest);
            }
            if (g != null) boxes[dest.X, dest.Y].dislay(g, dest.X, dest.Y);

            return true;
        }
        public bool undoMove(Graphics g)
        {
            if (tracer.isHasValidTrace == false) return false;
            Move move = tracer.UndoMove();
            boxes[move.dest.X, move.dest.Y].isJustMove = false;
            boxes[move.source.X, move.source.Y].isJustMove = false;
            if (g != null) boxes[move.source.X, move.source.Y].dislay(g, move.source.X, move.source.Y);
            boxes[move.dest.X, move.dest.Y].piece = tracer.undoDestPiece();
            boxes[move.source.X, move.source.Y].piece = tracer.undoSourcePiece();
            if (g != null) boxes[move.source.X, move.source.Y].dislay(g, move.source.X, move.source.Y);
            if (g != null) boxes[move.dest.X, move.dest.Y].dislay(g, move.dest.X, move.dest.Y);
            if (gameOver) gameOver = false;
            if (tracer.castlingFlag.Pop() == true) 
                undoMove(g);
            return true;
        }
        public bool checkMove(Point source, Point dest)
        {
            //if (boxes[source.X, source.Y].isContainingPiece() == false) return false;
            if (boxes[dest.X, dest.Y].isContainingPiece() && boxes[source.X, source.Y].piece.color == boxes[dest.X, dest.Y].piece.color) return false;
            List<Point> movePath = boxes[source.X, source.Y].piece.getMovePath(source, dest);
            if (movePath == null) return false;
            else
            {
                for (int i = 0; i < movePath.Count-1; i++)
                {
                    if (boxes[movePath[i].X, movePath[i].Y].isContainingPiece()) return false;
                }
            }
            if (boxes[source.X, source.Y].piece.isPawn())
            {
                if (source.Y != dest.Y && (!boxes[dest.X, dest.Y].isContainingPiece())) return false;

                if (source.Y==dest.Y &&boxes[dest.X, dest.Y].isContainingPiece()) return false;
            }
            return true;
        }
        public List<Move> getAvailableMove(Color color,bool castling) 
        {
            List<Move> resuilt = new List<Move>();
            List<Move> temp;
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    if (boxes[i, j].isContainingPiece() && boxes[i, j].piece.color == color)
                    {
                        temp = boxes[i, j].piece.getAvailableMove(new Point(i, j));
                        foreach (Move move in temp)
                        {
                            if (checkMove(move.source, move.dest)) resuilt.Add(move);
                        }
                        if(boxes[i,j].piece.isKing()&&boxes[i,j].piece.isMoved==false&&castling)
                        {
                            List<Move> tempMoves=castlingMove(new Point(i,j));
                            foreach(Move move in tempMoves )
                            {
                                move.castling = true;
                                resuilt.Add(move);
                            }
                        }
                    }
                }
            return resuilt;
        }
        public void promotion(Point p)
        {
            boxes[p.X, p.Y].piece = new Queen(boxes[p.X, p.Y].piece.color);
        }
        public void removePiece(int sourceX, int sourceY)
        {

        }
        public void drawBoard(Graphics g)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    boxes[i, j].dislay(g, i, j);
                }
            }
        }
        public void drawSelect(Graphics g)
        {
            if ((object)selecting != null)
            {
                boxes[selecting.X, selecting.Y].drawSelect(g, selecting.X, selecting.Y);
                List<Move> temp =boxes[selecting.X, selecting.Y].piece.getAvailableMove(selecting);
                foreach (Move move in temp)
                {
                    if (checkMove(move.source, move.dest)) boxes[move.dest.X, move.dest.Y].drawAvailableMove(g, move.dest.X, move.dest.Y);
                }
                if (boxes[selecting.X, selecting.Y].piece.isKing() && boxes[selecting.X, selecting.Y].piece.isMoved == false)
                {
                    List<Move> tempMoves = castlingMove(selecting);
                    foreach (Move move in tempMoves)
                    {
                        boxes[move.dest.X, move.dest.Y].drawAvailableMove(g, move.dest.X, move.dest.Y);
                    }
                }
            }
        }
        public bool isContainingPiece(Point p)
        {
            if (boxes[p.X, p.Y].piece != null) return true;
            return false;
        }
        public bool isSelecting()
        {
            if ((object)selecting == null) return false;
            return true;
        }
        public void unSelect(Graphics g,bool isMoved)
        {
            if (g != null)
            {
                if (isMoved)
                {
                    Move previousMove = tracer.previousMove();
                    undoMove(null);
                    List<Move> temp = boxes[selecting.X, selecting.Y].piece.getAvailableMove(selecting);
                    List<Move> availableMove = new List<Move>();
                    foreach (Move move in temp)
                    {
                        if (checkMove(move.source, move.dest)) availableMove.Add(move);
                    }
                    movePiece(previousMove.source, previousMove.dest, null,false);
                    foreach (Move move in availableMove)
                    {
                        boxes[move.dest.X, move.dest.Y].dislay(g, move.dest.X, move.dest.Y);
                    }
                }
                else
                {
                    List<Move> temp = boxes[selecting.X, selecting.Y].piece.getAvailableMove(selecting);
                    foreach (Move move in temp)
                    {
                        if (checkMove(move.source, move.dest)) boxes[move.dest.X, move.dest.Y].dislay(g, move.dest.X, move.dest.Y);
                    }
                }
            }
            selecting = null;
        }
        public ChessBoard Clone()
        {
            ChessBoard copy;
            copy = (ChessBoard)this.MemberwiseClone();
            copy.tracer = new Tracer();
            copy.boxes = new Box[8,8];
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8;j++ )
                {
                    copy.boxes[i, j] = boxes[i, j].Clone();
                }
            return copy;
        }
        public List<Move> castlingMove(Point p)
        {
            List<Move> rs = new List<Move>();
            //if(checking==boxes[p.X,p.Y].color) return rs;
            List<Point> rooks= new List<Point>();
            rooks.Add(new Point (p.X, 0));
            rooks.Add(new Point(p.X, 7));
            foreach(Point r in rooks)
            {
                if(boxes[r.X,r.Y].isContainingPiece() && !boxes[r.X,r.Y].piece.isMoved)
                {
                    if(r.Y>p.Y)
                    {
                        int i;
                        for( i=p.Y+1;i<r.Y;i++)
                        {
                            if (boxes[p.X, i].isContainingPiece()) break;
                        }
                        if(i==r.Y)
                        {
                            List<Move> tempList = getAvailableMove(boxes[p.X, p.Y].piece.color == Color.Black ? Color.White : Color.Black,false); 
                            foreach(Move move in tempList)
                            {
                                if(move.dest.X==p.X)
                                for (i = p.Y ; i <= r.Y; i++)
                                {
                                    if (move.dest.Y == i)
                                    {
                                        i = -1;
                                        break;
                                    }
                                }
                                if (i == -1) break;
                            }
                            if (i != -1) rs.Add(new Move(p, new Point(p.X, p.Y + 2)));
                        }
                    }
                    else
                    {
                        int i;
                        for (i = r.Y + 1; i < p.Y; i++)
                        {
                            if (boxes[p.X, i].isContainingPiece()) break;
                        }
                        if (i == p.Y)
                        {
                            List<Move> tempList = getAvailableMove(boxes[p.X, p.Y].color == Color.Black ? Color.White : Color.Black,false);
                            foreach (Move move in tempList)
                            {
                                if (move.dest.X == p.X)
                                    for (i = r.Y ; i <= p.Y; i++)
                                    {
                                        if (move.dest.Y == i)
                                        {
                                            i = -1;
                                            break;
                                        }
                                    }
                                if (i == -1) break;
                            }
                            if (i != -1) rs.Add(new Move(p, new Point(p.X, p.Y - 2)));
                        }
                    }
                }
            }
            return rs;
        }
    }
}
