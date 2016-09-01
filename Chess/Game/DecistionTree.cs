using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Chess
{
    class DecistionTree
    {
        private Point source;
        private Point dest;
        private int winAmount;
        private int loseAmount;
        private float winrate;
        private List<DecistionTree> childTree;
        private bool isRoot;
        private string path;
        public DecistionTree()
        {
            path = "//database//";
            childTree.Sort((x, y) => x.winrate.CompareTo(y.winrate));
            isRoot = false;
        }
        public void Decistion()
        {
            
        }
        public void WriteToDatabase()
        {
            string fileName = source.X + "," + source.Y +"-"+ dest.X + "," + dest.Y + ".txt";
            if(isRoot) File.Create(path+ fileName);
            TextWriter tw = new StreamWriter(path + fileName);
            tw.WriteLine(winrate);
            tw.WriteLine(winAmount);
            tw.WriteLine(loseAmount);
            tw.WriteLine(path);
            tw.WriteLine(isRoot);
            foreach (DecistionTree tree in childTree)
            {
                tw.WriteLine(tree.source.X + "," + tree.source.Y + "-" + tree.dest.X + "," + tree.dest.Y + tree.winrate);
                Directory.CreateDirectory(path + tree.source.X + "," + tree.source.Y+"-"+tree.dest.X+","+tree.dest.Y);
                tree.WriteToDatabase();
            }
        }
    }
}
