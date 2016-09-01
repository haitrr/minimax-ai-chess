using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace Chess
{
    class Evalue
    {
        public enum Type { Alpha,Beta,Exact}
        public int value { get; set; }
        public int depth { get; set; }
        public  Type type { get;set;}
        public Evalue(int Value,int Depth,Type t)
        {
            value = Value;
            depth = Depth;
        }
    }
}
