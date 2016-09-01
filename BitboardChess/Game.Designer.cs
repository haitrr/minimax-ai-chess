namespace Chess
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
            this.GameBoardPanel = new System.Windows.Forms.Panel();
            this.UndoButton = new System.Windows.Forms.Button();
            this.gameMenuStrip = new System.Windows.Forms.MenuStrip();
            this.gameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playerVsComputerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playerVsPlayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.blackTurnPictureBox = new System.Windows.Forms.PictureBox();
            this.whiteTurnPictureBox = new System.Windows.Forms.PictureBox();
            this.thingkingLable = new System.Windows.Forms.Label();
            this.depthLable = new System.Windows.Forms.Label();
            this.depthTextBox = new System.Windows.Forms.TextBox();
            this.gameMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.blackTurnPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.whiteTurnPictureBox)).BeginInit();
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
            this.newGameToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.gameToolStripMenuItem.Name = "gameToolStripMenuItem";
            this.gameToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.gameToolStripMenuItem.Text = "Game";
            // 
            // newGameToolStripMenuItem
            // 
            this.newGameToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playerVsComputerToolStripMenuItem,
            this.playerVsPlayerToolStripMenuItem});
            this.newGameToolStripMenuItem.Name = "newGameToolStripMenuItem";
            this.newGameToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.newGameToolStripMenuItem.Text = "New Game";
            // 
            // playerVsComputerToolStripMenuItem
            // 
            this.playerVsComputerToolStripMenuItem.Name = "playerVsComputerToolStripMenuItem";
            this.playerVsComputerToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.playerVsComputerToolStripMenuItem.Text = "Player vs Computer";
            this.playerVsComputerToolStripMenuItem.Click += new System.EventHandler(this.playerVsComputerToolStripMenuItem_Click);
            // 
            // playerVsPlayerToolStripMenuItem
            // 
            this.playerVsPlayerToolStripMenuItem.Name = "playerVsPlayerToolStripMenuItem";
            this.playerVsPlayerToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.playerVsPlayerToolStripMenuItem.Text = "Player vs Player";
            this.playerVsPlayerToolStripMenuItem.Click += new System.EventHandler(this.playerVsPlayerToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
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
            // blackTurnPictureBox
            // 
            this.blackTurnPictureBox.Location = new System.Drawing.Point(582, 82);
            this.blackTurnPictureBox.Name = "blackTurnPictureBox";
            this.blackTurnPictureBox.Size = new System.Drawing.Size(100, 100);
            this.blackTurnPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.blackTurnPictureBox.TabIndex = 11;
            this.blackTurnPictureBox.TabStop = false;
            // 
            // whiteTurnPictureBox
            // 
            this.whiteTurnPictureBox.Location = new System.Drawing.Point(579, 349);
            this.whiteTurnPictureBox.Name = "whiteTurnPictureBox";
            this.whiteTurnPictureBox.Size = new System.Drawing.Size(100, 100);
            this.whiteTurnPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.whiteTurnPictureBox.TabIndex = 12;
            this.whiteTurnPictureBox.TabStop = false;
            // 
            // thingkingLable
            // 
            this.thingkingLable.AutoSize = true;
            this.thingkingLable.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.thingkingLable.Location = new System.Drawing.Point(589, 185);
            this.thingkingLable.Name = "thingkingLable";
            this.thingkingLable.Size = new System.Drawing.Size(83, 17);
            this.thingkingLable.TabIndex = 13;
            this.thingkingLable.Text = "Thinking....";
            this.thingkingLable.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.Controls.Add(this.thingkingLable);
            this.Controls.Add(this.whiteTurnPictureBox);
            this.Controls.Add(this.blackTurnPictureBox);
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
            ((System.ComponentModel.ISupportInitialize)(this.blackTurnPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.whiteTurnPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel GameBoardPanel;
        private System.Windows.Forms.Button UndoButton;
        private System.Windows.Forms.MenuStrip gameMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem gameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newGameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playerVsComputerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playerVsPlayerToolStripMenuItem;
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
        private System.Windows.Forms.PictureBox blackTurnPictureBox;
        private System.Windows.Forms.PictureBox whiteTurnPictureBox;
        private System.Windows.Forms.Label thingkingLable;
        private System.Windows.Forms.Label depthLable;
        private System.Windows.Forms.TextBox depthTextBox;
    }
}