using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyChess
{
    class Evalue
    {
        private static int[,] valueMaxtrixWhiteBishop = new int[8, 8]{
{310,320,320,320,320,320,320,310},
{320,330,330,330,330,330,330,320},
{320,330,335,340,340,335,330,320},
{320,335,335,340,340,335,335,320},
{320,330,340,340,340,340,330,320},
{320,340,340,340,340,340,340,320},
{320,335,330,330,330,330,335,320},
{310,320,320,320,320,320,320,310}};
        private static int[,] valueMaxtrixWhiteKing = new int[8, 8]{
{9970,9960,9960,9950,9950,9960,9960,9970},
{9970,9960,9960,9950,9950,9960,9960,9970},
{9970,9960,9960,9950,9950,9960,9960,9970},
{9970,9960,9960,9950,9950,9960,9960,9970},
{9980,9970,9970,9960,9960,9970,9970,9980},
{9990,9980,9980,9980,9980,9980,9980,9990},
{10020,10020,10000,10000,10000,10000,10020,10020},
{10020,10030,10010,10000,10000,10010,10030,10020}};
        private static int[,] valueMaxtrixWhiteKingLateGame = new int[8, 8]{
{9950,9960,9970,9980,9980,9970,9960,9950},
{9970,9980,9990,10000,10000,9990,9980,9970},
{9970,9990,10020,10030,10030,10020,9990,9970},
{9970,9990,10030,10040,10040,10030,9990,9970},
{9970,9990,10030,10040,10040,10030,9990,9970},
{9970,9990,10020,10030,10030,10020,9990,9970},
{9970,9970,10000,10000,10000,10000,9970,9970},
{9950,9970,9970,9970,9970,9970,9970,9950}};
        private static int[,] valueMaxtrixWhitePawn = new int[8, 8]{
{100,100,100,100,100,100,100,100},
{150,150,150,150,150,150,150,150},
{110,110,120,130,130,120,110,110},
{105,105,110,127,127,110,105,105},
{100,100,100,125,125,100,100,100},
{105,95,90,100,100,90,95,105},
{105,110,110,75,75,110,110,105},
{100,100,100,100,100,100,100,100}};
        private static int[,] valueMaxtrixWhiteKnight = new int[8, 8]{
{270,280,290,290,290,290,280,270},
{280,300,320,320,320,320,300,280},
{290,320,330,335,335,330,320,290},
{290,325,335,340,340,335,325,290},
{290,320,335,340,340,335,320,290},
{290,325,330,335,335,330,325,290},
{280,300,320,325,325,320,300,280},
{270,280,290,290,290,290,280,270}};
        private static int[,] valueMaxtrixWhiteQueen = new int[8, 8]{
{880,890,890,895,895,890,890,880},
{890,900,900,900,900,900,900,890},
{890,900,905,905,905,905,900,890},
{895,900,905,905,905,905,900,895},
{900,900,905,905,905,905,900,895},
{890,905,905,905,905,905,900,890},
{890,900,905,900,900,900,900,890},
{880,890,890,895,895,890,890,880}};
        private static int[,] valueMaxtrixWhiteRook = new int[8, 8]{
{500,500,500,500,500,500,500,500},
{505,510,510,510,510,510,510,505},
{495,500,500,500,500,500,500,495},
{495,500,500,500,500,500,500,495},
{495,500,500,500,500,500,500,495},
{495,500,500,500,500,500,500,495},
{495,500,500,500,500,500,500,495},
{500,500,500,505,505,500,500,500}};
        private static int[,] valueMaxtrixBlackBishop = mirrorMaxtrix(valueMaxtrixWhiteBishop);
        private static int[,] valueMaxtrixBlackKing = mirrorMaxtrix(valueMaxtrixWhiteKing);
        private static int[,] valueMaxtrixBlackPawn = mirrorMaxtrix(valueMaxtrixWhitePawn);
        private static int[,] valueMaxtrixBlackKnight = mirrorMaxtrix(valueMaxtrixWhiteKnight);
        private static int[,] valueMaxtrixBlackQueen = mirrorMaxtrix(valueMaxtrixWhiteQueen);
        private static int[,] valueMaxtrixBlackRook = mirrorMaxtrix(valueMaxtrixWhiteRook);
        private static int[,] valueMaxtrixBlackKingLateGame = mirrorMaxtrix(valueMaxtrixWhiteKingLateGame);

        public static int evalue(ulong WP, ulong WN, ulong WB, ulong WR, ulong WQ, ulong WK, ulong BP, ulong BN, ulong BB, ulong BR, ulong BQ, ulong BK, bool white)
        {
            int value = 0;
            ulong whitePawn = WP & ~(WP - 1);
            while (whitePawn != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(whitePawn);
                value += valueMaxtrixWhitePawn[index / 8, index % 8];
                WP &= ~whitePawn;
                whitePawn = WP & ~(WP - 1);
            }
            ulong whiteRook = WR & ~(WR - 1);
            while (whiteRook != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(whiteRook);
                value += valueMaxtrixWhiteRook[index / 8, index % 8];
                WR &= ~whiteRook;
                whiteRook = WR & ~(WR - 1);
            }
            ulong whiteBishop = WB & ~(WB - 1);
            while (whiteBishop != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(whiteBishop);
                value += valueMaxtrixWhiteBishop[index / 8, index % 8];
                WB &= ~whiteBishop;
                whiteBishop = WB & ~(WB - 1);
            }
            ulong whiteQueen = WQ & ~(WQ - 1);
            while (whiteQueen != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(whiteQueen);
                value += valueMaxtrixWhiteQueen[index / 8, index % 8];
                WQ &= ~whiteQueen;
                whiteQueen = WQ & ~(WQ - 1);
            }
            ulong whiteKing = WK & ~(WK - 1);
            while (whiteKing != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(whiteKing);
                if (ChessBoard.lategame) value += valueMaxtrixWhiteKingLateGame[index / 8, index % 8];
                else
                value += valueMaxtrixWhiteKing[index / 8, index % 8];
                WK &= ~whiteKing;
                whiteKing = WK & ~(WK - 1);
            }
            ulong whiteKnight = WN & ~(WN - 1);
            while (whiteKnight != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(whiteKnight);
                value += valueMaxtrixWhiteKnight[index / 8, index % 8];
                WN &= ~whiteKnight;
                whiteKnight = WN & ~(WN - 1);
            }
            ulong blackPawn = BP & ~(BP - 1);
            while (blackPawn != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(blackPawn);
                value -= valueMaxtrixBlackPawn[index / 8, index % 8];
                BP &= ~blackPawn;
                blackPawn = BP & ~(BP - 1);
            }
            ulong blackRook = BR & ~(BR - 1);
            while (blackRook != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(blackRook);
                value -= valueMaxtrixBlackRook[index / 8, index % 8];
                BR &= ~blackRook;
                blackRook = BR & ~(BR - 1);
            }
            ulong blackBishop = BB & ~(BB - 1);
            while (blackBishop != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(blackBishop);
                value -= valueMaxtrixBlackBishop[index / 8, index % 8];
                BB &= ~blackBishop;
                blackBishop = BB & ~(BB - 1);
            }
            ulong blackQueen = BQ & ~(BQ - 1);
            while (blackQueen != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(blackQueen);
                value -= valueMaxtrixBlackQueen[index / 8, index % 8];
                BQ &= ~blackQueen;
                blackQueen = BQ & ~(BQ - 1);
            }
            ulong blackKing = BK & ~(BK - 1);
            while (blackKing != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(blackKing);
                if (ChessBoard.lategame) value -= valueMaxtrixBlackKingLateGame[index / 8, index % 8];
                else
                value -= valueMaxtrixBlackKing[index / 8, index % 8];
                BK &= ~blackKing;
                blackKing = BK & ~(BK - 1);
            }
            ulong blackKnight = BN & ~(BN - 1);
            while (blackKnight != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(blackKnight);
                value -= valueMaxtrixBlackKnight[index / 8, index % 8];
                BN &= ~blackKnight;
                blackKnight = BN & ~(BN - 1);
            }

            if (white) return value; else return -value;
        }
        public static int[,] mirrorMaxtrix(int[,] maxtrix)
        {
            int[,] mirror = new int[8, 8];
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    mirror[i, j] = maxtrix[7 - i, j];
                }
            return mirror;
        }
    }
}
