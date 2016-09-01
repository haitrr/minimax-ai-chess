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
        private ChessGame chessGame;
        public Game()
        {
            InitializeComponent();
            initDifficultyComboBox();
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            graphics = GameBoardPanel.CreateGraphics();
            chessGame = new ChessGame(BoardHeight,BoardWidth);
            init();
            chessGame.difficulty = ChessGame.Difficulty.veryHard;
            difficultyComboBox.SelectedIndex = 3;
            playerVsComputerToolStripMenuItem_Click(null, null);

            //chessGame.startCoPGame_Test(graphics);
            //chessGame.drawGame(graphics);
            //whiteTurnPictureBox.Visible = true;
            //blackTurnPictureBox.Visible = false;
            //difficultyComboBox.Enabled = true;
            //heuristicValueTextBox.Enabled = true;
            //predictedHeuristicValue.Enabled = true;
            //difficultyComboBox.SelectedIndex = 3;
            //chessGame.doComputerMove(graphics);

            difficultyComboBox.Enabled = true;
        }

        private void GameBoardPanel_Paint(object sender, PaintEventArgs e)
        {
            chessGame.drawGame(graphics);
            chessGame.unSelectPiece(null, true);
        }

        private void GameBoardPanel_MouseClick(object sender, MouseEventArgs e)
        {
            Point click;
            click=new Point(e.Y / Box.boxWidth, e.X / Box.boxHeight);
            if(chessGame.processClick(click, graphics)) announceWinner() ;
            
        }
        public void announceWinner()
        {
            MessageBox.Show(chessGame.getWinner().name + " has won the game!");
        }

        private void UndoButton_Click(object sender, EventArgs e)
        {
            chessGame.UndoMove(graphics);
        }
        private void gameMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            
        }

        private void playerVsComputerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chessGame.startCoPGame_Test(graphics);
            chessGame.doComputerMove(graphics);
            //chessGame.startCoPGame(graphics);
            chessGame.drawGame(graphics);
            init();
            whiteTurnPictureBox.Visible = true;
        }

        private void playerVsPlayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            blackTurnPictureBox.Visible = false;
            whiteTurnPictureBox.Visible = false;
            if (chessGame.startMultiPlayerGame()) blackTurnPictureBox.Visible = true;
            else whiteTurnPictureBox.Visible=true;
            chessGame.drawGame(graphics);
            heuristicValueTextBox.Visible = false;
            predictedHeuristicValue.Visible = false;
            predictedValueLable.Visible = false;
            presentValueLable.Visible = false;
            difficultyLable.Visible = false;
            difficultyComboBox.Visible = false;
            depthTextBox.Visible = false;
            depthLable.Visible = false;
        }

        public void initDifficultyComboBox()
        {
            difficultyComboBox.Items.Add("Very Easy");    //0
            difficultyComboBox.Items.Add("Easy");           //1
            difficultyComboBox.Items.Add("Hard");           //2
            difficultyComboBox.Items.Add("Very Hard");      //3
            difficultyComboBox.Enabled = false;
        }
        public void difficultChange(object sender, EventArgs e)
        {
            switch(difficultyComboBox.SelectedIndex)
            {
                case 0: chessGame.difficulty = ChessGame.Difficulty.veryEasy; break;
                case 1: chessGame.difficulty = ChessGame.Difficulty.easy; break;
                case 2: chessGame.difficulty = ChessGame.Difficulty.hard; break;
                case 3: chessGame.difficulty = ChessGame.Difficulty.veryHard; break;
                default: break;
            }
        }
        public void restartButtonClicked(object sender, MouseEventArgs e)
        {
            chessGame.restartGame();
            chessGame.drawGame(graphics);
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
            chessGame.whiteTurn = whiteTurnPictureBox;
            chessGame.blackTurn = blackTurnPictureBox;
            chessGame.heuristicValueTextBox = heuristicValueTextBox;
            chessGame.predictedHeuristicValue = predictedHeuristicValue;
            chessGame.thinking = thingkingLable;
            whiteTurnPictureBox.Image= 
            blackTurnPictureBox.Image = new Bitmap (System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "turn.png"));
            whiteTurnPictureBox.Visible=false;
            blackTurnPictureBox.Visible=false;
            thingkingLable.Visible = false;
            heuristicValueTextBox.ReadOnly = true;
            predictedHeuristicValue.ReadOnly= true;
            depthLable.Visible = true;
            depthTextBox.Visible = true;
            depthTextBox.ReadOnly = true;
            heuristicValueTextBox.Visible = true;
            predictedHeuristicValue.Visible = true;
            heuristicValueTextBox.Text = "N/A";
            predictedHeuristicValue.Text = "N/A";
            heuristicValueTextBox.Visible = true; 
            predictedValueLable.Visible = true;
            presentValueLable.Visible = true;
            difficultyLable.Visible = true;
            difficultyComboBox.Visible = true;
            chessGame.searchedDepth = depthTextBox;
        }
    }
}
