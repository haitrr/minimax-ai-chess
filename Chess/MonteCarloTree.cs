using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace Chess
{
    class MonteCarloTree
    {
        private State rootState;
        private Color player,winner;
        private List<State> stateList;
        private List<State> notPlayed;
        private double timeLimit;
        public MonteCarloTree(State rs,double time,Player p)
        {
            player = p.color;
            rootState = rs;
            rootState.parent = null;
            timeLimit = time;
            stateList = new List<State>();
            notPlayed = new List<State>();
        }
        public Move getBestMove()
        {
            DateTime beginTime = DateTime.Now;
            stateList.Add(rootState);
            spawnNewState(rootState);
            //List<Move> availableMove = rootState.board.getAvailableMove(rootState.player);
            //foreach(Move move in availableMove)
            //{
            //    State s = rootState.Clone();
            //    s.board.movePiece(move.source, move.dest, null);
            //    s.updatePlayer();
            //    s.parent = rootState;
            //    s.isPlayed = true;
            //    stateList.Add(s);
            //    notPlayed.Add(s);
            //    rootState.child.Add(s);
            //}
            int simulateTime = 0;
            while ((DateTime.Now - beginTime).TotalMilliseconds < timeLimit)
            {
                simulate();
                simulateTime++;
            }
            float rootR = (float)rootState.winAmount / rootState.playAmount;
            float max = 0,winrate;
            Move choose=null;
            foreach(State state in rootState.child)
            {
                
                if (state.playAmount >= max)
                {
                    max = state.playAmount;
                    choose = state.createMove;
                }
            }
            //for(int i=0;i<availableMove.Count;i++)
            //{
            //    if (stateList[i].playAmount != 0)
            //    {
            //        winrate = (float)stateList[i].winAmount / stateList[i].playAmount;
            //        if (winrate >= max)
            //        {
            //            max = winrate;
            //            choose = availableMove[i];
            //        }
            //    }
            //}
            return choose;
        }
        public Move selectMove(State state)
        {
            Random r = new Random();
            //List<Move> availableMove = state.board.getAvailableMove(state.player.color);
            //Move choose = availableMove[r.Next(availableMove.Count)];
            //return choose;
            List<Move> availableMove = state.board.getAvailableMove(state.player,true);
            //tempListMove = availableMove;
            List<Move> bestMove = new List<Move>();
            List<double> evalue=new List<double>();
            State s;
            int max = Int32.MinValue, temp;
            Move choose = null;
            foreach (Move move in availableMove)
            {
                s = state.Clone();
                s.board.movePiece(move.source, move.dest, null,false);
                temp = heuristic(s.board, state.player);
                //if (temp >= max)
                //{
                //    max = temp;
                //    choose = move;
                //}
                if (bestMove.Count < 5)
                {
                    bestMove.Add(move);
                    evalue.Add(temp);
                }
                else
                {
                    for (int i = 0; i < evalue.Count; i++)
                    {
                        if (temp > evalue[i])
                        {
                            evalue[i] = temp;
                            bestMove[i] = move;
                            break;
                        }
                    }
                }
            }
            return bestMove[r.Next(bestMove.Count)];
            //return availableMove[r.Next(availableMove.Count)];
        }
        public int heuristic(ChessBoard board,Color player)
        {
            int mypoint = 0;
            int enemypoint = 0;
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    if (board.boxes[i, j].isContainingPiece())
                    {
                        if (board.boxes[i, j].piece.color == player)
                        {
                            mypoint += board.boxes[i, j].piece.getPoint(player, new Point(i, j));
                        }
                        else
                        {
                            enemypoint += board.boxes[i, j].piece.getPoint((player==Color.Black)?Color.White:Color.Black, new Point(i, j));
                        }
                    }
                }
            return mypoint - enemypoint;
        }
        public State chooseState(State s)
        {
            //Random r=new Random();
            //State rs=null;
            //double max = double.MinValue,temp;
            //foreach(State state in stateList)
            //{
            //    if (state.availableChild == 0) continue;
            //    temp = UTC(state, Math.Sqrt(2));
            //    if(temp>max)
            //    {
            //        max = temp;
            //        rs = state;
            //    }
            //}
            //while (true)
            //{
            //    State rss = rs.child[r.Next(rs.child.Count)];
            //    if (notPlayed.Contains(rss)) return rss;
            //}
            State state = s,rs=null;
            double temp;
            double max=double.MinValue;
            if (state.child.Count==0) return state;
            foreach(State child in state.child)
            {
                if (child.playAmount > 0)
                {
                    temp = UTC(child, Math.Sqrt(2));
                    if (temp > max)
                    {
                        max = temp;
                        rs = child;
                    }
                }
                else return child;
            }
            return chooseState(rs);
        }
        public void simulate()
        {
            //Random r=new Random();
            //int choose = r.Next(notPlayed.Count);

            //State state = notPlayed[choose];
            State state = chooseState(rootState);
            if(state.playAmount>4 )
                spawnNewState(state);
            stateList.Add(state);
            notPlayed.Remove(state);
            state.parent.availableChild--;
            //notPlayed.RemoveAt(choose);
           
            State rState = state.Clone();
            int moveAmount = 0;
            while (moveAmount<100)
            {
                
                Move move=selectMove(rState);
                rState.board.movePiece(move.source, move.dest,null,false);
                rState.updatePlayer();
                //if (moveAmount == 0)
                //{
                //    State s = state.Copy();
                //    s.parent = state;
                //    notPlayed.Add(s);
                //}
                if(rState.board.gameOver)
                {
                    winner = rState.board.winner;
                    break;
                }
                moveAmount++;
            }
            if(moveAmount<120) backpropagate(state, winner);
        }
        public void backpropagate(State state,Color winner)
        {
            bool win = false;
            if (winner == player)
            {
                state.winAmount++;
                win = true;
            }
            state.playAmount++;
            while(state.parent!=null)
            {
                state.parent.playAmount ++;
                if (win) state.parent.winAmount++;
                state = state.parent;
            }
        }
        public State isIn(List<State> list,State state)
        {
            foreach(State s in list)
            {
                if (state.Equals(s)) return s;
            }
            return null;
        }
        public void spawnNewState(State state)
        {
            int temp;
            State s;
            List<State> sList=new List<State>();
            List<int> choose=new List<int>();
            List<int> evalue = new List<int>();
            List<Move> availableMove = state.board.getAvailableMove(state.player,true);
            for (int i = 0; i < availableMove.Count;i++ )
            {
                s = state.Clone();
                s.board.movePiece(availableMove[i].source, availableMove[i].dest, null,false);
                temp = heuristic(s.board, s.player);
                s.updatePlayer();
                s.createMove = availableMove[i];
                sList.Add(s);
                evalue.Add(temp);
            }
            for (int i = 0; i < evalue.Count;i++ )
            {
                if (choose.Count < 5)
                {
                    choose.Add(i);
                    continue;
                }
                else
                {
                    for (int j = 0; j < choose.Count; j++)
                    {
                        if (evalue[i] > evalue[choose[j]])
                        {
                            choose[j] = i;
                            break;
                        }
                    }
                }
            }
            foreach(int i in choose)
            {
                //if (sList[i].board.gameOver == false)
                //{
                    state.availableChild++;
                    sList[i].parent = state;
                    state.child.Add(sList[i]);
                    notPlayed.Add(sList[i]);
                //}
            }
        }
        public double UTC(State state,double C)
        {
            if (state == rootState) return 0;
            if (state.playAmount == 0) return 0;
            double utc=0;
            utc += (double)state.winAmount / state.playAmount;
            double temp;
            temp = C * Math.Sqrt(Math.Log(state.parent.playAmount) / state.playAmount);
            utc += temp;
            return utc;
        }
    }
}
