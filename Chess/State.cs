using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Chess
{
    class State
    {
        public ChessBoard board { get; set; }
        public int winAmount { get; set; }
        public int playAmount { get; set; }
        public Color player { get; set; }
        public bool isPlayed { get; set; }
        public State parent { get; set; }
        public List<State> child { get; set; }
        public int availableChild {get;set;}
        public Move createMove { get; set; }
        public State(Color p,ChessBoard b)
        {
            board = b;
            winAmount = 0;
            playAmount = 0;
            player = p;
            isPlayed = false;
            child = new List<State>();
            createMove = null;
            availableChild = 0;
        }
        public State Clone()
        {
            State s = (State)MemberwiseClone();
            s.board = board.Clone();
            s.isPlayed = false;
            s.child = new List<State>();
            s.availableChild = 0;
            return s;
        }
        public void updatePlayer()
        {
            if(player==Color.Black)
            {
                player = Color.White;
            }
            else
            {
                player = Color.Black;
            }
        }
        public bool Equals(State s)
        {
            if(s.player==player)
            {
                if (board.Equals(s.board)) return true;
            }
            return false;
        }
    }
}
