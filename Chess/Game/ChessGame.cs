using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;
using System.Collections.Concurrent;
namespace Chess
{
    class ChessGame
    {
        ConcurrentDictionary<ulong, Evalue> transpositionTable;
        public System.Windows.Forms.TextBox heuristicValueTextBox, predictedHeuristicValue, searchedDepth;
        public System.Windows.Forms.PictureBox whiteTurn, blackTurn;
        public System.Windows.Forms.Label thinking;
        public enum Difficulty { veryEasy,easy,hard,veryHard};
        public Difficulty difficulty { get; set; }
        private enum GameMode { PvP, CovP };
        private GameMode gameMode;
        private ChessBoard chessBoard;
        private ulong tableSize;
        public Player Black { get; private set; }
        public Player White { get; private set; }
        public Player onTurn { get; private set; }
        private List<int> depthList;
        int count = 0;
        public ChessGame(int BoardHeight,int BoardWidth)
        {
            Box.init();
            chessBoard = new ChessBoard(BoardHeight, BoardWidth);
            difficulty = Difficulty.easy;
            Node.init();
            transpositionTable = new ConcurrentDictionary<ulong, Evalue>();
            tableSize = 18000000;
        }
        public void drawGame(Graphics g)
        {
            chessBoard.drawBoard(g);
        }
        public int heuristic(ChessBoard board,Player p)
        {
            int mypoint=0;
            int enemypoint=0;
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8;j++ )
                {
                    if(board.boxes[i,j].piece!=null)
                    {
                        if(board.boxes[i,j].piece.color==p.color)
                        {
                            mypoint += board.boxes[i, j].piece.evalueMaxtrix[i, j];
                            //mypoint += board.boxes[i, j].piece.getPoint(p.color, new Point(i, j));
                        }
                        else
                        {
                            enemypoint += board.boxes[i, j].piece.evalueMaxtrix[i, j];
                            //mypoint += board.boxes[i, j].piece.getPoint(remainingPlayer(p).color, new Point(i, j));
                        }
                    }
                }
            return mypoint-enemypoint;
        }
        public Move interationDeepeningParallel(ChessBoard board, int timeout, Player p, int x, int y, Node node)
        {
            double temp;
            DateTime begin = DateTime.Now;
            int depth = 0;
            Move bestMove;
            do
            {
                depth++;
                //score = minimaxWithTranspositionTable(board, depth, p, Int32.MinValue, Int32.MaxValue, x, y, node.Clone());
                bestMove = RootAlphaBetaParallel(board.Clone(), depth, p, x, y);
                temp = (DateTime.Now - begin).TotalMilliseconds;
            } while (temp < timeout);
            //predictedHeuristicValue.Text = max.ToString();
            searchedDepth.Text = depth + "   in " + temp.ToString("0.00") + "s";
            return bestMove;
        }
        public Move interationDeepening(ChessBoard board,int timeout,Player player,int x,int y,Node node,bool useParallel,bool useTranspositionTable)
        {
            double temp;
            DateTime begin=DateTime.Now;
            int depth = 0;
            Move bestMove = null;
            do
            {
                depth++;
                bestMove = RootAlphaBeta(board, depth, player, x, y, useParallel,useTranspositionTable);
                temp = (DateTime.Now - begin).TotalMilliseconds;
            } while (temp < timeout);
            searchedDepth.Text = depth + "   in " + temp.ToString("0.00") + "s";
            return bestMove;
        }
        public List<Move> getBestAvailableMoves(ChessBoard board, Player p,int movesAmount)
        {
            ChessBoard state;
            int temp;
            List<int> evalueList=new List<int>();
            List<Move> availableMove = getAvailableMove(board, p);
            List<Move> bestMoves = new List<Move>();
            state = board.Clone();
            foreach (Move move in availableMove)
            {
                state.movePiece(move.source, move.dest, null,false);
                temp = heuristic(state, p);
                move.evalue = temp;
                state.undoMove(null);
                if (bestMoves.Count < movesAmount)
                {
                    bestMoves.Add(move);
                    evalueList.Add(temp);
                }
                else
                {
                    for (int i = 0; i < movesAmount; i++)
                    {
                        if (temp > evalueList[i])
                        {
                            evalueList[i] = temp;
                            bestMoves[i] = move;
                            break;
                        }
                    }
                }
            }
            return bestMoves.OrderByDescending(o => o.evalue).ToList();
        }
        public int minimaxWithTranspositionTable(ChessBoard board,int depth,Player P,int alpha,int beta,int x,int y,Node parent)
        {
            if (transpositionTable.ContainsKey(parent.index))
            {
                count++;
                Evalue temp = transpositionTable[parent.index];
                if (temp.depth >= depth)
                {
                    if (temp.type == Evalue.Type.Exact)
                        return temp.value;
                    if (temp.type == Evalue.Type.Alpha)
                    {
                        if (temp.value > alpha) alpha = temp.value;
                    }
                    else if (temp.type == Evalue.Type.Beta)
                    {
                        if (temp.value < beta) beta = temp.value;
                    }
                    if (alpha >= beta)
                    {
                        return temp.value;
                    }

                }

            }

            int bestValue;
            if (board.gameOver)
            {
                return heuristic(board, onTurn);
            }
            if (depth == 0)
            {
                int v=heuristic(board, onTurn);
                return quiescenceSearch(board.Clone(), depth, P, alpha, beta, x, y, v);
            }
            if(P==onTurn)
            {
                bestValue = Int32.MinValue;
                List<Move> availableMove = getBestAvailableMoves(board, P,x);
                foreach (Move m in availableMove)
                {
                    board.movePiece(m.source, m.dest, null,false);
                    Node node = parent.Clone();
                    node.updateNode(m, tableSize);
                    int v = minimaxWithTranspositionTable(board, depth - 1, remainingPlayer(P), alpha, beta, x, y, node);
                    bestValue = Math.Max(bestValue, v);
                    alpha = Math.Max(bestValue, alpha);
                    board.undoMove(null);
                    if (beta <= alpha) break;
                }
            }
            else
            {
                bestValue = Int32.MaxValue;
                List<Move> availableMove = getBestAvailableMoves(board, P, y);
                foreach (Move m in availableMove)
                {
                    board.movePiece(m.source, m.dest, null,false);
                    Node node = parent.Clone();
                    node.updateNode(m, tableSize);
                    int v = minimaxWithTranspositionTable(board, depth - 1, remainingPlayer(P), alpha, beta, x, y, node);
                    bestValue = Math.Min(bestValue, v);
                    beta = Math.Min(beta, bestValue);
                    board.undoMove( null);
                    if (beta <= alpha) break;
                }
            }
            if (alpha >= beta)
            {
                if (!transpositionTable.ContainsKey(parent.index)) transpositionTable.TryAdd(parent.index, new Evalue(bestValue, depth, Evalue.Type.Exact));
                else
                {
                    transpositionTable[parent.index].depth = depth;
                    transpositionTable[parent.index].value = bestValue;
                    transpositionTable[parent.index].type = Evalue.Type.Exact;
                }
            }else
            if (bestValue <= alpha)
            {
                if(!transpositionTable.ContainsKey(parent.index))  transpositionTable.TryAdd(parent.index, new Evalue(bestValue, depth, Evalue.Type.Alpha));
                else
                {
                    transpositionTable[parent.index].depth = depth;
                    transpositionTable[parent.index].value = bestValue;
                    transpositionTable[parent.index].type = Evalue.Type.Alpha;
                }
            }
            else if (bestValue >= beta)
            {
                if (!transpositionTable.ContainsKey(parent.index)) transpositionTable.TryAdd(parent.index, new Evalue(bestValue, depth, Evalue.Type.Beta));
                else
                {
                    transpositionTable[parent.index].depth = depth;
                    transpositionTable[parent.index].value = bestValue;
                    transpositionTable[parent.index].type = Evalue.Type.Beta;
                }
            }
            return bestValue;
        }
        public int minimax(ChessBoard board, int depth, Player P, int alpha, int beta, int x, int y)
        {

            int bestValue;
            if (depth == 0 || board.gameOver)
            {
                int v = heuristic(board, onTurn);
                return quiescenceSearch(board.Clone(), depth, P, alpha, beta, x, y, v);
            }
            if (P == onTurn)
            {
                bestValue = Int32.MinValue;
                List<Move> availableMove = getBestAvailableMoves(board, P, x);
                foreach (Move m in availableMove)
                {
                    
                    board.movePiece(m.source, m.dest, null, false);
                    int v = minimax(board, depth - 1, remainingPlayer(P), alpha, beta, x, y);
                    bestValue = Math.Max(bestValue, v);
                    alpha = Math.Max(bestValue, alpha);
                    board.undoMove(null);
                    if (beta <= alpha) break;
                }
            }
            else
            {
                bestValue = Int32.MaxValue;
                List<Move> availableMove = getBestAvailableMoves(board, P, y);
                foreach (Move m in availableMove)
                {
                    board.movePiece(m.source, m.dest, null, false);
                    int v = minimax(board, depth - 1, remainingPlayer(P), alpha, beta, x, y);
                    bestValue = Math.Min(bestValue, v);
                    beta = Math.Min(beta, bestValue);
                    board.undoMove(null);
                    if (beta <= alpha) break;
                }
            }
            return bestValue;
        }
        public int quiescenceSearch(ChessBoard board, int depth, Player P, int alpha, int beta, int x, int y, int previousValue)
        {
            Move bestMove=null;
            int bestValue;
            if (depth == 0 || board.gameOver)
            {
                int v = heuristic(board, onTurn);
                return v;
            }
            if (P == onTurn)
            {
                bestValue = Int32.MinValue;
                List<Move> availableMove = getBestAvailableMoves(board, P, x);
                foreach (Move m in availableMove)
                {

                    board.movePiece(m.source, m.dest, null, false);
                    int v = minimax(board, depth - 1, remainingPlayer(P), alpha, beta, x, y);
                    //bestValue = Math.Max(bestValue, v);
                    if (bestValue < v)
                    {
                        bestValue = v;
                        bestMove = m;
                    }
                    alpha = Math.Max(bestValue, alpha);
                    board.undoMove(null);
                    if (beta <= alpha) break;
                }
            }
            else
            {
                bestValue = Int32.MaxValue;
                List<Move> availableMove = getBestAvailableMoves(board, P, y);
                foreach (Move m in availableMove)
                {
                    board.movePiece(m.source, m.dest, null, false);
                    int v = minimax(board, depth - 1, remainingPlayer(P), alpha, beta, x, y);
                    bestValue = Math.Min(bestValue, v);
                    if (bestValue > v)
                    {
                        bestValue = v;
                        bestMove = m;
                    }
                    beta = Math.Min(beta, bestValue);
                    board.undoMove(null);
                    if (beta <= alpha) break;
                }
            }
            if(Math.Abs(previousValue-bestValue)>=200)
            {
                board.movePiece(bestMove.source, bestMove.dest, null, false);
                int v = quiescenceSearch(board, 1, remainingPlayer(P), alpha, beta, x, y,bestValue);
            }
            return bestValue;
        }
        public Move RootAlphaBeta(ChessBoard cboard, int depth, Player player, int x, int y,bool useParallel,bool useTranspositionTable)
        {
            if (useParallel)
            {

                List<Move> tempList = getAvailableMove(cboard, player);
                int moveCount = tempList.Count;
                Move[] availableMove = new Move[moveCount];
                for (int i = 0; i < moveCount; i++)
                {
                    availableMove[i] = tempList[i];
                }
                ChessBoard[] boards = new ChessBoard[moveCount];
                int[] scores = new int[moveCount];
                for (int i = 0; i < moveCount; i++)
                {
                    boards[i] = cboard.Clone();
                }
                Move bestMove = null;
                int max = Int32.MinValue;
                Parallel.For(0, moveCount, i =>
                {
                    boards[i].movePiece(availableMove[i].source, availableMove[i].dest, null, false);
                    if(useTranspositionTable)
                        scores[i] = minimaxWithTranspositionTable(boards[i], depth - 1, remainingPlayer(player), Int32.MinValue, Int32.MaxValue, x, y, new Node(boards[i].Clone(), remainingPlayer(player), tableSize));
                    else
                        scores[i] = minimax(boards[i], depth - 1, remainingPlayer(player), Int32.MinValue, Int32.MaxValue, x, y);
                });
                for (int i = 0; i < moveCount; i++)
                {
                    if (scores[i] > max)
                    {
                        max = scores[i];
                        bestMove = availableMove[i];
                    }
                }
                predictedHeuristicValue.Text = max.ToString();
                return bestMove;
            }
            else
            {
                List<Move> availableMove = getAvailableMove(cboard, player);
                int temp, max = Int32.MinValue;
                ChessBoard board = cboard.Clone();
                Move bestMove = null;
                foreach (Move move in availableMove)
                {
                    board.movePiece(move.source, move.dest, null, false);
                    if (useTranspositionTable)
                        temp = minimaxWithTranspositionTable(board, depth - 1, remainingPlayer(player), Int32.MinValue, Int32.MaxValue, x, y, new Node(board.Clone(), remainingPlayer(player), tableSize));
                    else
                        temp = minimax(board, depth - 1, remainingPlayer(player), Int32.MinValue, Int32.MaxValue, x, y);
                    if (max < temp)
                    {
                        max = temp;
                        bestMove = move;
                    }
                    board.undoMove(null);
                }
                predictedHeuristicValue.Text = max.ToString();
                return bestMove;
            }
        }
        public Move RootAlphaBetaParallel(ChessBoard cboard,int depth,Player player,int x,int y)
        {
            List<Move> tempList=getAvailableMove(cboard, player);
            int moveCount = tempList.Count;
            Move[] availableMove=new Move[moveCount];
            for (int i = 0; i < moveCount;i++ )
            {
                availableMove[i] = tempList[i];
            }
            ChessBoard[] boards = new ChessBoard[moveCount];
            int[] scores=new int[moveCount];
            for(int i=0;i<moveCount;i++)
            {
                boards[i]=cboard.Clone();
            }
            Move bestMove = null;
            //ChessBoard board;
            int max = Int32.MinValue;
            Parallel.For(0, moveCount, i =>
            {
                boards[i].movePiece(availableMove[i].source, availableMove[i].dest, null, false);
                //scores[i] = minimax(boards[i], depth - 1, remainingPlayer(player), Int32.MinValue, Int32.MaxValue, x, y);
                scores[i] = minimaxWithTranspositionTable(boards[i], depth - 1, remainingPlayer(player), Int32.MinValue, Int32.MaxValue, x, y, new Node(boards[i].Clone(), remainingPlayer(player),tableSize));
            });
            for(int i=0;i<moveCount;i++)
            {
                if(scores[i]>max)
                {
                    max = scores[i];
                    bestMove = availableMove[i];
                }
            }
        //    foreach (Move move in availableMove)
        //    {
        //        board.movePiece(move.source, move.dest, null, false);
        //        temp = minimax(board.Clone(), depth, player, Int32.MinValue, Int32.MaxValue, x, y);
        //        if (max < temp)
        //        {
        //            max = temp;
        //            bestMove = move;
        //        }
        //        board.undoMove(null);
        //    }
            predictedHeuristicValue.Text = max.ToString();
            return bestMove;
        }
        public Move getMachineMove()
        {
            //int temp;
            //List<Move> availableMove = getAvailableMove(chessBoard, onTurn);
            //int max = Int32.MinValue;
            ChessBoard board = chessBoard.Clone();
            Move bestMove = null;
            DateTime begin = DateTime.Now;
            transpositionTable = new ConcurrentDictionary<ulong, Evalue>();
            depthList = new List<int>();
            //foreach (Move move in availableMove)
            //{
            //    board.movePiece(move.source, move.dest, null,false);
            //    switch (difficulty)
            //    {
            //        //case Difficulty.veryEasy: temp = minimax(board, 1, remainingPlayer(onTurn), Int32.MinValue, Int32.MaxValue, 30, 30); break;
            //        //case Difficulty.easy: temp = minimax(board, 2, remainingPlayer(onTurn), Int32.MinValue, Int32.MaxValue, 10, 10); break;
            //        //case Difficulty.hard: temp = minimax(board, 4, remainingPlayer(onTurn), Int32.MinValue, Int32.MaxValue, 6, 6); break;
            //        //case Difficulty.veryHard: temp = minimax(board, 5, remainingPlayer(onTurn), Int32.MinValue, Int32.MaxValue, 100,100 ); break;
            //        //case Difficulty.veryHard: temp = minimaxWithTranspositionTable(board,3, remainingPlayer(onTurn), Int32.MinValue, Int32.MaxValue, 100, 100, new Node(board.Clone(), remainingPlayer(onTurn), tableSize)); break;
            //        case Difficulty.veryEasy: temp = interationDeepening(board,20, remainingPlayer(onTurn), 5, 5, new Node(board.Clone(), remainingPlayer(onTurn), tableSize)); break;
            //        case Difficulty.easy: temp = interationDeepening(board, 20, remainingPlayer(onTurn), 10, 10, new Node(board.Clone(), remainingPlayer(onTurn), tableSize)); break;
            //        case Difficulty.hard: temp = interationDeepening(board, 20, remainingPlayer(onTurn), 20, 20, new Node(board.Clone(), remainingPlayer(onTurn), tableSize)); break;
            //        case Difficulty.veryHard: temp = interationDeepening(board, 20, remainingPlayer(onTurn), 100, 100, new Node(board.Clone(), remainingPlayer(onTurn),tableSize)); break;
            //        default: temp = 1; break;
            //    }
            //    if (max < temp)
            //    {
            //        max = temp;
            //        bestMove = move;
            //    }
            //    board.undoMove(null);
            //}
            bestMove = interationDeepening(board, 700, onTurn, 100, 100, new Node(board.Clone(), onTurn, tableSize),true,true);
            //searchedDepth.Text = (DateTime.Now - begin).TotalSeconds.ToString("0.00");
            //bestMove=interationDeepeningParallel(board, 300,onTurn, 100, 100, new Node(board.Clone(), onTurn, tableSize));
            //bestMove = interationDeepening(board, 300, onTurn, 100, 100, new Node(board.Clone(), onTurn, tableSize));
            //predictedHeuristicValue.Text = max.ToString();
            depthList = depthList.OrderBy(O => O).ToList();
            return bestMove;
            //State state=new State(Black.color,chessBoard.Clone());
            //MonteCarloTree MCTree = new MonteCarloTree(state, 1000,Black);
            //return MCTree.getBestMove();
        }
        public Player remainingPlayer(Player p)
        {
            if (p == Black) return White;
            return Black;
        }
        public List<Move> getAvailableMove(ChessBoard board, Player player)
        {
            return board.getAvailableMove(player.color,true);
        }
        public bool startCoPGame(Graphics g)
        {
            chessBoard.init();
            Black = new Player("Computer", Color.Black);
            White = new Player("Player", Color.White);
            gameMode = GameMode.CovP;
            onTurn=White;
            return false;
        }
        public void doComputerMove(Graphics g)
        {
            Move temp = getMachineMove();
            chessBoard.movePiece(temp.source, temp.dest, g,false);
            heuristicValueTextBox.Text = heuristic(chessBoard, onTurn).ToString();
            nextTurn();
        }
        public bool startCoPGame_Test(Graphics g)
        {
            Black = new Player("Player", Color.Black);
            White = new Player("Computer", Color.White);
            gameMode = GameMode.CovP;
            onTurn = White;
            return false;
        }
        public bool startMultiPlayerGame()
        {
            chessBoard.init();
            Black = new Player("Black",Color.Black);
            White = new Player("White",Color.White);
            gameMode = GameMode.PvP;
            return getFirstPlayer();    
        }
        public int doPlayerMove(Point source, Point dest, Graphics g)
        {
            if (chessBoard.movePiece(source, dest, g,true) == true)
            {
                chessBoard.unSelect(g, true);
                if (chessBoard.gameOver) return 2;
                nextTurn();
                if (gameMode == GameMode.CovP)
                {
                    doComputerMove(g);
                }
                if (chessBoard.gameOver) return 2;
                return 0;
            }
            return 1;
        }
        public bool processClick(Point p,Graphics g)
        {
            if (chessBoard.isSelecting())
            {
                int temp = doPlayerMove(chessBoard.selecting, p, g);
                if (temp == 2) return true;
                else if (temp == 0) ;
                else
                    if (chessBoard.boxes[p.X, p.Y].isContainingPiece() && chessBoard.boxes[p.X, p.Y].piece.color == onTurn.color)
                {
                    chessBoard.unSelect(g, false);
                    chessBoard.selecting = p;
                    chessBoard.drawSelect(g);
                }
                else
                    chessBoard.unSelect(g, false);
            }
            else
            if (chessBoard.isContainingPiece(p))
            {
                    if (chessBoard.getPieceColor(p) == onTurn.color)
                    {
                        chessBoard.selecting=p;
                        chessBoard.drawSelect(g);
                    }
            }
            return false;
        }
        public Player getWinner()
        {
            if (chessBoard.winner == Color.Black) return Black;
            else return White;
        }
        public void nextTurn()
        {
            if (onTurn == Black)
            {
                onTurn = White;
                whiteTurn.Visible=true;
                blackTurn.Visible=false;
                thinking.Visible = false;
            }
            else
            {
                onTurn = Black;
                whiteTurn.Visible = false;
                blackTurn.Visible = true;
                blackTurn.Refresh();
                if (gameMode == GameMode.CovP)
                {
                    thinking.Visible = true;
                    thinking.Refresh();
                }
            }
        }
        public void UndoMove(Graphics g)
        {
            if(gameMode==GameMode.CovP)if (onTurn.name != "Computer" && chessBoard.tracer.count == 1) return;
            chessBoard.undoMove(g);
            nextTurn();
            if (gameMode == GameMode.CovP && onTurn.name=="Computer")
            {
                if(chessBoard.undoMove(g))
                nextTurn();
            }
        }
        public bool getFirstPlayer()
        {
            Random r;
            r = new Random();
            int turn= r.Next(1,3);
            if (turn == 1)
            {
                onTurn = Black;
                return true;
            }
            onTurn = White;
            return false;

        }
         public void unSelectPiece(Graphics g,bool isMoved)
        {
            chessBoard.unSelect(g, isMoved);
        }
        public bool restartGame()
         {
             chessBoard.init();
             return getFirstPlayer();
         }

    }
}
