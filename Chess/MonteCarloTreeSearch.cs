//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Drawing;

//namespace Chess
//{
//    class MonteCarloTreeSearch
//    {
//        private List<ChessBoard> stateList;
//        private double calculationTime;
//        private double maxMove;
//        private Color player;
//        private Dictionary<ChessBoard, int> win, play;
//        private Lookup<ChessBoard, Color> wins, plays;
//        private Color winner;
//        private int maxDepth;
//        public MonteCarloTreeSearch(ChessBoard b)
//        {
//            stateList.Add(b);
//        }
//        public void Update()
//        {

//        }
//        public Move getMove()
//        {
//            maxDepth = 0;
//            ChessBoard state = stateList[stateList.Count - 1];
//            player = player;
//            List<Move> availiableMove = state.getAvailableMove(player);
//            if (availiableMove == null) return null;
//            if (availiableMove.Count==1) return availiableMove[0];
//            int games = 0;
//            DateTime beginTime = DateTime.Now;
//            while ((DateTime.Now - beginTime).TotalMilliseconds < calculationTime)
//            {
//                runSimulation();
//                games += 1;
//            }
//            foreach
//        }
//        public void runSimulation()
//        {
//            List<ChessBoard> visitedState=new List<ChessBoard>();
//            double movedAmount=0;
//            List<ChessBoard> copyList = copyStateList(stateList);
//            ChessBoard state = copyList[copyList.Count - 1];
//            //player = self.board.current_player(state)
//            bool expand = true;
//            while (movedAmount < maxMove)
//            {
//                List<Move> availableMove = state.getAvailableMove(player);
//                Random r = new Random();
//                Move choose = availableMove[r.Next(availableMove.Count)];
//                state.movePiece(choose.source, choose.dest, null);
//                copyList.Add(state);
//                if (expand && plays.Contains(state) == false)
//                {
//                    expand = false;
//                    win.Add(state, 0);
//                    play.Add(state, 0);
//                }
//                player = player;
//                winner = state.winner;
//                visitedState.Add(state);
//                if (state.gameOver) break;
//            }
//            foreach (ChessBoard s in visitedState)
//            {
//                if (visitedState.Contains(s) == false) continue;
//                play[state] += 1;
//                if (player == winner) win[s] += 1;
//            }
//        }
//        public List<ChessBoard> copyStateList(List<ChessBoard> b)
//        {
//            List<ChessBoard> rs = new List<ChessBoard>();
//            foreach(ChessBoard board in b)
//            {
//                rs.Add(board.deepCopy());
//            }
//            return rs;
//        }
//    }
//}
