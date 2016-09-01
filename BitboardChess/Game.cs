using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess
{
    public partial class Game : Form
    {
        private const int BoardHeight = 560;
        private const int BoardWidth = 560;
        private Graphics graphics;
        public Game()
        {
            InitializeComponent();
            
        }

        private void GameBoardPanel_Paint(object sender, PaintEventArgs e)
        {
        }

        private void GameBoardPanel_MouseClick(object sender, MouseEventArgs e)
        {
            
        }
        public void announceWinner()
        {
            MessageBox.Show( " has won the game!");
        }

        private void UndoButton_Click(object sender, EventArgs e)
        {
        }
        private void gameMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            
        }

        private void playerVsComputerToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void playerVsPlayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        public void initDifficultyComboBox()
        {
        }
        public void difficultChange(object sender, EventArgs e)
        {
        }
        public void restartButtonClicked(object sender, MouseEventArgs e)
        {

        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chess 1.0");
        }
        public void init()
        {
           
        }
    }
}
