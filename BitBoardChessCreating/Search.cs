using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitBoardChessCreating
{
    class Search
    {
        public static double nodeProcessed=0;
        public static int zeroWindowsSearch(int alpha, int beta, int depth, ulong WP, ulong WN, ulong WB, ulong WR, ulong WQ, ulong WK, ulong BP, ulong BN, ulong BB, ulong BR, ulong BQ, ulong BK, ulong EP, bool CWK, bool CWQ, bool CBK, bool CBQ, bool WhiteToMove)
        {
            return 0;
        }
        public static int getFirstLegalMove(string moves, ulong WP, ulong WN, ulong WB, ulong WR, ulong WQ, ulong WK, ulong BP, ulong BN, ulong BB, ulong BR, ulong BQ, ulong BK, ulong EP, bool CWK, bool CWQ, bool CBK, bool CBQ, bool WhiteToMove)
        {
            for (int i = 0; i < moves.Length; i += 4)
            {
                ulong WPt = Moves.makeMove(WP, moves.Substring(i,  4), 'P'), WNt = Moves.makeMove(WN, moves.Substring(i, 4), 'N'),
                        WBt = Moves.makeMove(WB, moves.Substring(i,  4), 'B'), WRt = Moves.makeMove(WR, moves.Substring(i, 4), 'R'),
                        WQt = Moves.makeMove(WQ, moves.Substring(i,  4), 'Q'), WKt = Moves.makeMove(WK, moves.Substring(i, 4), 'K'),
                        BPt = Moves.makeMove(BP, moves.Substring(i,  4), 'p'), BNt = Moves.makeMove(BN, moves.Substring(i, 4), 'n'),
                        BBt = Moves.makeMove(BB, moves.Substring(i,  4), 'b'), BRt = Moves.makeMove(BR, moves.Substring(i, 4), 'r'),
                        BQt = Moves.makeMove(BQ, moves.Substring(i,  4), 'q'), BKt = Moves.makeMove(BK, moves.Substring(i, 4), 'k');
                WRt = Moves.makeMoveCastle(WRt, WK | BK, moves.Substring(i,  4), 'R');
                BRt = Moves.makeMoveCastle(BRt, WK | BK, moves.Substring(i,  4), 'r');
                if (((WKt & Moves.unSafeForWhite(WPt, WNt, WBt, WRt, WQt, WKt, BPt, BNt, BBt, BRt, BQt, BKt)) == 0 && WhiteToMove) ||
                        ((BKt & Moves.unSafeForBlack(WPt, WNt, WBt, WRt, WQt, WKt, BPt, BNt, BBt, BRt, BQt, BKt)) == 0 && !WhiteToMove))
                {
                    return i;
                }
            }
            return -1;
        }
        public static int negamaxAlphaBeta(int alpha, int beta, int depth, ulong WP, ulong WN, ulong WB, ulong WR, ulong WQ, ulong WK, ulong BP, ulong BN, ulong BB, ulong BR, ulong BQ, ulong BK, ulong EP, bool CWK, bool CWQ, bool CBK, bool CBQ, bool WhiteToMove)
        {
            int bestValue;
            int bestValueIndex = -1;
            if (depth == 0)
            {
                bestValue = Evalue.evalue(WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK);
                return bestValue;
            }
            string moves;
            if (WhiteToMove)
            {
                moves = Moves.possibleMovesW(WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK, EP, CWK, CWQ, CBK, CBQ);
            }
            else
            {
                moves = Moves.possibleMovesB(WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK, EP, CWK, CWQ, CBK, CBQ);
            }
            int firstLegalMoveIndex = getFirstLegalMove(moves, WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK, EP, CWK, CWQ, CBK, CBQ,WhiteToMove);
            if (firstLegalMoveIndex == -1)
            {
                return WhiteToMove ? 30000 : -30000;
            }
            ulong WPt = Moves.makeMove(WP, moves.Substring(firstLegalMoveIndex, 4), 'P'), WNt = Moves.makeMove(WN, moves.Substring(firstLegalMoveIndex, 4), 'N'),
                    WBt = Moves.makeMove(WB, moves.Substring(firstLegalMoveIndex, 4), 'B'), WRt = Moves.makeMove(WR, moves.Substring(firstLegalMoveIndex, 4), 'R'),
                    WQt = Moves.makeMove(WQ, moves.Substring(firstLegalMoveIndex, 4), 'Q'), WKt = Moves.makeMove(WK, moves.Substring(firstLegalMoveIndex, 4), 'K'),
                    BPt = Moves.makeMove(BP, moves.Substring(firstLegalMoveIndex, 4), 'p'), BNt = Moves.makeMove(BN, moves.Substring(firstLegalMoveIndex, 4), 'n'),
                    BBt = Moves.makeMove(BB, moves.Substring(firstLegalMoveIndex, 4), 'b'), BRt = Moves.makeMove(BR, moves.Substring(firstLegalMoveIndex, 4), 'r'),
                    BQt = Moves.makeMove(BQ, moves.Substring(firstLegalMoveIndex, 4), 'q'), BKt = Moves.makeMove(BK, moves.Substring(firstLegalMoveIndex, 4), 'k'),
                    EPt = Moves.makeMoveEP(WP | BP, moves.Substring(firstLegalMoveIndex, 4));
            WRt = Moves.makeMoveCastle(WRt, WK | BK, moves.Substring(firstLegalMoveIndex, 4), 'R');
            BRt = Moves.makeMoveCastle(BRt, WK | BK, moves.Substring(firstLegalMoveIndex, 4), 'r');
            bool CWKt = CWK, CWQt = CWQ, CBKt = CBK, CBQt = CBQ;
            if (char.IsDigit(moves[firstLegalMoveIndex + 3]))
            {//'regular' move
                int start = (int)(char.GetNumericValue(moves[firstLegalMoveIndex]) * 8) + (int)(char.GetNumericValue(moves[firstLegalMoveIndex + 1]));
                if (((1ul << start) & WK) != 0) { CWKt = false; CWQt = false; }
                else if (((1ul << start) & BK) != 0) { CBKt = false; CBQt = false; }
                else if (((1ul << start) & WR & (1ul << 63)) != 0) { CWKt = false; }
                else if (((1ul << start) & WR & (1ul << 56)) != 0) { CWQt = false; }
                else if (((1ul << start) & BR & (1ul << 7)) != 0) { CBKt = false; }
                else if (((1ul << start) & BR & 1ul) != 0) { CBQt = false; }
            }
            bestValue = -negamaxAlphaBeta(-alpha, -beta, depth - 1, WPt, WNt, WBt, WRt, WQt, WKt, BPt, BNt, BBt, BRt, BQt, BKt, EPt, CWKt, CWQt, CBKt, CBQt, !WhiteToMove);
            if (Math.Abs(bestValue) == Int32.MaxValue)
            {
                return bestValue;
            }
            if (bestValue > alpha)
            {
                if (bestValue >= beta)
                {
                    return bestValue;
                }
                alpha = bestValue;
            }


            bestValueIndex = firstLegalMoveIndex;
            for (int i = firstLegalMoveIndex + 4; i < moves.Length; i += 4)
            {
                int score;
                //legal, non-castle move
                WPt = Moves.makeMove(WP, moves.Substring(i, 4), 'P');
                WNt = Moves.makeMove(WN, moves.Substring(i, 4), 'N');
                WBt = Moves.makeMove(WB, moves.Substring(i, 4), 'B');
                WRt = Moves.makeMove(WR, moves.Substring(i, 4), 'R');
                WQt = Moves.makeMove(WQ, moves.Substring(i, 4), 'Q');
                WKt = Moves.makeMove(WK, moves.Substring(i, 4), 'K');
                BPt = Moves.makeMove(BP, moves.Substring(i, 4), 'p');
                BNt = Moves.makeMove(BN, moves.Substring(i, 4), 'n');
                BBt = Moves.makeMove(BB, moves.Substring(i, 4), 'b');
                BRt = Moves.makeMove(BR, moves.Substring(i, 4), 'r');
                BQt = Moves.makeMove(BQ, moves.Substring(i, 4), 'q');
                BKt = Moves.makeMove(BK, moves.Substring(i, 4), 'k');
                EPt = Moves.makeMoveEP(WP | BP, moves.Substring(i, 4));
                WRt = Moves.makeMoveCastle(WRt, WK | BK, moves.Substring(i, 4), 'R');
                BRt = Moves.makeMoveCastle(BRt, WK | BK, moves.Substring(i, 4), 'r');
                CWKt = CWK;
                CWQt = CWQ;
                CBKt = CBK;
                CBQt = CBQ;
                if (char.IsDigit(moves[i + 3]))
                {//'regular' move
                    int start = (int)(char.GetNumericValue(moves[i]) * 8) + (int)(char.GetNumericValue(moves[i + 1]));
                    if (((1ul << start) & WK) != 0) { CWKt = false; CWQt = false; }
                    else if (((1ul << start) & BK) != 0) { CBKt = false; CBQt = false; }
                    else if (((1ul << start) & WR & (1ul << 63)) != 0) { CWKt = false; }
                    else if (((1ul << start) & WR & (1ul << 56)) != 0) { CWQt = false; }
                    else if (((1ul << start) & BR & (1ul << 7)) != 0) { CBKt = false; }
                    else if (((1ul << start) & BR & 1ul) != 0) { CBQt = false; }
                }
                score = -zeroWindowsSearch(-alpha, -beta, depth - 1, WPt, WNt, WBt, WRt, WQt, WKt, BPt, BNt, BBt, BRt, BQt, BKt, EPt, CWKt, CWQt, CBKt, CBQt, !WhiteToMove);
                if(score>alpha &&score <beta)
                {
                    score = -negamaxAlphaBeta(-alpha, -beta, depth - 1, WPt, WNt, WBt, WRt, WQt, WKt, BPt, BNt, BBt, BRt, BQt, BKt, EPt, CWKt, CWQt, CBKt, CBQt, !WhiteToMove);
                    if(score>alpha)
                    {
                        bestValueIndex = i;
                        alpha = score;
                    }
                }
                if ((score != UInt32.MinValue && (score > bestValue)))
                {
                    if (score >= beta)
                    {
                        return score;
                    }
                    bestValue = score;
                    if (Math.Abs(bestValue) == 30000)
                    {
                        return bestValue;
                    }
                }
            }
            return bestValue;
        }
        public static string getLegalMoves(string moves, ulong WP, ulong WN, ulong WB, ulong WR, ulong WQ, ulong WK, ulong BP, ulong BN, ulong BB, ulong BR, ulong BQ, ulong BK, ulong EP, bool CWK, bool CWQ, bool CBK, bool CBQ, bool WhiteToMove)
        {
            string legalMoves="";
            for (int i = 0; i < moves.Length; i += 4)
            {
                ulong WPt = Moves.makeMove(WP, moves.Substring(i, 4), 'P'), WNt = Moves.makeMove(WN, moves.Substring(i, 4), 'N'),
                        WBt = Moves.makeMove(WB, moves.Substring(i, 4), 'B'), WRt = Moves.makeMove(WR, moves.Substring(i, 4), 'R'),
                        WQt = Moves.makeMove(WQ, moves.Substring(i, 4), 'Q'), WKt = Moves.makeMove(WK, moves.Substring(i, 4), 'K'),
                        BPt = Moves.makeMove(BP, moves.Substring(i, 4), 'p'), BNt = Moves.makeMove(BN, moves.Substring(i, 4), 'n'),
                        BBt = Moves.makeMove(BB, moves.Substring(i, 4), 'b'), BRt = Moves.makeMove(BR, moves.Substring(i, 4), 'r'),
                        BQt = Moves.makeMove(BQ, moves.Substring(i, 4), 'q'), BKt = Moves.makeMove(BK, moves.Substring(i, 4), 'k');
                WRt = Moves.makeMoveCastle(WRt, WK | BK, moves.Substring(i, 4), 'R');
                BRt = Moves.makeMoveCastle(BRt, WK | BK, moves.Substring(i, 4), 'r');
                if (((WKt & Moves.unSafeForWhite(WPt, WNt, WBt, WRt, WQt, WKt, BPt, BNt, BBt, BRt, BQt, BKt)) == 0 && WhiteToMove) ||
                        ((BKt & Moves.unSafeForBlack(WPt, WNt, WBt, WRt, WQt, WKt, BPt, BNt, BBt, BRt, BQt, BKt)) == 0 && !WhiteToMove))
                {
                    legalMoves+=moves.Substring(i,4);
                }
            }
            return legalMoves;
        }
        public static int negamaxWithAlphaBeta(int alpha, int beta, int depth, ulong WP, ulong WN, ulong WB, ulong WR, ulong WQ, ulong WK, ulong BP, ulong BN, ulong BB, ulong BR, ulong BQ, ulong BK, ulong EP, bool CWK, bool CWQ, bool CBK, bool CBQ, bool computerTurn)
        {
            nodeProcessed++;
            if (depth == 0) return Evalue.evalue(WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK);
            string moves;
            if(computerTurn)
            {
                moves = Moves.possibleMovesW(WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK, EP, CWK, CWQ, CBK, CBQ);
            }
            else
            {
                moves=Moves.possibleMovesB(WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK, EP, CWK, CWQ, CBK, CBQ);
            }
            string legalMove = getLegalMoves(moves, WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK, EP, CWK, CWQ, CBK, CBQ, computerTurn);
            if(legalMove.Length==0)
            {
                return computerTurn ? -30000 : 30000;
            }
            int bestValue = Int32.MinValue;
            for(int i=0;i<legalMove.Length;i+=4)
            {
                ulong WPt = Moves.makeMove(WP, moves.Substring(i, 4), 'P'), WNt = Moves.makeMove(WN, moves.Substring(i, 4), 'N'),
                            WBt = Moves.makeMove(WB, moves.Substring(i, 4), 'B'), WRt = Moves.makeMove(WR, moves.Substring(i, 4), 'R'),
                            WQt = Moves.makeMove(WQ, moves.Substring(i, 4), 'Q'), WKt = Moves.makeMove(WK, moves.Substring(i, 4), 'K'),
                            BPt = Moves.makeMove(BP, moves.Substring(i, 4), 'p'), BNt = Moves.makeMove(BN, moves.Substring(i, 4), 'n'),
                            BBt = Moves.makeMove(BB, moves.Substring(i, 4), 'b'), BRt = Moves.makeMove(BR, moves.Substring(i, 4), 'r'),
                            BQt = Moves.makeMove(BQ, moves.Substring(i, 4), 'q'), BKt = Moves.makeMove(BK, moves.Substring(i, 4), 'k'),
                            EPt = Moves.makeMoveEP(WP | BP, moves.Substring(i, 4));
                WRt = Moves.makeMoveCastle(WRt, WK | BK, moves.Substring(i, 4), 'R');
                BRt = Moves.makeMoveCastle(BRt, WK | BK, moves.Substring(i, 4), 'r');
                bool CWKt = CWK, CWQt = CWQ, CBKt = CBK, CBQt = CBQ;
                if (char.IsDigit(moves[i + 3]))
                {//'regular' move
                    int start = (int)(char.GetNumericValue(moves[i]) * 8) + (int)(char.GetNumericValue(moves[i + 1]));
                    /*if (((1ul<<start)&(WP|BP))!=0) {
                        if (Math.abs(moves[i)-moves[i+2))==2) {
                            EPt=Moves.FileMasks8[moves[i+1)-'0'];
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
                int childValue = -negamaxWithAlphaBeta(-alpha, -beta, depth - 1, WPt, WNt, WBt, WRt, WQt, WKt, BPt, BNt, BBt, BRt, BQt, BKt, EPt, CWKt, CWQt, CBKt, CBQt, !computerTurn);
                bestValue = Math.Max(bestValue, childValue);
                alpha = Math.Max(alpha, childValue);
                if (alpha >= beta) break;
            }
            return bestValue;
        }
        public static string searchForBestMove(ulong WP, ulong WN, ulong WB, ulong WR, ulong WQ, ulong WK, ulong BP, ulong BN, ulong BB, ulong BR, ulong BQ, ulong BK, ulong EP, bool CWK, bool CWQ, bool CBK, bool CBQ,bool turn)
        {
            string bestMove="";
            string moves;
            int alpha = Int32.MinValue, beta = Int32.MaxValue;
            if(turn)
            {
                moves = Moves.possibleMovesW(WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK, EP, CWK, CWQ, CBK, CBQ);
            }
            else
            {
                moves=Moves.possibleMovesB(WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK, EP, CWK, CWQ, CBK, CBQ);
            }
            string legalMove = getLegalMoves(moves, WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK, EP, CWK, CWQ, CBK, CBQ, turn);
            if (legalMove.Length == 0)
            {
                return null;
            }
            int bestValue = Int32.MinValue;
            int bestMoveIndex=-1;
            for (int i = 0; i < legalMove.Length; i += 4)
            {
                Console.WriteLine(nodeProcessed);
                Console.WriteLine(bestValue);
                Console.WriteLine(bestMove);
                ulong WPt = Moves.makeMove(WP, moves.Substring(i, 4), 'P'), WNt = Moves.makeMove(WN, moves.Substring(i, 4), 'N'),
                            WBt = Moves.makeMove(WB, moves.Substring(i, 4), 'B'), WRt = Moves.makeMove(WR, moves.Substring(i, 4), 'R'),
                            WQt = Moves.makeMove(WQ, moves.Substring(i, 4), 'Q'), WKt = Moves.makeMove(WK, moves.Substring(i, 4), 'K'),
                            BPt = Moves.makeMove(BP, moves.Substring(i, 4), 'p'), BNt = Moves.makeMove(BN, moves.Substring(i, 4), 'n'),
                            BBt = Moves.makeMove(BB, moves.Substring(i, 4), 'b'), BRt = Moves.makeMove(BR, moves.Substring(i, 4), 'r'),
                            BQt = Moves.makeMove(BQ, moves.Substring(i, 4), 'q'), BKt = Moves.makeMove(BK, moves.Substring(i, 4), 'k'),
                            EPt = Moves.makeMoveEP(WP | BP, moves.Substring(i, 4));
                WRt = Moves.makeMoveCastle(WRt, WK | BK, moves.Substring(i, 4), 'R');
                BRt = Moves.makeMoveCastle(BRt, WK | BK, moves.Substring(i, 4), 'r');
                bool CWKt = CWK, CWQt = CWQ, CBKt = CBK, CBQt = CBQ;
                if (char.IsDigit(moves[i + 3]))
                {//'regular' move
                    int start = (int)(char.GetNumericValue(moves[i]) * 8) + (int)(char.GetNumericValue(moves[i + 1]));
                    /*if (((1ul<<start)&(WP|BP))!=0) {
                        if (Math.abs(moves[i)-moves[i+2))==2) {
                            EPt=Moves.FileMasks8[moves[i+1)-'0'];
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
                int childValue = -negamaxWithAlphaBeta(-alpha, -beta, 6, WPt, WNt, WBt, WRt, WQt, WKt, BPt, BNt, BBt, BRt, BQt, BKt, EPt, CWKt, CWQt, CBKt, CBQt, !turn);
                if(bestValue<childValue)
                {
                    bestValue = childValue;
                    bestMove = moves.Substring(i, 4);
                }
                alpha = Math.Max(alpha, childValue);
                if (alpha >= beta) break;
            }
            return bestMove;
        }
    }
}
