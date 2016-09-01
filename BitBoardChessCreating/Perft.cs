using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitBoardChessCreating
{
    public class Perft
    {
        public static string moveToAlgebra(string move)
        {
            string movestring = "";
            movestring += "" + (char)(move[1] + 49);
            movestring += "" + ('8' - move[0]);
            movestring += "" + (char)(move[3] + 49);
            movestring += "" + ('8' - move[2]);
            return movestring;
        }
        public static int perftTotalMoveCounter = 0;
        static int perftMoveCounter = 0;
        static int perftMaxDepth = 2;
        public static void perftRoot(ulong WP, ulong WN, ulong WB, ulong WR, ulong WQ, ulong WK, ulong BP, ulong BN, ulong BB, ulong BR, ulong BQ, ulong BK, ulong EP, bool CWK, bool CWQ, bool CBK, bool CBQ, bool WhiteToMove, int depth)
    {
        string moves;
        if (WhiteToMove) {
            moves=Moves.possibleMovesW(WP,WN,WB,WR,WQ,WK,BP,BN,BB,BR,BQ,BK,EP,CWK,CWQ,CBK,CBQ);
        } else {
            moves=Moves.possibleMovesB(WP,WN,WB,WR,WQ,WK,BP,BN,BB,BR,BQ,BK,EP,CWK,CWQ,CBK,CBQ);
        }
        for (int i=0;i<moves.Length;i+=4) {
            ulong WPt=Moves.makeMove(WP, moves.Substring(i,4), 'P'), WNt=Moves.makeMove(WN, moves.Substring(i,4), 'N'),
                    WBt=Moves.makeMove(WB, moves.Substring(i,4), 'B'), WRt=Moves.makeMove(WR, moves.Substring(i,4), 'R'),
                    WQt=Moves.makeMove(WQ, moves.Substring(i,4), 'Q'), WKt=Moves.makeMove(WK, moves.Substring(i,4), 'K'),
                    BPt=Moves.makeMove(BP, moves.Substring(i,4), 'p'), BNt=Moves.makeMove(BN, moves.Substring(i,4), 'n'),
                    BBt=Moves.makeMove(BB, moves.Substring(i,4), 'b'), BRt=Moves.makeMove(BR, moves.Substring(i,4), 'r'),
                    BQt=Moves.makeMove(BQ, moves.Substring(i,4), 'q'), BKt=Moves.makeMove(BK, moves.Substring(i,4), 'k'),
                    EPt=Moves.makeMoveEP(WP|BP,moves.Substring(i,4));
            WRt=Moves.makeMoveCastle(WRt, WK|BK, moves.Substring(i,4), 'R');
            BRt=Moves.makeMoveCastle(BRt, WK|BK, moves.Substring(i,4), 'r');
            bool CWKt=CWK,CWQt=CWQ,CBKt=CBK,CBQt=CBQ;
            if (char.IsDigit(moves[i+3])) {//'regular' move
                int start = (int)(char.GetNumericValue(moves[i]) * 8) + (int)(char.GetNumericValue(moves[i + 1]));
                /*if (((1ul<<start)&(WP|BP))!=0) {
                    if (Math.abs(moves[i)-moves[i+2))==2) {
                        EPt=Moves.FileMasks8[moves[i+1)-'0'];
                    }
                }
                else */if (((1ul<<start)&WK)!=0) {CWKt=false; CWQt=false;}
                else if (((1ul<<start)&BK)!=0) {CBKt=false; CBQt=false;}
                else if (((1ul<<start)&WR&(1ul<<Moves.CASTLE_ROOKS[0]))!=0) {CWKt=false;}
                else if (((1ul<<start)&WR&(1ul<<Moves.CASTLE_ROOKS[1]))!=0) {CWQt=false;}
                else if (((1ul<<start)&BR&(1ul<<Moves.CASTLE_ROOKS[2]))!=0) {CBKt=false;}
                else if (((1ul<<start)&BR&1ul)!=0) {CBQt=false;}
            }
            if (((WKt&Moves.unSafeForWhite(WPt,WNt,WBt,WRt,WQt,WKt,BPt,BNt,BBt,BRt,BQt,BKt))==0 && WhiteToMove) ||
                    ((BKt&Moves.unSafeForBlack(WPt,WNt,WBt,WRt,WQt,WKt,BPt,BNt,BBt,BRt,BQt,BKt))==0 && !WhiteToMove)) {
                perft(WPt,WNt,WBt,WRt,WQt,WKt,BPt,BNt,BBt,BRt,BQt,BKt,EPt,CWKt,CWQt,CBKt,CBQt,!WhiteToMove,depth+1);
                Console.WriteLine(moveToAlgebra(moves.Substring(i,4))+" "+perftMoveCounter);
                perftTotalMoveCounter+=perftMoveCounter;
                perftMoveCounter=0;
            }
        }
    }
        public static void perft(ulong WP, ulong WN, ulong WB, ulong WR, ulong WQ, ulong WK, ulong BP, ulong BN, ulong BB, ulong BR, ulong BQ, ulong BK, ulong EP, bool CWK, bool CWQ, bool CBK, bool CBQ, bool WhiteToMove, int depth)
        {
            if (depth < perftMaxDepth)
            {
                string moves;
                if (WhiteToMove)
                {
                    moves = Moves.possibleMovesW(WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK, EP, CWK, CWQ, CBK, CBQ);
                }
                else
                {
                    moves = Moves.possibleMovesB(WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK, EP, CWK, CWQ, CBK, CBQ);
                }
                for (int i = 0; i < moves.Length; i += 4)
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
                    if (((WKt & Moves.unSafeForWhite(WPt, WNt, WBt, WRt, WQt, WKt, BPt, BNt, BBt, BRt, BQt, BKt)) == 0 && WhiteToMove) ||
                            ((BKt & Moves.unSafeForBlack(WPt, WNt, WBt, WRt, WQt, WKt, BPt, BNt, BBt, BRt, BQt, BKt)) == 0 && !WhiteToMove))
                    {
                        if (depth + 1 == perftMaxDepth) 
                        { perftMoveCounter++; }
                        perft(WPt, WNt, WBt, WRt, WQt, WKt, BPt, BNt, BBt, BRt, BQt, BKt, EPt, CWKt, CWQt, CBKt, CBQt, !WhiteToMove, depth + 1);
                    }
                }
            }
        }
    }
}
