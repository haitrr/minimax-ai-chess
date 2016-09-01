﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class Point
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public Point(int x,int y)
        {
            X = x;
            Y = y;
        }
        public static bool operator ==(Point p1,Point p2)
        {
            if (p1.X == p2.X && p1.Y == p2.Y) return true;
            return false;
        }
        public static bool operator !=(Point p1,Point p2)
        {
            if (p1.X == p2.X && p1.Y == p2.Y) return false;
            return true;
        }
    }
}
