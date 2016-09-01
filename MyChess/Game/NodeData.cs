using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace MyChess
{
    class NodeData
    {
        public enum Type { Alpha, Beta, Exact }
        public int value { get; set; }
        public int depth { get; set; }
        public Type type { get; set; }
        //public string legalMoves;
        public NodeData(int Value, int Depth, Type t,string LegalMoves)
        {
            value = Value;
            depth = Depth;
            type = t;
            //legalMoves = LegalMoves;
        }
    }
}
