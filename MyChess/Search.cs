using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Collections.Concurrent;
using System.Threading.Tasks;
namespace MyChess
{
    class Search
    {
        public static int bestSearchedValue;
        public static double tableSize=1000000;
        public static ulong transpositionTableUsed = 0;
        public static ConcurrentDictionary<ulong, NodeData> transpositionTable = new ConcurrentDictionary<ulong, NodeData>();
        public static double nodeProcessed = 0;
        public static int negamaxWithAlphaBeta(int alpha, int beta, int quieseDepth, int depth, ulong WP, ulong WN, ulong WB, ulong WR, ulong WQ, ulong WK, ulong BP, ulong BN, ulong BB, ulong BR, ulong BQ, ulong BK, ulong EP, bool CWK, bool CWQ, bool CBK, bool CBQ, bool white)
        {
            //ulong hashCode = Zobrist.getZobristHash(WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK, EP, CWK, CWQ, CBK, CBQ, white);
            //if (transpositionTable.ContainsKey(hashCode) && transpositionTable[hashCode].depth >= depth)
            //{
            //    NodeData data = transpositionTable[hashCode];
            //    if (data.type == NodeData.Type.Exact)
            //    {
            //        return data.value;
            //    }
            //    if (data.type == NodeData.Type.Alpha)
            //    {
            //        alpha = Math.Max(data.value, alpha);
            //    }
            //    else if (data.type == NodeData.Type.Beta)
            //    {
            //        beta = Math.Min(beta, data.value);
            //    }
            //    if (alpha >= beta) return data.value;
            //}
            
            if (depth == 0)
            {
                int value = Evalue.evalue(WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK, white);
                return value;
            }
            string legalMove = Moves.LegalMoves(WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK, EP, CWK, CWQ, CBK, CBQ, white);
            if(legalMove.Length==0)
            {
                if ((white && (WK & Moves.unSafeForWhite(WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK)) == 0) ||
                        (!white && (BK & Moves.unSafeForBlack(WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK)) == 0))
                    return 0;
                else return -20000;
            }
            int bestValue = -20000;
            int moveCount = legalMove.Length / 4;
            string[] moves = new string[moveCount];
            for (int i = 0; i < moveCount; i++)
            {
                moves[i] = legalMove.Substring(i * 4, 4);
            }
            for (int i = 0; i < moveCount; i ++)
            {
                ulong WRt = Moves.makeMoveCastle(WR, WK | BK, moves[i], 'R');
                ulong BRt = Moves.makeMoveCastle(BR, WK | BK, moves[i], 'r');
                WRt = Moves.makeMove(WR, moves[i], 'R');
                BRt = Moves.makeMove(BR, moves[i], 'r');
                ulong WPt = Moves.makeMove(WP, moves[i], 'P'), WNt = Moves.makeMove(WN, moves[i], 'N'),
                            WBt = Moves.makeMove(WB, moves[i], 'B'),
                            WQt = Moves.makeMove(WQ, moves[i], 'Q'), WKt = Moves.makeMove(WK, moves[i], 'K'),
                            BPt = Moves.makeMove(BP, moves[i], 'p'), BNt = Moves.makeMove(BN, moves[i], 'n'),
                            BBt = Moves.makeMove(BB, moves[i], 'b'),
                            BQt = Moves.makeMove(BQ, moves[i], 'q'), BKt = Moves.makeMove(BK, moves[i], 'k'),
                            EPt = Moves.makeMoveEP(WP | BP, moves[i]);
                bool CWKt = CWK, CWQt = CWQ, CBKt = CBK, CBQt = CBQ;
                if (char.IsDigit(moves[i][3]))
                {
                    int start = (moves[i][0] - '0') * 8 + moves[i][1] - '0';
                    if (((1ul << start) & WK) != 0) { CWKt = false; CWQt = false; }
                    else if (((1ul << start) & BK) != 0) { CBKt = false; CBQt = false; }
                    else if (((1ul << start) & WR & (1ul << 63)) != 0) { CWKt = false; }
                    else if (((1ul << start) & WR & (1ul << 56)) != 0) { CWQt = false; }
                    else if (((1ul << start) & BR & (1ul << 7)) != 0) { CBKt = false; }
                    else if (((1ul << start) & BR & 1ul) != 0) { CBQt = false; }
                }
                int childValue = -negamaxWithAlphaBeta(-beta, -alpha, quieseDepth, depth - 1, WPt, WNt, WBt, WRt, WQt, WKt, BPt, BNt, BBt, BRt, BQt, BKt, EPt, CWKt, CWQt, CBKt, CBQt, white ? false : true);
                bestValue = Math.Max(bestValue, childValue);
                alpha = Math.Max(alpha, childValue);
                if (alpha >= beta) break;
            }
            //hashCode = Zobrist.getZobristHash(WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK, EP, CWK, CWQ, CBK, CBQ, white);
            //if (bestValue <= alpha)
            //{
            //    transpositionTable.AddOrUpdate(hashCode, new NodeData(bestValue, depth, NodeData.Type.Beta, legalMove), (key, oldData) => oldData = new NodeData(bestValue, depth, NodeData.Type.Beta, legalMove));
            //}
            //else if (bestValue >= beta)
            //{
            //    transpositionTable.AddOrUpdate(hashCode, new NodeData(bestValue, depth, NodeData.Type.Alpha, legalMove), (key, oldData) => oldData = new NodeData(bestValue, depth, NodeData.Type.Alpha, legalMove));
            //}
            //else
            //{
            //    transpositionTable.AddOrUpdate(hashCode, new NodeData(bestValue, depth, NodeData.Type.Exact, ""), (key, oldData) => oldData = new NodeData(bestValue, depth, NodeData.Type.Exact, ""));
            //}
            return bestValue;
        }
        public static string negamaxRoot(int depth, int quieseDepth, string moveToStartWith, ulong WP, ulong WN, ulong WB, ulong WR, ulong WQ, ulong WK, ulong BP, ulong BN, ulong BB, ulong BR, ulong BQ, ulong BK, ulong EP, bool CWK, bool CWQ, bool CBK, bool CBQ, bool white)
        {
            string bestMove = "";
            nodeProcessed = 0;
            int alpha = -100000, beta = 100000;
            string legalMove = Moves.LegalMoves(WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK, EP, CWK, CWQ, CBK, CBQ, white);
            if (legalMove.Length == 0)
            {
                return null;
            }
            legalMove = moveToStartWith + legalMove;
            int bestValue = -50000;
            int moveCount = legalMove.Length / 4;
            int[] childValues = new int[moveCount];
            string[] moves = new string[moveCount];
            for (int i = 0; i < moveCount; i++)
            {
                moves[i] = legalMove.Substring(i * 4, 4);
            }
            //Parallel.For(0, moveCount, i =>
            for (int i = 0; i < moveCount; i++)
            {
                ulong WRt = Moves.makeMoveCastle(WR, WK | BK, moves[i], 'R');
                ulong BRt = Moves.makeMoveCastle(BR, WK | BK, moves[i], 'r');
                WRt = Moves.makeMove(WR, moves[i], 'R');
                BRt = Moves.makeMove(BR, moves[i], 'r');
                ulong WPt = Moves.makeMove(WP, moves[i], 'P'), WNt = Moves.makeMove(WN, moves[i], 'N'),
                            WBt = Moves.makeMove(WB, moves[i], 'B'),
                            WQt = Moves.makeMove(WQ, moves[i], 'Q'), WKt = Moves.makeMove(WK, moves[i], 'K'),
                            BPt = Moves.makeMove(BP, moves[i], 'p'), BNt = Moves.makeMove(BN, moves[i], 'n'),
                            BBt = Moves.makeMove(BB, moves[i], 'b'),
                            BQt = Moves.makeMove(BQ, moves[i], 'q'), BKt = Moves.makeMove(BK, moves[i], 'k'),
                            EPt = Moves.makeMoveEP(WP | BP, moves[i]);
                bool CWKt = CWK, CWQt = CWQ, CBKt = CBK, CBQt = CBQ;
                if (char.IsDigit(moves[i][3]))
                {
                    int start = (moves[i][0] - '0') * 8 + moves[i][1] - '0';
                    if (((1ul << start) & WK) != 0) { CWKt = false; CWQt = false; }
                    else if (((1ul << start) & BK) != 0) { CBKt = false; CBQt = false; }
                    else if (((1ul << start) & WR & (1ul << 63)) != 0) { CWKt = false; }
                    else if (((1ul << start) & WR & (1ul << 56)) != 0) { CWQt = false; }
                    else if (((1ul << start) & BR & (1ul << 7)) != 0) { CBKt = false; }
                    else if (((1ul << start) & BR & 1ul) != 0) { CBQt = false; }
                }
                childValues[i] = -negamaxWithAlphaBeta(-beta, -alpha, quieseDepth, depth - 1, WPt, WNt, WBt, WRt, WQt, WKt, BPt, BNt, BBt, BRt, BQt, BKt, EPt, CWKt, CWQt, CBKt, CBQt, white ? false : true);
            }
            for (int i = 0; i < moveCount; i++)
            {
                if (childValues[i] > bestValue)
                {
                    bestValue = childValues[i];
                    bestMove = moves[i];
                }
            }
            bestSearchedValue = bestValue;
            return bestMove;
        }
        public static string interationDeepening(int maxDepth, int quieseDepth, double timeout, ulong WP, ulong WN, ulong WB, ulong WR, ulong WQ, ulong WK, ulong BP, ulong BN, ulong BB, ulong BR, ulong BQ, ulong BK, ulong EP, bool CWK, bool CWQ, bool CBK, bool CBQ, bool white)
        {
            Stopwatch sW=new Stopwatch();
            sW.Start();
            int depth = 0;
            string bestMove = "";
            do
            {
                depth++;
                //bestMove = rootAlphaBeta(depth, bestMove, WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK, EP, CWK, CWQ, CBK, CBQ, white);
                bestMove = negamaxRoot(depth, quieseDepth,bestMove, WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK, EP, CWK, CWQ, CBK, CBQ, white); ;
            } while (sW.ElapsedMilliseconds < timeout && depth<maxDepth);
            sW.Stop();
            Console.WriteLine(transpositionTable.Count);
            return bestMove;
        }
        public static int quieseSearch(int depth,int alpha, int beta, ulong WP, ulong WN, ulong WB, ulong WR, ulong WQ, ulong WK, ulong BP, ulong BN, ulong BB, ulong BR, ulong BQ, ulong BK, ulong EP, bool CWK, bool CWQ, bool CBK, bool CBQ, bool white)
        {
            int standPat = Evalue.evalue(WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK, white);
            if(depth==0) return standPat;
            if (standPat > beta) return beta;
            if (alpha > standPat) alpha = standPat;
            string captureMoves = Moves.captureMoves(WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK, EP, CWK, CWQ, CBK, CBQ, white);
            for (int i = 0; i < captureMoves.Length; i += 4)
            {
                ulong WRt = Moves.makeMoveCastle(WR, WK | BK, captureMoves.Substring(i, 4), 'R');
                ulong BRt = Moves.makeMoveCastle(BR, WK | BK, captureMoves.Substring(i, 4), 'r');
                WRt = Moves.makeMove(WR, captureMoves.Substring(i, 4), 'R');
                BRt = Moves.makeMove(BR, captureMoves.Substring(i, 4), 'r');
                ulong WPt = Moves.makeMove(WP, captureMoves.Substring(i, 4), 'P'), WNt = Moves.makeMove(WN, captureMoves.Substring(i, 4), 'N'),
                            WBt = Moves.makeMove(WB, captureMoves.Substring(i, 4), 'B'),
                            WQt = Moves.makeMove(WQ, captureMoves.Substring(i, 4), 'Q'), WKt = Moves.makeMove(WK, captureMoves.Substring(i, 4), 'K'),
                            BPt = Moves.makeMove(BP, captureMoves.Substring(i, 4), 'p'), BNt = Moves.makeMove(BN, captureMoves.Substring(i, 4), 'n'),
                            BBt = Moves.makeMove(BB, captureMoves.Substring(i, 4), 'b'),
                            BQt = Moves.makeMove(BQ, captureMoves.Substring(i, 4), 'q'), BKt = Moves.makeMove(BK, captureMoves.Substring(i, 4), 'k'),
                            EPt = Moves.makeMoveEP(WP | BP, captureMoves.Substring(i, 4));
                bool CWKt = CWK, CWQt = CWQ, CBKt = CBK, CBQt = CBQ;
                if (char.IsDigit(captureMoves[i + 3]))
                {//'regular' move
                    int start = (captureMoves[i] - '0') * 8 + captureMoves[i + 1] - '0';
                    /*if (((1ul<<start)&(WP|BP))!=0) {
                        if (Math.abs(captureMoves[i)-captureMoves[i+2))==2) {
                            EPt=Moves.FileMasks8[captureMoves[i+1)-'0'];
                        }
                    }
                    else */
                    if (((1ul << start) & WK) != 0) { CWKt = false; CWQt = false; }
                    else if (((1ul << start) & BK) != 0) { CBKt = false; CBQt = false; }
                    else if (((1ul << start) & WR & (1ul << 63)) != 0) { CWKt = false; }
                    else if (((1ul << start) & WR & (1ul << 56)) != 0) { CWQt = false; }
                    else if (((1ul << start) & BR & (1ul << 7)) != 0) { CBKt = false; }
                    else if (((1ul << start) & BR & 1ul) != 0) { CBQt = false; }
                }
                int score = -quieseSearch(depth-1,-beta, -alpha, WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK, EP, CWK, CWQ, CBK, CBQ, white?false:true);
                if (score >= beta)
                    return beta;
                if (score > alpha)
                    alpha = score;
            }
            return alpha;
        }
    }
}

