using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace Chess 
{
    class Node 
    {
        private static System.Security.Cryptography.RandomNumberGenerator rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
        public static ulong[,] zobristHashing;
        public ChessBoard board { get; set; }
        public Player p;
        public ulong index { get; private set; }
        public ulong hashValue { get; private set; }
        public static void init()
        {
            zobristHashing = new ulong[16, 48];
            for (int i = 0; i < 16; i++)
                for (int j = 0; j < 48; j++)
                {
                    zobristHashing[i, j] = GetPseudoRandomNumber();
                }
        }
        public Node(ChessBoard Board, Player P,ulong tableSize)
        {
            board = Board;
            p = P;
            getZobristHashValue();
            index = hashValue ;//% tableSize;
        }
        public Node(){}
        public void updateNode(Move move, ulong tableSize)
        {
            xOrPos(move.dest.X, move.dest.Y);
            xOrPos(move.source.X, move.source.Y);
            if(move.castling==true)
            {
                if (move.dest.X == 6) 
                {
                    xOrPos(move.source.X,5);
                    xOrPos(move.source.X,7);
                }
                else
                {
                    xOrPos(move.source.X, 0);
                    xOrPos(move.source.X, 3);
                }
            }
            board.movePiece(move.source,move.dest,null,false);
            xOrPos(move.dest.X, move.dest.Y);
            xOrPos(move.source.X, move.source.Y);
            if (move.castling == true)
            {
                if (move.dest.X == 6)
                {
                    xOrPos(move.source.X, 5);
                    xOrPos(move.source.X, 7);
                }
                else
                {
                    xOrPos(move.source.X, 0);
                    xOrPos(move.source.X, 3);
                }
            }
            index = hashValue;// % tableSize;
        }
        public Node Clone()
        {
            Node clone = new Node();
            clone.board = board.Clone();
            clone.hashValue = hashValue;
            clone.index = index;
            clone.p = new Player("", p.color);
            return clone;
        }
        public void xOrPos(int i,int j)
        {
            if (board.boxes[i, j].isContainingPiece())
            switch (board.boxes[i, j].piece.GetType().Name)
            {
                    
                case "King":
                    if (board.boxes[i, j].piece.color == Color.Black)
                    {
                        hashValue ^= zobristHashing[i, j];
                    }
                    else
                    {
                        hashValue ^= zobristHashing[i + 8, j];
                    }
                    break;
                case "Bishop":
                    if (board.boxes[i, j].piece.color == Color.Black)
                    {
                        hashValue ^= zobristHashing[i, j + 8];
                    }
                    else
                    {
                        hashValue ^= zobristHashing[i + 8, j + 8];
                    }
                    break;
                case "Pawn":
                    if (board.boxes[i, j].piece.color == Color.Black)
                    {
                        hashValue ^= zobristHashing[i, j + 8 * 2];
                    }
                    else
                    {
                        hashValue ^= zobristHashing[i + 8, j + 8 * 2];
                    }
                    break;
                case "Queen":
                    if (board.boxes[i, j].piece.color == Color.Black)
                    {
                        hashValue ^= zobristHashing[i, j + 8 * 3];
                    }
                    else
                    {
                        hashValue ^= zobristHashing[i + 8, j + 8 * 3];
                    }
                    break;
                case "Knight":
                    if (board.boxes[i, j].piece.color == Color.Black)
                    {
                        hashValue ^= zobristHashing[i, j + 8 * 4];
                    }
                    else
                    {
                        hashValue ^= zobristHashing[i + 8, j + 8 * 4];
                    }
                    break;
                case "Rook":
                    if (board.boxes[i, j].piece.color == Color.Black)
                    {
                        hashValue ^= zobristHashing[i, j + 8 * 5];
                    }
                    else
                    {
                        hashValue ^= zobristHashing[i + 8, j + 8 * 5];
                    }
                    break;
            }
        }
        public void getZobristHashValue()
        {
            hashValue = 1;
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                   xOrPos(i, j);
                }
        }
        
        public override bool Equals(object obj)
        {
            Node other = obj as Node;
            if(GetHashCode()==other.GetHashCode()) return true;
            else return false;
            if(other.p.color.Equals(p.color))
            return board.Equals(other.board);
            return false;
        }
        public override int GetHashCode()
        {
            return board.GetHashCode() + p.GetHashCode();
        }
        public static UInt64 GetPseudoRandomNumber()
        {
            byte[] random64Bits = new byte[8];
            rng.GetBytes(random64Bits);
            return BitConverter.ToUInt64(random64Bits, 0);
        }
    }
}
