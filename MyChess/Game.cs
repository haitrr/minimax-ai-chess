using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyChess.Properties;
namespace MyChess
{
    public partial class Game : Form
    {
        private const int BoardHeight = 560;
        private const int BoardWidth = 560;
        private Graphics graphics;
        public Game()
        {
            InitializeComponent();
            Zobrist.zobristFillArray();
            Moves.init();
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            initDifficultyComboBox();
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            //graphics = GameBoardPanel.CreateGraphics();
            //ChessBoard.graphics = graphics;
            init();
            difficultyComboBox.SelectedIndex = 2;
            ChessBoard.init();
            difficultyComboBox.Enabled = true;
        }
        private void GameBoardPanel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            ChessBoard.graphics = e.Graphics;
            ChessBoard.draw();
        }

        private void GameBoardPanel_MouseClick(object sender, MouseEventArgs e)
        {
            int x = e.Y / (BoardHeight / 8), y = e.X / (BoardWidth / 8);
            if (ChessBoard.selecting == -1)
            {
                if (((char.IsUpper(ChessBoard.charBoard[x, y]) && ChessBoard.white) || (!char.IsUpper(ChessBoard.charBoard[x, y]) && !ChessBoard.white)) && ChessBoard.charBoard[x, y]!=' ')
                {
                    ChessBoard.selecting = x * 8 + y;
                    GameBoardPanel.Invalidate();
                }
            }
            else
            {
                if ((char.IsUpper(ChessBoard.charBoard[x, y]) && ChessBoard.white) & ChessBoard.charBoard[x, y] != ' ' || (!char.IsUpper(ChessBoard.charBoard[x, y]) && !ChessBoard.white) & ChessBoard.charBoard[x, y] != ' ')
                {
                    ChessBoard.UnSelect();
                    GameBoardPanel.Invalidate();
                    GameBoardPanel.Refresh();
                }
                else
                {
                    string move=ChessBoard.checkMove(x,y);
                    if (move != null)
                    {
                        ChessBoard.makeMove(move);
                        GameBoardPanel.Invalidate();
                        GameBoardPanel.Refresh();
                        if (ChessBoard.gameOver && ChessBoard.staledraw) MessageBox.Show("Draw");
                        else
                            if (ChessBoard.gameOver) MessageBox.Show(!ChessBoard.white ? "White won!" : "Black won!");
                            else
                            {
                                ChessBoard.makeBestMoves();
                                GameBoardPanel.Invalidate();
                                if (ChessBoard.gameOver && ChessBoard.staledraw) MessageBox.Show("Draw");
                                else
                                    if (ChessBoard.gameOver) MessageBox.Show(!ChessBoard.white ? "White won!" : "Black won!");
                            }
                    }
                    else
                    {
                        ChessBoard.UnSelect();
                        GameBoardPanel.Invalidate();
                    }
                }
            }
        }
        public void announceWinner()
        {
        }

        private void UndoButton_Click(object sender, EventArgs e)
        {
            ChessBoard.undoMove();
            GameBoardPanel.Invalidate();
        }
        private void gameMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            
        }

        private void playerVsComputerToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void playerVsPlayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
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
                case 0: ChessGame.difficulty = ChessGame.Difficulty.veryEasy; break;
                case 1: ChessGame.difficulty = ChessGame.Difficulty.easy; break;
                case 2: ChessGame.difficulty = ChessGame.Difficulty.hard; break;
                case 3: ChessGame.difficulty = ChessGame.Difficulty.veryHard; break;
                default: break;
            }
        }
        public void restartButtonClicked(object sender, MouseEventArgs e)
        {
            ChessBoard.init();
            GameBoardPanel.Invalidate();
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
            ChessGame.heuristicValueTextBox = heuristicValueTextBox;
            ChessGame.predictedHeuristicValue = predictedHeuristicValue;
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
            ChessGame.searchedDepth = depthTextBox;
        }
    }
}
