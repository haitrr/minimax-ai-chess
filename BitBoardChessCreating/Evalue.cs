using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitBoardChessCreating
{
    class Evalue
    {
        private static int[,] valueMaxtrixBishop = new int[8, 8]{
{310,320,320,320,320,320,320,310},
{320,330,330,330,330,330,330,320},
{320,330,335,340,340,335,330,320},
{320,335,335,340,340,335,335,320},
{320,330,340,340,340,340,330,320},
{320,340,340,340,340,340,340,320},
{320,335,330,330,330,330,335,320},
{310,320,320,320,320,320,320,310}};
        private static int[,] valueMaxtrixKing = new int[8, 8]{
{19970,19960,19960,19950,19950,19960,19960,19970},
{19970,19960,19960,19950,19950,19960,19960,19970},
{19970,19960,19960,19950,19950,19960,19960,19970},
{19970,19960,19960,19950,19950,19960,19960,19970},
{19980,19970,19970,19960,19960,19970,19970,19980},
{19990,19980,19980,19980,19980,19980,19980,19990},
{20020,20020,20000,20000,20000,20000,20020,20020},
{20020,20030,20010,20000,20000,20010,20030,20020}};
        private static int[,] valueMaxtrixPawn = new int[8, 8]{
{100,100,100,100,100,100,100,100},
{150,150,150,150,150,150,150,150},
{110,110,120,130,130,120,110,110},
{105,105,110,125,125,110,105,105},
{100,100,100,120,120,100,100,100},
{105,95,90,100,100,90,95,105},
{105,110,110,80,80,110,110,105},
{100,100,100,100,100,100,100,100}};
        private static int[,] valueMaxtrixKnight = new int[8, 8]{
{270,280,290,290,290,290,280,270},
{280,300,320,320,320,320,300,280},
{290,320,330,335,335,330,320,290},
{290,325,335,340,340,335,325,290},
{290,320,335,340,340,335,320,290},
{290,325,330,335,335,330,325,290},
{280,300,320,325,325,320,300,280},
{270,280,290,290,290,290,280,270}};
        private static int[,] valueMaxtrixQueen = new int[8, 8]{
{880,890,890,895,895,890,890,880},
{890,900,900,900,900,900,900,890},
{890,900,905,905,905,905,900,890},
{895,900,905,905,905,905,900,895},
{900,900,905,905,905,905,900,895},
{890,905,905,905,905,905,900,890},
{890,900,905,900,900,900,900,890},
{880,890,890,895,895,890,890,880}};
        private static int[,] valueMaxtrixRook = new int[8, 8]{
{500,500,500,500,500,500,500,500},
{505,510,510,510,510,510,510,505},
{495,500,500,500,500,500,500,495},
{495,500,500,500,500,500,500,495},
{495,500,500,500,500,500,500,495},
{495,500,500,500,500,500,500,495},
{495,500,500,500,500,500,500,495},
{500,500,500,505,505,500,500,500}};
        public static int evalue(ulong WP, ulong WN, ulong WB, ulong WR, ulong WQ, ulong WK, ulong BP, ulong BN, ulong BB, ulong BR, ulong BQ, ulong BK)
        {
            //int value=0;
            //ulong whitePawn = WP & ~(WP - 1);
            //while(whitePawn!=0)
            //{
            //    int index=BitHelper.getNumberOfTrailingZeros(whitePawn);
            //    value += valueMaxtrixPawn[index/8,index%8];
            //}
            //ulong whiteRook = WP & ~(WP - 1);
            //while (whiteRook != 0)
            //{
            //    int index = BitHelper.getNumberOfTrailingZeros(whiteRook);
            //    value += valueMaxtrixPawn[index / 8, index % 8];
            //}
            //ulong whiteBishop  = WP & ~(WP - 1);
            //while (whiteBishop != 0)
            //{
            //    int index = BitHelper.getNumberOfTrailingZeros(whiteBishop);
            //    value += valueMaxtrixPawn[index / 8, index % 8];
            //}
            //ulong whiteQueen = WP & ~(WP - 1);
            //while (whiteQueen != 0)
            //{
            //    int index = BitHelper.getNumberOfTrailingZeros(whiteQueen);
            //    value += valueMaxtrixPawn[index / 8, index % 8];
            //}
            //ulong whiteKing = WP & ~(WP - 1);
            //while (whiteKing != 0)
            //{
            //    int index = BitHelper.getNumberOfTrailingZeros(whiteKing);
            //    value += valueMaxtrixPawn[index / 8, index % 8];
            //}
            //ulong whiteKnight = WP & ~(WP - 1);
            //while (whiteKnight != 0)
            //{
            //    int index = BitHelper.getNumberOfTrailingZeros(whiteKnight);
            //    value += valueMaxtrixPawn[index / 8, index % 8];
            //}
            //ulong blackPawn = WP & ~(WP - 1);
            //while (blackPawn != 0)
            //{
            //    int index = BitHelper.getNumberOfTrailingZeros(blackPawn);
            //    value += valueMaxtrixPawn[index / 8, index % 8];
            //}
            //ulong blackRook = WP & ~(WP - 1);
            //while (blackRook != 0)
            //{
            //    int index = BitHelper.getNumberOfTrailingZeros(blackRook);
            //    value += valueMaxtrixPawn[index / 8, index % 8];
            //}
            //ulong blackBishop = WP & ~(WP - 1);
            //while (blackBishop != 0)
            //{
            //    int index = BitHelper.getNumberOfTrailingZeros(blackBishop);
            //    value += valueMaxtrixPawn[index / 8, index % 8];
            //}
            //ulong blackQueen = WP & ~(WP - 1);
            //while (blackQueen != 0)
            //{
            //    int index = BitHelper.getNumberOfTrailingZeros(blackQueen);
            //    value += valueMaxtrixPawn[index / 8, index % 8];
            //}
            //ulong blackKing = WP & ~(WP - 1);
            //while (blackKing != 0)
            //{
            //    int index = BitHelper.getNumberOfTrailingZeros(blackKing);
            //    value += valueMaxtrixPawn[index / 8, index % 8];
            //}
            //ulong blackKnight = WP & ~(WP - 1);
            //while (blackKnight != 0)
            //{
            //    int index = BitHelper.getNumberOfTrailingZeros(blackKnight);
            //    value += valueMaxtrixPawn[index / 8, index % 8];
            //}
            //return value;
            return 0;
        }
    }
}
