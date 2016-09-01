using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace Chess
{
    class Player
    {
        public string name {get;private set;}
        public Color color {get;private set;}
        public Player()
        {

        }
        public Player(string Name,Color Colour)
        {
            name = Name;
            color = Colour;
        }
    }
}
