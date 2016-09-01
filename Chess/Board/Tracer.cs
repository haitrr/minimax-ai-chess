using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class Tracer
    {
        public bool isHasValidTrace { get; private set; }
        public int count { get; set; }
        private Stack<Move> savedMove ;
        private Stack<Piece> sourcePiece;
        private Stack<Piece> destPiece;
        public Stack<bool> castlingFlag { get; set; }
        public Tracer()
        {
            isHasValidTrace = false;
            savedMove = new Stack<Move>();
            sourcePiece = new Stack<Piece>();
            destPiece = new Stack<Piece>();
            castlingFlag = new Stack<bool>();
            count = 0;
        }
        public void AddMove(Move m,Piece source,Piece dest)
        {
            isHasValidTrace = true;
            savedMove.Push(m);
            sourcePiece.Push(source);
            destPiece.Push(dest);
            castlingFlag.Push(false); count++;
        }
        public Move UndoMove()
        {
            if (savedMove.Count == 1) isHasValidTrace = false;
            count--;
            return savedMove.Pop();
        }
        public Piece undoSourcePiece()
        {
            return sourcePiece.Pop();
        }
        public Piece undoDestPiece()
        {
            return destPiece.Pop();
        }
        public Move previousMove()
        {
            return savedMove.Peek();
        }
        //public Tracer Clone()
        //{
        //    Tracer clone = new Tracer();
        //    if (isHasValidTrace)
        //    {
        //        clone.AddMove(savedMove.Peek(), sourcePiece.Peek(), destPiece.Peek());
        //        clone.castlingFlag.Push(castlingFlag.Peek());
        //        clone.isHasValidTrace = true;
        //    }
        //    return clone;
        //}
    }
}
