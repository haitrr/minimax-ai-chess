namespace MyChess
{
    partial class Game
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.GameBoardPanel = new MyChess.DoubleBufferedPanel();
            this.UndoButton = new System.Windows.Forms.Button();
            this.gameMenuStrip = new System.Windows.Forms.MenuStrip();
            this.gameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.heuristicValueTextBox = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.predictedHeuristicValue = new System.Windows.Forms.TextBox();
            this.difficultyLable = new System.Windows.Forms.Label();
            this.difficultyComboBox = new System.Windows.Forms.ComboBox();
            this.restartButton = new System.Windows.Forms.Button();
            this.presentValueLable = new System.Windows.Forms.Label();
            this.predictedValueLable = new System.Windows.Forms.Label();
            this.depthLable = new System.Windows.Forms.Label();
            this.depthTextBox = new System.Windows.Forms.TextBox();
            this.gameMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // GameBoardPanel
            // 
            this.GameBoardPanel.Location = new System.Drawing.Point(12, 31);
            this.GameBoardPanel.Name = "GameBoardPanel";
            this.GameBoardPanel.Size = new System.Drawing.Size(561, 561);
            this.GameBoardPanel.TabIndex = 0;
            this.GameBoardPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.GameBoardPanel_Paint);
            this.GameBoardPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.GameBoardPanel_MouseClick);
            // 
            // UndoButton
            // 
            this.UndoButton.Location = new System.Drawing.Point(592, 492);
            this.UndoButton.Name = "UndoButton";
            this.UndoButton.Size = new System.Drawing.Size(75, 23);
            this.UndoButton.TabIndex = 1;
            this.UndoButton.Text = "Undo";
            this.UndoButton.UseVisualStyleBackColor = true;
            this.UndoButton.Click += new System.EventHandler(this.UndoButton_Click);
            // 
            // gameMenuStrip
            // 
            this.gameMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gameToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.gameMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.gameMenuStrip.Name = "gameMenuStrip";
            this.gameMenuStrip.Size = new System.Drawing.Size(689, 24);
            this.gameMenuStrip.TabIndex = 2;
            this.gameMenuStrip.Text = "Game";
            this.gameMenuStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.gameMenuStrip_ItemClicked);
            // 
            // gameToolStripMenuItem
            // 
            this.gameToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.gameToolStripMenuItem.Name = "gameToolStripMenuItem";
            this.gameToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.gameToolStripMenuItem.Text = "Game";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // heuristicValueTextBox
            // 
            this.heuristicValueTextBox.Location = new System.Drawing.Point(376, 4);
            this.heuristicValueTextBox.Name = "heuristicValueTextBox";
            this.heuristicValueTextBox.Size = new System.Drawing.Size(100, 20);
            this.heuristicValueTextBox.TabIndex = 3;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // predictedHeuristicValue
            // 
            this.predictedHeuristicValue.Location = new System.Drawing.Point(582, 4);
            this.predictedHeuristicValue.Name = "predictedHeuristicValue";
            this.predictedHeuristicValue.Size = new System.Drawing.Size(100, 20);
            this.predictedHeuristicValue.TabIndex = 5;
            // 
            // difficultyLable
            // 
            this.difficultyLable.AutoSize = true;
            this.difficultyLable.Location = new System.Drawing.Point(117, 6);
            this.difficultyLable.Name = "difficultyLable";
            this.difficultyLable.Size = new System.Drawing.Size(47, 13);
            this.difficultyLable.TabIndex = 6;
            this.difficultyLable.Text = "Difficulty";
            // 
            // difficultyComboBox
            // 
            this.difficultyComboBox.FormattingEnabled = true;
            this.difficultyComboBox.Location = new System.Drawing.Point(170, 3);
            this.difficultyComboBox.Name = "difficultyComboBox";
            this.difficultyComboBox.Size = new System.Drawing.Size(121, 21);
            this.difficultyComboBox.TabIndex = 7;
            this.difficultyComboBox.SelectedIndexChanged += new System.EventHandler(this.difficultChange);
            // 
            // restartButton
            // 
            this.restartButton.Location = new System.Drawing.Point(592, 255);
            this.restartButton.Name = "restartButton";
            this.restartButton.Size = new System.Drawing.Size(75, 23);
            this.restartButton.TabIndex = 8;
            this.restartButton.Text = "Restart";
            this.restartButton.UseVisualStyleBackColor = true;
            this.restartButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.restartButtonClicked);
            // 
            // presentValueLable
            // 
            this.presentValueLable.AutoSize = true;
            this.presentValueLable.Location = new System.Drawing.Point(297, 7);
            this.presentValueLable.Name = "presentValueLable";
            this.presentValueLable.Size = new System.Drawing.Size(73, 13);
            this.presentValueLable.TabIndex = 9;
            this.presentValueLable.Text = "Present Value";
            // 
            // predictedValueLable
            // 
            this.predictedValueLable.AutoSize = true;
            this.predictedValueLable.Location = new System.Drawing.Point(491, 6);
            this.predictedValueLable.Name = "predictedValueLable";
            this.predictedValueLable.Size = new System.Drawing.Size(82, 13);
            this.predictedValueLable.TabIndex = 10;
            this.predictedValueLable.Text = "Predicted Value";
            // 
            // depthLable
            // 
            this.depthLable.AutoSize = true;
            this.depthLable.Location = new System.Drawing.Point(614, 31);
            this.depthLable.Name = "depthLable";
            this.depthLable.Size = new System.Drawing.Size(36, 13);
            this.depthLable.TabIndex = 14;
            this.depthLable.Text = "Depth";
            // 
            // depthTextBox
            // 
            this.depthTextBox.Location = new System.Drawing.Point(582, 56);
            this.depthTextBox.Name = "depthTextBox";
            this.depthTextBox.Size = new System.Drawing.Size(100, 20);
            this.depthTextBox.TabIndex = 15;
            // 
            // Game
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(689, 600);
            this.Controls.Add(this.depthTextBox);
            this.Controls.Add(this.depthLable);
            this.Controls.Add(this.predictedValueLable);
            this.Controls.Add(this.presentValueLable);
            this.Controls.Add(this.restartButton);
            this.Controls.Add(this.difficultyComboBox);
            this.Controls.Add(this.difficultyLable);
            this.Controls.Add(this.predictedHeuristicValue);
            this.Controls.Add(this.heuristicValueTextBox);
            this.Controls.Add(this.UndoButton);
            this.Controls.Add(this.GameBoardPanel);
            this.Controls.Add(this.gameMenuStrip);
            this.MainMenuStrip = this.gameMenuStrip;
            this.Name = "Game";
            this.Text = "Game";
            this.gameMenuStrip.ResumeLayout(false);
            this.gameMenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel GameBoardPanel;
        private System.Windows.Forms.Button UndoButton;
        private System.Windows.Forms.MenuStrip gameMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem gameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.TextBox heuristicValueTextBox;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.TextBox predictedHeuristicValue;
        private System.Windows.Forms.Label difficultyLable;
        private System.Windows.Forms.ComboBox difficultyComboBox;
        private System.Windows.Forms.Button restartButton;
        private System.Windows.Forms.Label presentValueLable;
        private System.Windows.Forms.Label predictedValueLable;
        private System.Windows.Forms.Label depthLable;
        private System.Windows.Forms.TextBox depthTextBox;
    }
}