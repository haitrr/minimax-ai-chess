using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitBoardChessCreating
{
    class Program
    {
        public static ulong WP = 0L, WN = 0L, WB = 0L, WR = 0L, WQ = 0L, WK = 0L, BP = 0L, BN = 0L, BB = 0L, BR = 0L, BQ = 0L, BK = 0L, EP = 0L;
        public static bool CWK = true, CWQ = true, CBK = true, CBQ = true, WhiteToMove = true;//true=castle is possible
        public static ulong UniversalWP = 0L, UniversalWN = 0L, UniversalWB = 0L, UniversalWR = 0L,
                UniversalWQ = 0L, UniversalWK = 0L, UniversalBP = 0L, UniversalBN = 0L,
                UniversalBB = 0L, UniversalBR = 0L, UniversalBQ = 0L, UniversalBK = 0L,
                UniversalEP = 0L;
        public static bool UniversalCastleWK = true, UniversalCastleWQ = true,
                UniversalCastleBK = true, UniversalCastleBQ = true;//true=castle is possible
        static void Main(string[] args)
        {
            //BoardGeneration.initiateStandardChess(ref WP,ref WN, ref WB, ref WR, ref WQ, ref WK,ref  BP,ref BN,ref BB,ref BR,ref BQ,ref BK);
            BoardGeneration.importFEN("r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R3K2R w KQkq -");
            //BoardGeneration.importFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
            BoardGeneration.drawArray(WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK);
            //Perft.perftRoot(WP,WN,WB,WR,WQ,WK,BP,BN,BB,BR,BQ,BK,EP,CWK,CWQ,CBK,CBQ,WhiteToMove,0);
            Console.WriteLine(Search.searchForBestMove(WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK, EP, CWK, CWQ, CBK, CBQ, true));
            //Console.WriteLine(Perft.perftTotalMoveCounter);
            Console.WriteLine(Search.nodeProcessed);
            Console.ReadLine();
        }
    }
}
