using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class Move
    {
        public Point source { get; set;}
        public Point dest { get; set; }
        public int evalue { get; set; }
        public bool castling;
        public bool enPassant;
        public Move(Point Source,Point Dest)
        {
            source = Source;
            dest = Dest;
            castling = false;
            enPassant = false;
        }
    }
}
