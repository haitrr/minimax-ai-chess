using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyChess
{
    class Moves
    {
        static ulong FILE_A = 72340172838076673;
        static ulong FILE_H = 9259542123273814144;
        static ulong FILE_AB = 217020518514230019;
        static ulong FILE_GH = 13889313184910721216;
        static ulong RANK_1 = 18374686479671623680;
        static ulong RANK_4 = 1095216660480;
        static ulong RANK_5 = 4278190080;
        static ulong RANK_8 = 255;
        static ulong CENTRE = 103481868288;
        static ulong EXTENDED_CENTRE = 66229406269440;
        static ulong KING_SIDE = 2238024097177997000;
        static ulong QUEEN_SIDE = 1085102592571150095;
        static ulong KING_SPAN = 460039;
        static ulong KNIGHT_SPAN = 43234889994;
        static ulong NOT_MY_PIECES;
        static ulong MY_PIECES;
        static ulong ALL_MY_PIECES;
        static ulong EMPTY;
        static ulong enmk;
        static ulong OCCUPIED;
        static ulong OCCUPIED_H;
        static ulong OCCUPIED_D;
        static ulong OCCUPIED_AD;
        public static int[] CASTLE_ROOKS = new int[4] { 63, 56, 7, 0 };
        public static ulong[] RankMasks8 = new ulong[8]/*from rank1 to rank8*/
         {
            0xFFul, 0xFF00ul, 0xFF0000ul, 0xFF000000ul, 0xFF00000000ul, 0xFF0000000000ul, 0xFF000000000000ul, 0xFF00000000000000ul
         };
        public static ulong[] FileMasks8 = new ulong[8]/*from fileA to FileH*/
         {
            0x101010101010101ul, 0x202020202020202ul, 0x404040404040404ul, 0x808080808080808ul,
             0x1010101010101010ul, 0x2020202020202020ul, 0x4040404040404040ul, 0x8080808080808080ul
         };
        static ulong[] DiagonalMasks8 = new ulong[15]/*from top left to bottom right*/
    {
	0x1ul, 0x102ul, 0x10204ul, 0x1020408ul, 0x102040810ul, 0x10204081020ul, 0x1020408102040ul,
	0x102040810204080ul, 0x204081020408000ul, 0x408102040800000ul, 0x810204080000000ul,
	0x1020408000000000ul, 0x2040800000000000ul, 0x4080000000000000ul, 0x8000000000000000ul
    };
        static ulong[] AntiDiagonalMasks8 = new ulong[15]/*from top right to bottom left*/
    {
	0x80ul, 0x8040ul, 0x804020ul, 0x80402010ul, 0x8040201008ul, 0x804020100804ul, 0x80402010080402ul,
	0x8040201008040201ul, 0x4020100804020100ul, 0x2010080402010000ul, 0x1008040201000000ul,
	0x804020100000000ul, 0x402010000000000ul, 0x201000000000000ul, 0x100000000000000ul
    };
        static ulong HAndVMoves(int s)
        {
            ////REMINDER: requires OCCUPIED to be up to date
            //ulong binaryS = 1ul << s;
            //ulong possibilitiesHorizontal = (OCCUPIED - 2 * binaryS) ^ BitHelper.reverse(BitHelper.reverse(OCCUPIED) - 2 * BitHelper.reverse(binaryS));
            ////binaryS = 1ul << ((((s >> 3) | (s << 3)) & 63) ^ 56);
            //////binaryS = BitHelper.rotate90clockwise(binaryS);
            //ulong possibilitiesVertical = ((OCCUPIED & FileMasks8[s % 8]) - (2 * binaryS)) ^ BitHelper.reverse(BitHelper.reverse(OCCUPIED & FileMasks8[s % 8]) - (2 * BitHelper.reverse(binaryS)));
            ////ulong possibilitiesVertical = (OCCUPIED_H - 2 * binaryS) ^ BitHelper.reverse(BitHelper.reverse(OCCUPIED_H) - 2 * BitHelper.reverse(binaryS));
            ////return (possibilitiesHorizontal & RankMasks8[s / 8]) | (BitHelper.rotate90antiClockwise(possibilitiesVertical) & FileMasks8[s % 8]);
            //return (possibilitiesHorizontal & RankMasks8[s / 8]) | (possibilitiesVertical & FileMasks8[s % 8]);
            ulong bbBlockers = OCCUPIED & occupancyMaskRook[s];

            int databaseIndex = (int)((bbBlockers * magicNumberRook[s]) >> magicNumberShiftsRook[s]);

            ulong bbMoveSquares = magicMovesRook[s, databaseIndex]; //~ALL_MY_PIECES;
            //if (bbMoveSquares == 0) return 578721382705530880;
            return bbMoveSquares;
        }
        static ulong HAndVMoves2(int s)
        {
            ////REMINDER: requires OCCUPIED to be up to date
            ulong binaryS = 1ul << s;
            ulong possibilitiesHorizontal = (OCCUPIED - 2 * binaryS) ^ BitHelper.reverse(BitHelper.reverse(OCCUPIED) - 2 * BitHelper.reverse(binaryS));
            ////binaryS = 1ul << ((((s >> 3) | (s << 3)) & 63) ^ 56);
            //////binaryS = BitHelper.rotate90clockwise(binaryS);
            ulong possibilitiesVertical = ((OCCUPIED & FileMasks8[s % 8]) - (2 * binaryS)) ^ BitHelper.reverse(BitHelper.reverse(OCCUPIED & FileMasks8[s % 8]) - (2 * BitHelper.reverse(binaryS)));
            ////ulong possibilitiesVertical = (OCCUPIED_H - 2 * binaryS) ^ BitHelper.reverse(BitHelper.reverse(OCCUPIED_H) - 2 * BitHelper.reverse(binaryS));
            ////return (possibilitiesHorizontal & RankMasks8[s / 8]) | (BitHelper.rotate90antiClockwise(possibilitiesVertical) & FileMasks8[s % 8]);
            return (possibilitiesHorizontal & RankMasks8[s / 8]) | (possibilitiesVertical & FileMasks8[s % 8]);
            //ulong bbBlockers = OCCUPIED & occupancyMaskRook[s];

            //int databaseIndex = (int)((bbBlockers * magicNumberRook[s]) >> magicNumberShiftsRook[s]);

            //ulong bbMoveSquares = magicMovesRook[s, databaseIndex]; //~ALL_MY_PIECES;
            //return bbMoveSquares;
        }
        static ulong DAndAntiDMoves(int s)
        {
            ulong bbBlockers = OCCUPIED & occupancyMaskBishop[s];

            int databaseIndex = (int)((bbBlockers * magicNumberBishop[s]) >> magicNumberShiftsBishop[s]);

            ulong bbMoveSquares = magicMovesBishop[s, databaseIndex];// &~ALL_MY_PIECES;
            return bbMoveSquares;
        }
        static ulong DAndAntiDMoves2(int s)
        {
            //////REMINDER: requires OCCUPIED to be up to date
            ulong binaryS = 1ul << s;
            ////ulong binaryS = 1ul << (((s * 0x20800000) >> 26) ^ 7);
            ulong possibilitiesDiagonal = ((OCCUPIED & DiagonalMasks8[(s / 8) + (s % 8)]) - (2 * binaryS)) ^ BitHelper.reverse(BitHelper.reverse(OCCUPIED & DiagonalMasks8[(s / 8) + (s % 8)]) - (2 * BitHelper.reverse(binaryS)));
            ////ulong possibilitiesDiagonal = (OCCUPIED_D - 2 * binaryS) ^ BitHelper.reverse(BitHelper.reverse(OCCUPIED_D) - 2 * BitHelper.reverse(binaryS));
            ////binaryS = 1ul<< ((s + 8*((s&7)^7)) & 63);
            ulong possibilitiesAntiDiagonal = ((OCCUPIED & AntiDiagonalMasks8[(s / 8) + 7 - (s % 8)]) - (2 * binaryS)) ^ BitHelper.reverse(BitHelper.reverse(OCCUPIED & AntiDiagonalMasks8[(s / 8) + 7 - (s % 8)]) - (2 * BitHelper.reverse(binaryS)));
            ////ulong possibilitiesAntiDiagonal=(OCCUPIED_AD - 2 * binaryS) ^ BitHelper.reverse(BitHelper.reverse(OCCUPIED_AD) - 2 * BitHelper.reverse(binaryS));
            ////return (BitHelper.rotate90antiClockwise(possibilitiesDiagonal) & DiagonalMasks8[(s / 8) + (s % 8)]) | (BitHelper.rotate90clockwise(possibilitiesAntiDiagonal) & AntiDiagonalMasks8[(s / 8) + 7- (s % 8)]);
            return (possibilitiesDiagonal & DiagonalMasks8[(s / 8) + (s % 8)]) | (possibilitiesAntiDiagonal & AntiDiagonalMasks8[(s / 8) + 7 - (s % 8)]);

        }
        public static ulong makeMove(ulong board, string move, char type)
        {
            if (char.IsDigit(move[3]))
            {//'regular' move
                int start = (move[0] - '0') * 8 + move[1] - '0';
                int end = (move[2] - '0') * 8 + move[3] - '0';
                if (((board >> start) & 1) == 1) { board &= ~(1ul << start); board |= (1ul << end); } else { board &= ~(1ul << end); }
            }
            else if (move[3] == 'P')
            {//pawn promotion
                int start, end;
                if (char.IsUpper(move[2]))
                {
                    start = BitHelper.getNumberOfTrailingZeros(FileMasks8[move[0] - '0'] & RankMasks8[1]);
                    end = BitHelper.getNumberOfTrailingZeros(FileMasks8[move[1] - '0'] & RankMasks8[0]);
                }
                else
                {
                    start = BitHelper.getNumberOfTrailingZeros(FileMasks8[move[0] - '0'] & RankMasks8[6]);
                    end = BitHelper.getNumberOfTrailingZeros(FileMasks8[move[1] - '0'] & RankMasks8[7]);
                }
                if (type == move[2]) { board |= (1ul << end); } else { board &= ~(1ul << start); board &= ~(1ul << end); }
            }
            else if (move[3] == 'E')
            {//en passant
                int start, end;
                if (move[2] == 'W')
                {
                    start = BitHelper.getNumberOfTrailingZeros(FileMasks8[move[0] - '0'] & RankMasks8[3]);
                    end = BitHelper.getNumberOfTrailingZeros(FileMasks8[move[1] - '0'] & RankMasks8[2]);
                    board &= ~(FileMasks8[move[1] - '0'] & RankMasks8[3]);
                }
                else
                {
                    start = BitHelper.getNumberOfTrailingZeros(FileMasks8[move[0] - '0'] & RankMasks8[4]);
                    end = BitHelper.getNumberOfTrailingZeros(FileMasks8[move[1] - '0'] & RankMasks8[5]);
                    board &= ~(FileMasks8[move[1] - '0'] & RankMasks8[4]);
                }
                if (((board >> start) & 1) == 1) { board &= ~(1ul << start); board |= (1ul << end); }
            }
            else
            {
                Console.WriteLine("ERROR: Invalid move type");
            }
            return board;
        }
        public static ulong makeMoveCastle(ulong rookBoard, ulong kingBoard, string move, char type)
        {
            int start = (move[0] - '0') * 8 + move[1] - '0';
            if ((((kingBoard >> start) & 1) == 1) && (("0402".Equals(move)) || ("0406".Equals(move)) || ("7472".Equals(move)) || ("7476".Equals(move))))
            {//'regular' move
                if (type == 'R')
                {
                    switch (move)
                    {
                        case "7472": rookBoard &= ~(1ul << CASTLE_ROOKS[1]); rookBoard |= (1ul << (CASTLE_ROOKS[1] + 3));
                            break;
                        case "7476": rookBoard &= ~(1ul << CASTLE_ROOKS[0]); rookBoard |= (1ul << (CASTLE_ROOKS[0] - 2));
                            break;
                    }
                }
                else
                {
                    switch (move)
                    {
                        case "0402": rookBoard &= ~(1ul << CASTLE_ROOKS[3]); rookBoard |= (1ul << (CASTLE_ROOKS[3] + 3));
                            break;
                        case "0406": rookBoard &= ~(1ul << CASTLE_ROOKS[2]); rookBoard |= (1ul << (CASTLE_ROOKS[2] - 2));
                            break;
                    }
                }
            }
            return rookBoard;
        }
        public static ulong makeMoveEP(ulong board, string move)
        {
            if (char.IsDigit(move[3]))
            {
                int start = (move[0] - '0') * 8 + move[1] - '0';
                if ((Math.Abs(move[0] - move[2]) == 2) && (((board >> start) & 1) == 1))
                {//pawn double push
                    return FileMasks8[move[1] - '0'];
                }
            }
            return 0;
        }
        public static string possibleMovesW(ulong WP, ulong WN, ulong WB, ulong WR, ulong WQ, ulong WK, ulong BP, ulong BN, ulong BB, ulong BR, ulong BQ, ulong BK, ulong EP, bool CWK, bool CWQ, bool CBK, bool CBQ)
        {
            //enmk = BK;
            NOT_MY_PIECES = ~(WP | WN | WB | WR | WQ | WK | BK);//added BK to avoid illegal capture
            MY_PIECES = WP | WN | WB | WR | WQ;//omitted WK to avoid illegal capture
            //ALL_MY_PIECES = WP | WN | WB | WR | WQ | WK;
            OCCUPIED = WP | WN | WB | WR | WQ | WK | BP | BN | BB | BR | BQ | BK;
            //OCCUPIED_H = BitHelper.rotate90clockwise(OCCUPIED);
            //OCCUPIED_D = BitHelper.pseudoRotate45clockwise(OCCUPIED);
            //OCCUPIED_AD = BitHelper.pseudoRotate45antiClockwise(OCCUPIED);
            EMPTY = ~OCCUPIED;
            StringBuilder list = new StringBuilder();
            list.Append(possibleWP(WP, BP, EP));
            list.Append(possibleN(OCCUPIED, WN));
            list.Append(possibleB(OCCUPIED, WB));
            list.Append(possibleR(OCCUPIED, WR));
            list.Append(possibleQ(OCCUPIED, WQ));
            list.Append(possibleK(OCCUPIED, WK));
            list.Append(possibleCW(WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK, CWK, CWQ));
            return list.ToString();
        }
        public static string possibleMovesB(ulong WP, ulong WN, ulong WB, ulong WR, ulong WQ, ulong WK, ulong BP, ulong BN, ulong BB, ulong BR, ulong BQ, ulong BK, ulong EP, bool CWK, bool CWQ, bool CBK, bool CBQ)
        {
            NOT_MY_PIECES = ~(BP | BN | BB | BR | BQ | BK | WK);//added WK to avoid illegal capture
            MY_PIECES = BP | BN | BB | BR | BQ;//omitted BK to avoid illegal capture
            OCCUPIED = WP | WN | WB | WR | WQ | WK | BP | BN | BB | BR | BQ | BK;
            //OCCUPIED_H = BitHelper.rotate90clockwise(OCCUPIED);
            //OCCUPIED_D = BitHelper.pseudoRotate45clockwise(OCCUPIED);
            //OCCUPIED_AD = BitHelper.pseudoRotate45antiClockwise(OCCUPIED);
            EMPTY = ~OCCUPIED;
            StringBuilder list = new StringBuilder();
                    list.Append(possibleBP(BP, WP, EP) );
                    list.Append(possibleN(OCCUPIED, BN) );
                    list.Append(possibleB(OCCUPIED, BB) );
                    list.Append(possibleR(OCCUPIED, BR) );
                    list.Append(possibleQ(OCCUPIED, BQ) );
                    list.Append(possibleK(OCCUPIED, BK) );
                    list.Append(possibleCB(WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK, CBK, CBQ));
            return list.ToString();
        }
        public static string possibleWP(ulong WP, ulong BP, ulong EP)
        {
            StringBuilder list =new StringBuilder();
            //x1,y1,x2,y2
            ulong PAWN_MOVES = (WP >> 7) & NOT_MY_PIECES & OCCUPIED & ~RANK_8 & ~FILE_A;//capture right
            ulong possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            while (possibility != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(possibility);
                list.Append(index / 8 + 1).Append(index % 8 - 1).Append(index / 8).Append(index % 8);
                PAWN_MOVES &= ~possibility;
                possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            }
            PAWN_MOVES = (WP >> 9) & NOT_MY_PIECES & OCCUPIED & ~RANK_8 & ~FILE_H;//capture left
            possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            while (possibility != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(possibility);
                list.Append(index / 8 + 1).Append(index % 8 + 1).Append(index / 8).Append(index % 8);
                PAWN_MOVES &= ~possibility;
                possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            }
            PAWN_MOVES = (WP >> 8) & EMPTY & ~RANK_8;//move 1 forward
            possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            while (possibility != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(possibility);
                list.Append(index / 8 + 1).Append(index % 8).Append(index / 8).Append(index % 8);
                PAWN_MOVES &= ~possibility;
                possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            }
            PAWN_MOVES = (WP >> 16) & EMPTY & (EMPTY >> 8) & RANK_4;//move 2 forward
            possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            while (possibility != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(possibility);
                list.Append(index / 8 + 2).Append(index % 8).Append(index / 8).Append(index % 8);
                PAWN_MOVES &= ~possibility;
                possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            }
            //y1,y2,Promotion Type,"P"
            PAWN_MOVES = (WP >> 7) & NOT_MY_PIECES & OCCUPIED & RANK_8 & ~FILE_A;//pawn promotion by capture right
            possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            while (possibility != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(possibility);
                list.Append(index % 8 - 1).Append(index % 8).Append("QP").Append(index % 8 - 1).Append(index % 8).Append("RP").Append(index % 8 - 1).Append(index % 8).Append("BP").Append(index % 8 - 1).Append(index % 8).Append("NP");
                PAWN_MOVES &= ~possibility;
                possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            }
            PAWN_MOVES = (WP >> 9) & NOT_MY_PIECES & OCCUPIED & RANK_8 & ~FILE_H;//pawn promotion by capture left
            possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            while (possibility != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(possibility);
                list.Append(index % 8 + 1).Append(index % 8).Append("QP").Append(index % 8 + 1).Append(index % 8).Append("RP").Append(index % 8 + 1).Append(index % 8).Append("BP").Append(index % 8 + 1).Append(index % 8).Append("NP");
                PAWN_MOVES &= ~possibility;
                possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            }
            PAWN_MOVES = (WP >> 8) & EMPTY & RANK_8;//pawn promotion by move 1 forward
            possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            while (possibility != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(possibility);
                list.Append(index % 8).Append(index % 8).Append("QP").Append(index % 8).Append(index % 8).Append("RP").Append(index % 8).Append(index % 8).Append("BP").Append(index % 8).Append(index % 8).Append("NP");
                PAWN_MOVES &= ~possibility;
                possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            }
            //y1,y2,"WE"
            //en passant right
            possibility = (WP << 1) & BP & RANK_5 & ~FILE_A & EP;//shows piece to remove, not the destination
            if (possibility != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(possibility);
                list.Append(index % 8 - 1).Append(index % 8).Append("WE");
            }
            //en passant left
            possibility = (WP >> 1) & BP & RANK_5 & ~FILE_H & EP;//shows piece to remove, not the destination
            if (possibility != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(possibility);
                list.Append(index % 8 + 1).Append(index % 8).Append("WE");
            }
            return list.ToString();
        }
        public static string possibleBP(ulong BP, ulong WP, ulong EP)
        {
            StringBuilder list = new StringBuilder();
            //x1,y1,x2,y2
            ulong PAWN_MOVES = (BP << 7) & NOT_MY_PIECES & OCCUPIED & ~RANK_1 & ~FILE_H;//capture right
            ulong possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            while (possibility != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(possibility);
                list.Append(index / 8 - 1).Append(index % 8 + 1).Append(index / 8).Append(index % 8);
                PAWN_MOVES &= ~possibility;
                possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            }
            PAWN_MOVES = (BP << 9) & NOT_MY_PIECES & OCCUPIED & ~RANK_1 & ~FILE_A;//capture left
            possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            while (possibility != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(possibility);
                list.Append(index / 8 - 1).Append(index % 8 - 1).Append(index / 8).Append(index % 8);
                PAWN_MOVES &= ~possibility;
                possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            }
            PAWN_MOVES = (BP << 8) & EMPTY & ~RANK_1;//move 1 forward
            possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            while (possibility != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(possibility);
                list.Append(index / 8 - 1).Append(index % 8).Append(index / 8).Append(index % 8);
                PAWN_MOVES &= ~possibility;
                possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            }
            PAWN_MOVES = (BP << 16) & EMPTY & (EMPTY << 8) & RANK_5;//move 2 forward
            possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            while (possibility != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(possibility);
                list.Append(index / 8 - 2).Append(index % 8).Append(index / 8).Append(index % 8);
                PAWN_MOVES &= ~possibility;
                possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            }
            //y1,y2,Promotion Type,"P"
            PAWN_MOVES = (BP << 7) & NOT_MY_PIECES & OCCUPIED & RANK_1 & ~FILE_H;//pawn promotion by capture right
            possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            while (possibility != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(possibility);
                list.Append(index % 8 + 1).Append(index % 8).Append("qP").Append(index % 8 + 1).Append(index % 8).Append("rP").Append(index % 8 + 1).Append(index % 8).Append("bP").Append(index % 8 + 1).Append(index % 8).Append("nP");
                PAWN_MOVES &= ~possibility;
                possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            }
            PAWN_MOVES = (BP << 9) & NOT_MY_PIECES & OCCUPIED & RANK_1 & ~FILE_A;//pawn promotion by capture left
            possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            while (possibility != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(possibility);
                list.Append(index % 8 - 1).Append(index % 8).Append("qP").Append(index % 8 - 1).Append(index % 8).Append("rP").Append(index % 8 - 1).Append(index % 8).Append("bP").Append(index % 8 - 1).Append(index % 8).Append("nP");
                PAWN_MOVES &= ~possibility;
                possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            }
            PAWN_MOVES = (BP << 8) & EMPTY & RANK_1;//pawn promotion by move 1 forward
            possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            while (possibility != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(possibility);
                list.Append(index % 8).Append(index % 8).Append("qP").Append(index % 8).Append(index % 8).Append("rP").Append(index % 8).Append(index % 8).Append("bP").Append(index % 8).Append(index % 8).Append("nP");
                PAWN_MOVES &= ~possibility;
                possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            }
            //y1,y2,"BE"
            //en passant right
            possibility = (BP >> 1) & WP & RANK_4 & ~FILE_H & EP;//shows piece to remove, not the destination
            if (possibility != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(possibility);
                list.Append(index % 8 + 1).Append(index % 8).Append("BE");
            }
            //en passant left
            possibility = (BP << 1) & WP & RANK_4 & ~FILE_A & EP;//shows piece to remove, not the destination
            if (possibility != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(possibility);
                list.Append(index % 8 - 1).Append(index % 8).Append("BE");
            }
            return list.ToString();
        }
        public static string captureEnPassantAndPromotionMoveWP(ulong WP, ulong BP, ulong EP)
        {
            StringBuilder list =new StringBuilder();
            ulong PAWN_MOVES = (WP >> 7) & NOT_MY_PIECES & OCCUPIED & ~RANK_8 & ~FILE_A;//capture right
            ulong possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            while (possibility != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(possibility);
                list.Append(index / 8 + 1).Append(index % 8 - 1).Append(index / 8).Append(index % 8);
                PAWN_MOVES &= ~possibility;
                possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            }
            PAWN_MOVES = (WP >> 9) & NOT_MY_PIECES & OCCUPIED & ~RANK_8 & ~FILE_H;//capture left
            possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            while (possibility != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(possibility);
                list.Append(index / 8 + 1).Append(index % 8 + 1).Append(index / 8).Append(index % 8);
                PAWN_MOVES &= ~possibility;
                possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            }
            //y1,y2,Promotion Type,"P"
            PAWN_MOVES = (WP >> 7) & NOT_MY_PIECES & OCCUPIED & RANK_8 & ~FILE_A;//pawn promotion by capture right
            possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            while (possibility != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(possibility);
                list.Append(index % 8 - 1).Append(index % 8).Append("QP").Append(index % 8 - 1).Append(index % 8).Append("RP").Append(index % 8 - 1).Append(index % 8).Append("BP").Append(index % 8 - 1).Append(index % 8).Append("NP");
                PAWN_MOVES &= ~possibility;
                possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            }
            PAWN_MOVES = (WP >> 9) & NOT_MY_PIECES & OCCUPIED & RANK_8 & ~FILE_H;//pawn promotion by capture left
            possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            while (possibility != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(possibility);
                list.Append(index % 8 + 1).Append(index % 8).Append("QP").Append(index % 8 + 1).Append(index % 8).Append("RP").Append(index % 8 + 1).Append(index % 8).Append("BP").Append(index % 8 + 1).Append(index % 8).Append("NP");
                PAWN_MOVES &= ~possibility;
                possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            }
            PAWN_MOVES = (WP >> 8) & EMPTY & RANK_8;//pawn promotion by move 1 forward
            possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            while (possibility != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(possibility);
                list.Append(index % 8).Append(index % 8).Append("QP").Append(index % 8).Append(index % 8).Append("RP").Append(index % 8).Append(index % 8).Append("BP").Append(index % 8).Append(index % 8).Append("NP");
                PAWN_MOVES &= ~possibility;
                possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            }
            //y1,y2,"WE"
            //en passant right
            possibility = (WP << 1) & BP & RANK_5 & ~FILE_A & EP;//shows piece to remove, not the destination
            if (possibility != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(possibility);
                list.Append(index % 8 - 1).Append(index % 8).Append("WE");
            }
            //en passant left
            possibility = (WP >> 1) & BP & RANK_5 & ~FILE_H & EP;//shows piece to remove, not the destination
            if (possibility != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(possibility);
                list.Append(index % 8 + 1).Append(index % 8).Append("WE");
            }
            return list.ToString();
        }
        public static string captureEnpassantAndPromotionMoveBP(ulong BP, ulong WP, ulong EP)
        {
            StringBuilder list =new StringBuilder();
            //x1,y1,x2,y2
            ulong PAWN_MOVES = (BP << 7) & NOT_MY_PIECES & OCCUPIED & ~RANK_1 & ~FILE_H;//capture right
            ulong possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            while (possibility != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(possibility);
                list.Append(index / 8 - 1).Append(index % 8 + 1).Append(index / 8).Append(index % 8);
                PAWN_MOVES &= ~possibility;
                possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            }
            PAWN_MOVES = (BP << 9) & NOT_MY_PIECES & OCCUPIED & ~RANK_1 & ~FILE_A;//capture left
            possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            while (possibility != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(possibility);
                list.Append(index / 8 - 1).Append(index % 8 - 1).Append(index / 8).Append(index % 8);
                PAWN_MOVES &= ~possibility;
                possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            }
            //y1,y2,Promotion Type,"P"
            PAWN_MOVES = (BP << 7) & NOT_MY_PIECES & OCCUPIED & RANK_1 & ~FILE_H;//pawn promotion by capture right
            possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            while (possibility != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(possibility);
                list.Append(index % 8 + 1).Append(index % 8).Append("qP").Append(index % 8 + 1).Append(index % 8).Append("rP").Append(index % 8 + 1).Append(index % 8).Append("bP").Append(index % 8 + 1).Append(index % 8).Append("nP");
                PAWN_MOVES &= ~possibility;
                possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            }
            PAWN_MOVES = (BP << 9) & NOT_MY_PIECES & OCCUPIED & RANK_1 & ~FILE_A;//pawn promotion by capture left
            possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            while (possibility != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(possibility);
                list.Append(index % 8 - 1).Append(index % 8).Append("qP").Append(index % 8 - 1).Append(index % 8).Append("rP").Append(index % 8 - 1).Append(index % 8).Append("bP").Append(index % 8 - 1).Append(index % 8).Append("nP");
                PAWN_MOVES &= ~possibility;
                possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            }
            PAWN_MOVES = (BP << 8) & EMPTY & RANK_1;//pawn promotion by move 1 forward
            possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            while (possibility != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(possibility);
                list.Append(index % 8).Append(index % 8).Append("qP").Append(index % 8).Append(index % 8).Append("rP").Append(index % 8).Append(index % 8).Append("bP").Append(index % 8).Append(index % 8).Append("nP");
                PAWN_MOVES &= ~possibility;
                possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            }
            //y1,y2,"BE"
            //en passant right
            possibility = (BP >> 1) & WP & RANK_4 & ~FILE_H & EP;//shows piece to remove, not the destination
            if (possibility != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(possibility);
                list.Append(index % 8 + 1).Append(index % 8).Append("BE");
            }
            //en passant left
            possibility = (BP << 1) & WP & RANK_4 & ~FILE_A & EP;//shows piece to remove, not the destination
            if (possibility != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(possibility);
                list.Append(index % 8 - 1).Append(index % 8).Append("BE");
            }
            return list.ToString();
        }
        public static string possibleN(ulong OCCUPIED, ulong N)
        {
            StringBuilder list =new StringBuilder();
            ulong i = N & ~(N - 1);
            ulong possibility;
            while (i != 0)
            {
                int iLocation = BitHelper.getNumberOfTrailingZeros(i);
                if (iLocation > 18)
                {
                    possibility = KNIGHT_SPAN << (iLocation - 18);
                }
                else
                {
                    possibility = KNIGHT_SPAN >> (18 - iLocation);
                }
                if (iLocation % 8 < 4)
                {
                    possibility &= ~FILE_GH & NOT_MY_PIECES;
                }
                else
                {
                    possibility &= ~FILE_AB & NOT_MY_PIECES;
                }
                ulong j = possibility & ~(possibility - 1);
                while (j != 0)
                {
                    int index = BitHelper.getNumberOfTrailingZeros(j);
                    list.Append(iLocation / 8).Append(iLocation % 8).Append(index / 8).Append(index % 8);
                    possibility &= ~j;
                    j = possibility & ~(possibility - 1);
                }
                N &= ~i;
                i = N & ~(N - 1);
            }
            return list.ToString();
        }

        public static string possibleB(ulong OCCUPIED, ulong B)
        {
            StringBuilder list =new StringBuilder();
            ulong i = B & ~(B - 1);
            ulong possibility;
            while (i != 0)
            {
                int iLocation = BitHelper.getNumberOfTrailingZeros(i);
                possibility = DAndAntiDMoves(iLocation) & NOT_MY_PIECES;
                ulong j = possibility & ~(possibility - 1);
                while (j != 0)
                {
                    int index = BitHelper.getNumberOfTrailingZeros(j);
                    list.Append(iLocation / 8).Append(iLocation % 8).Append(index / 8).Append(index % 8);
                    possibility &= ~j;
                    j = possibility & ~(possibility - 1);
                }
                B &= ~i;
                i = B & ~(B - 1);
            }
            return list.ToString();
        }
        public static string possibleR(ulong OCCUPIED, ulong R)
        {
            StringBuilder list =new StringBuilder();
            ulong i = R & ~(R - 1);
            ulong possibility;
            while (i != 0)
            {
                int iLocation = BitHelper.getNumberOfTrailingZeros(i);
                possibility = HAndVMoves(iLocation) & NOT_MY_PIECES;
                ulong j = possibility & ~(possibility - 1);
                while (j != 0)
                {
                    int index = BitHelper.getNumberOfTrailingZeros(j);
                    list.Append(iLocation / 8).Append(iLocation % 8).Append(index / 8).Append(index % 8);
                    possibility &= ~j;
                    j = possibility & ~(possibility - 1);
                }
                R &= ~i;
                i = R & ~(R - 1);
            }
            return list.ToString();
        }
        public static string possibleQ(ulong OCCUPIED, ulong Q)
        {
            StringBuilder list =new StringBuilder();
            ulong i = Q & ~(Q - 1);
            ulong possibility;
            while (i != 0)
            {
                int iLocation = BitHelper.getNumberOfTrailingZeros(i);
                possibility = (HAndVMoves(iLocation) | DAndAntiDMoves(iLocation)) & NOT_MY_PIECES;
                ulong j = possibility & ~(possibility - 1);
                while (j != 0)
                {
                    int index = BitHelper.getNumberOfTrailingZeros(j);
                    list.Append(iLocation / 8).Append(iLocation % 8).Append(index / 8).Append(index % 8);
                    possibility &= ~j;
                    j = possibility & ~(possibility - 1);
                }
                Q &= ~i;
                i = Q & ~(Q - 1);
            }
            return list.ToString();
        }
        public static string possibleK(ulong OCCUPIED, ulong K)
        {
            StringBuilder list =new StringBuilder();
            ulong possibility;
            int iLocation = BitHelper.getNumberOfTrailingZeros(K);
            if (iLocation > 9)
            {
                possibility = KING_SPAN << (iLocation - 9);
            }
            else
            {
                possibility = KING_SPAN >> (9 - iLocation);
            }
            if (iLocation % 8 < 4)
            {
                possibility &= ~FILE_GH & NOT_MY_PIECES;
            }
            else
            {
                possibility &= ~FILE_AB & NOT_MY_PIECES;
            }
            ulong j = possibility & ~(possibility - 1);
            while (j != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(j);
                list.Append(iLocation / 8).Append(iLocation % 8).Append(index / 8).Append(index % 8);
                possibility &= ~j;
                j = possibility & ~(possibility - 1);
            }
            return list.ToString();
        }
        public static string possibleCW(ulong WP, ulong WN, ulong WB, ulong WR, ulong WQ, ulong WK, ulong BP, ulong BN, ulong BB, ulong BR, ulong BQ, ulong BK, bool CWK, bool CWQ)
        {
            StringBuilder list =new StringBuilder();
            ulong UNSAFE = unSafeForWhite(WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK);
            if ((UNSAFE & WK) == 0)
            {
                if (CWK && (((1ul << CASTLE_ROOKS[0]) & WR) != 0))
                {
                    if (((OCCUPIED | UNSAFE) & ((1ul << 61) | (1ul << 62))) == 0)
                    {
                        list.Append("7476");
                    }
                }
                if (CWQ && (((1ul << CASTLE_ROOKS[1]) & WR) != 0))
                {
                    if (((OCCUPIED | (UNSAFE & ~(1ul << 57))) & ((1ul << 57) | (1ul << 58) | (1ul << 59))) == 0)
                    {
                        list.Append("7472");
                    }
                }
            }
            return list.ToString();
        }
        public static string possibleCB(ulong WP, ulong WN, ulong WB, ulong WR, ulong WQ, ulong WK, ulong BP, ulong BN, ulong BB, ulong BR, ulong BQ, ulong BK, bool CBK, bool CBQ)
        {
            StringBuilder list =new StringBuilder();
            ulong UNSAFE = unSafeForBlack(WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK);
            if ((UNSAFE & BK) == 0)
            {
                if (CBK && (((1ul << CASTLE_ROOKS[2]) & BR) != 0))
                {
                    if (((OCCUPIED | UNSAFE) & ((1ul << 5) | (1ul << 6))) == 0)
                    {
                        list.Append("0406");
                    }
                }
                if (CBQ && (((1ul << CASTLE_ROOKS[3]) & BR) != 0))
                {
                    if (((OCCUPIED | (UNSAFE & ~(1ul << 1))) & ((1ul << 1) | (1ul << 2) | (1ul << 3))) == 0)
                    {
                        list.Append("0402");
                    }
                }
            }
            return list.ToString();
        }
        public static ulong unSafeForBlack(ulong WP, ulong WN, ulong WB, ulong WR, ulong WQ, ulong WK, ulong BP, ulong BN, ulong BB, ulong BR, ulong BQ, ulong BK)
        {
            ulong unSafe;
            OCCUPIED = WP | WN | WB | WR | WQ | WK | BP | BN | BB | BR | BQ | BK;
            enmk = BK;
            ALL_MY_PIECES = BP | BN | BB | BR | BQ;
            //pawn
            unSafe = ((WP >> 7) & ~FILE_A);//pawn capture right
            unSafe |= ((WP >> 9) & ~FILE_H);//pawn capture left
            ulong possibility;
            //knight
            ulong i = WN & ~(WN - 1);
            while (i != 0)
            {
                int iLocation = BitHelper.getNumberOfTrailingZeros(i);
                if (iLocation > 18)
                {
                    possibility = KNIGHT_SPAN << (iLocation - 18);
                }
                else
                {
                    possibility = KNIGHT_SPAN >> (18 - iLocation);
                }
                if (iLocation % 8 < 4)
                {
                    possibility &= ~FILE_GH;
                }
                else
                {
                    possibility &= ~FILE_AB;
                }
                unSafe |= possibility;
                WN &= ~i;
                i = WN & ~(WN - 1);
            }
            //bishop/queen
            ulong QB = WQ | WB;
            i = QB & ~(QB - 1);
            while (i != 0)
            {
                int iLocation = BitHelper.getNumberOfTrailingZeros(i);
                //string a = Convert.ToString((long)DAndAntiDMoves(iLocation),2);
                //if (DAndAntiDMoves(iLocation) != (DAndAntiDMoves2(iLocation)))
                //{
                //    Console.WriteLine();
                //}
                possibility = DAndAntiDMoves(iLocation);

                unSafe |= possibility;
                QB &= ~i;
                i = QB & ~(QB - 1);
            }
            //rook/queen
            ulong QR = WQ | WR;
            i = QR & ~(QR - 1);
            while (i != 0)
            {
                int iLocation = BitHelper.getNumberOfTrailingZeros(i);
                //if (HAndVMoves(iLocation) != HAndVMoves2(iLocation))
                //{

                //    Console.WriteLine();
                //}
                possibility = HAndVMoves(iLocation);
                unSafe |= possibility;
                QR &= ~i;
                i = QR & ~(QR - 1);
            }
            //king
            int iLocationK = BitHelper.getNumberOfTrailingZeros(WK);
            if (iLocationK > 9)
            {
                possibility = KING_SPAN << (iLocationK - 9);
            }
            else
            {
                possibility = KING_SPAN >> (9 - iLocationK);
            }
            if (iLocationK % 8 < 4)
            {
                possibility &= ~FILE_GH;
            }
            else
            {
                possibility &= ~FILE_AB;
            }
            unSafe |= possibility;
            return unSafe;
        }
        public static ulong unSafeForWhite(ulong WP, ulong WN, ulong WB, ulong WR, ulong WQ, ulong WK, ulong BP, ulong BN, ulong BB, ulong BR, ulong BQ, ulong BK)
        {
            ulong unSafe;

            OCCUPIED = WP | WN | WB | WR | WQ | WK | BP | BN | BB | BR | BQ | BK;
            ALL_MY_PIECES = WP | WN | WB | WR | WQ;
            enmk = WK;
            //pawn
            unSafe = ((BP << 7) & ~FILE_H);//pawn capture right
            unSafe |= ((BP << 9) & ~FILE_A);//pawn capture left
            ulong possibility;
            //knight
            ulong i = BN & ~(BN - 1);
            while (i != 0)
            {
                int iLocation = BitHelper.getNumberOfTrailingZeros(i);
                if (iLocation > 18)
                {
                    possibility = KNIGHT_SPAN << (iLocation - 18);
                }
                else
                {
                    possibility = KNIGHT_SPAN >> (18 - iLocation);
                }
                if (iLocation % 8 < 4)
                {
                    possibility &= ~FILE_GH;
                }
                else
                {
                    possibility &= ~FILE_AB;
                }
                unSafe |= possibility;
                BN &= ~i;
                i = BN & ~(BN - 1);
            }
            //bishop/queen
            ulong QB = BQ | BB;
            i = QB & ~(QB - 1);
            while (i != 0)
            {
                int iLocation = BitHelper.getNumberOfTrailingZeros(i);
                //if (DAndAntiDMoves(iLocation) != (DAndAntiDMoves2(iLocation)))
                //{
                //    Console.WriteLine();
                //}
                possibility = DAndAntiDMoves(iLocation);
                unSafe |= possibility;
                QB &= ~i;
                i = QB & ~(QB - 1);
            }
            //rook/queen
            ulong QR = BQ | BR;
            i = QR & ~(QR - 1);
            while (i != 0)
            {
                int iLocation = BitHelper.getNumberOfTrailingZeros(i);
                //if (HAndVMoves(iLocation) != HAndVMoves2(iLocation))
                //    Console.WriteLine();
                possibility = HAndVMoves(iLocation);
                unSafe |= possibility;
                QR &= ~i;
                i = QR & ~(QR - 1);
            }
            //king
            int iLocationK = BitHelper.getNumberOfTrailingZeros(BK);
            if (iLocationK > 9)
            {
                possibility = KING_SPAN << (iLocationK - 9);
            }
            else
            {
                possibility = KING_SPAN >> (9 - iLocationK);
            }
            if (iLocationK % 8 < 4)
            {
                possibility &= ~FILE_GH;
            }
            else
            {
                possibility &= ~FILE_AB;
            }
            unSafe |= possibility;
            return unSafe;
        }
        public static void drawBitboard(ulong bitBoard)
        {
            string[,] chessBoard = new string[8, 8];
            for (int i = 0; i < 64; i++)
            {
                chessBoard[i / 8, i % 8] = "";
            }
            for (int i = 0; i < 64; i++)
            {
                if (((bitBoard >> i) & 1) == 1) { chessBoard[i / 8, i % 8] = "P"; }
                if ("".Equals(chessBoard[i / 8, i % 8])) { chessBoard[i / 8, i % 8] = " "; }
            }
            for (int i = 0; i < 8; i++)
            {
                string temp = "[";
                for (int j = 0; j < 8; j++)
                {
                    temp += chessBoard[i, j] + ",";

                }
                temp += "]";
                Console.WriteLine(temp);
            }
        }
        public static string LegalMoves(ulong WP, ulong WN, ulong WB, ulong WR, ulong WQ, ulong WK, ulong BP, ulong BN, ulong BB, ulong BR, ulong BQ, ulong BK, ulong EP, bool CWK, bool CWQ, bool CBK, bool CBQ, bool white)
        {
            string possibleMoves;
            if (white)
            {
                possibleMoves = possibleMovesW(WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK, EP, CWK, CWQ, CBK, CBQ);
            }
            else
            {
                possibleMoves = possibleMovesB(WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK, EP, CWK, CWQ, CBK, CBQ);
            }
            string legalMoves = "";
            int moveCount = possibleMoves.Length / 4;
            string[] moves = new string[moveCount];
            for (int i = 0; i < moveCount; i++)
            {
                moves[i] = possibleMoves.Substring(i * 4, 4);
            }
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
                if ((white && (WKt & Moves.unSafeForWhite(WPt, WNt, WBt, WRt, WQt, WKt, BPt, BNt, BBt, BRt, BQt, BKt)) == 0) ||
                        (!white && (BKt & Moves.unSafeForBlack(WPt, WNt, WBt, WRt, WQt, WKt, BPt, BNt, BBt, BRt, BQt, BKt)) == 0))
                {
                    legalMoves += moves[i];
                }
            }
            return legalMoves;
        }
        public static ulong[] magicNumberRook = {
            4719774060096012288,306245461898952768,72067489980219456,1188967895993417732,108112779403100166,72066407310950656,11601273739651580036,2377901849236750592,1225119975719583744,1297177567618416654,234750407635636260,1156721966682492928,9225905346021691397,288793334695658512,2315132791546970368,1729945345410500868,36030171945385984,58705675122917378,11824149119901824,2886948648522878976,5497692377344,1153625741871546881,9295434028961251848,1282030616756484,18155138147434504,9660221476661370888,288388733745496192,14447565201087725576,8798248895488,2816950946235392,144398864231694592,9962528083066213156,9259401108755849217,720716952753938432,72075188446961664,4521213305036808,563019243324424,562967141679744,41095398307137537,1442278057724347425,2314920579362226180,4758123379364790305,8646929022774804544,1155173442127986944,3542081124115185672,36591764286603280,4611967501994098692,285875175030785,5764748570397835776,2350914192145384576,9223407223375396992,577322773849079936,289356293372674176,4400194125952,10952755737987122176,576601494088343680,9800008785124163585,19792333383810,577624038827952258,2594672825365757989,5188710976426354690,281509370265601,1730086498538095204,7208011502308229218
                                                };
        public static ulong[] magicNumberBishop = {
           2314851325831561282,2308112437933310112,9291150313918464,4902203663306982400,7494554997682618368,905506135023435782,9232529388435677184,6921612616253048960,9511753664843351106,3527243032970752,2305860606235836416,1172347693503627264,576605905572855816,2350881209087209481,601471783673922,3659727146848258,306332909588188160,1130641626268673,9288983503437840,11531476193912045568,5765311227699920909,293016009109980164,2329486951316598786,562950566463496,2596430738966646828,162560595680821520,1153000669511166976,281753142968832,11602681117522542600,1159679103073553408,581602068843005760,70652221719560,18586420776276162,11250282277948753040,10809765315001712672,4611968627276054592,107752676524160,1478324184904910850,2393654012481664,578712828244986112,36594085045405312,4683875588354683904,5801200370726207748,2882304045060507648,1152925905136654848,73395700796227840,9368173367447781928,40537159836379168,2306406519124561930,594756935096745984,469500407317659648,72620681976086593,580984423523156032,146405058580938752,722880589904019969,2348570799900680,2310487638397683776,6922085545766166537,288232576284397569,6341147440210552832,1157513082614859841,8934691766784,9223442630145876544,9232559558198820896
                                                  };
        //public static ulong[] magicNumberRook = new ulong[64];
        //{
        //    (0x0080001020400080), (0x0040001000200040), (0x0080081000200080), (0x0080040800100080),
        //    (0x0080020400080080), (0x0080010200040080), (0x0080008001000200), (0x0080002040800100),
        //    (0x0000800020400080), (0x0000400020005000), (0x0000801000200080), (0x0000800800100080),
        //    (0x0000800400080080), (0x0000800200040080), (0x0000800100020080), (0x0000800040800100),
        //    (0x0000208000400080), (0x0000404000201000), (0x0000808010002000), (0x0000808008001000),
        //    (0x0000808004000800), (0x0000808002000400), (0x0000010100020004), (0x0000020000408104),
        //    (0x0000208080004000), (0x0000200040005000), (0x0000100080200080), (0x0000080080100080),
        //    (0x0000040080080080), (0x0000020080040080), (0x0000010080800200), (0x0000800080004100),
        //    (0x0000204000800080), (0x0000200040401000), (0x0000100080802000), (0x0000080080801000),
        //    (0x0000040080800800), (0x0000020080800400), (0x0000020001010004), (0x0000800040800100),
        //    (0x0000204000808000), (0x0000200040008080), (0x0000100020008080), (0x0000080010008080),
        //    (0x0000040008008080), (0x0000020004008080), (0x0000010002008080), (0x0000004081020004),
        //    (0x0000204000800080), (0x0000200040008080), (0x0000100020008080), (0x0000080010008080),
        //    (0x0000040008008080), (0x0000020004008080), (0x0000800100020080), (0x0000800041000080),
        //    (0x00FFFCDDFCED714A), (0x007FFCDDFCED714A), (0x003FFFCDFFD88096), (0x0000040810002101),
        //    (0x0001000204080011), (0x0001000204000801), (0x0001000082000401), (0x0001FFFAABFAD1A2)
        //};
        //static ulong[] magicNumberBishop =new ulong[64]
        //{
        //    (0x0002020202020200), (0x0002020202020000), (0x0004010202000000), (0x0004040080000000),
        //    (0x0001104000000000), (0x0000821040000000), (0x0000410410400000), (0x0000104104104000),
        //    (0x0000040404040400), (0x0000020202020200), (0x0000040102020000), (0x0000040400800000),
        //    (0x0000011040000000), (0x0000008210400000), (0x0000004104104000), (0x0000002082082000),
        //    (0x0004000808080800), (0x0002000404040400), (0x0001000202020200), (0x0000800802004000),
        //    (0x0000800400A00000), (0x0000200100884000), (0x0000400082082000), (0x0000200041041000),
        //    (0x0002080010101000), (0x0001040008080800), (0x0000208004010400), (0x0000404004010200),
        //    (0x0000840000802000), (0x0000404002011000), (0x0000808001041000), (0x0000404000820800),
        //    (0x0001041000202000), (0x0000820800101000), (0x0000104400080800), (0x0000020080080080),
        //    (0x0000404040040100), (0x0000808100020100), (0x0001010100020800), (0x0000808080010400),
        //    (0x0000820820004000), (0x0000410410002000), (0x0000082088001000), (0x0000002011000800),
        //    (0x0000080100400400), (0x0001010101000200), (0x0002020202000400), (0x0001010101000200),
        //    (0x0000410410400000), (0x0000208208200000), (0x0000002084100000), (0x0000000020880000),
        //    (0x0000001002020000), (0x0000040408020000), (0x0004040404040000), (0x0002020202020000),
        //    (0x0000104104104000), (0x0000002082082000), (0x0000000020841000), (0x0000000000208800),
        //    (0x0000000010020200), (0x0000000404080200), (0x0000040404040400), (0x0002020202020200)
        //};

        //    public static ulong[] occupancyMaskRook = {
        //    0x101010101017eul, 0x202020202027cul, 0x404040404047aul, 0x8080808080876ul, 0x1010101010106eul, 0x2020202020205eul, 0x4040404040403eul, 0x8080808080807eul, 0x1010101017e00ul, 0x2020202027c00ul, 0x4040404047a00ul, 0x8080808087600ul, 0x10101010106e00ul, 0x20202020205e00ul, 0x40404040403e00ul, 0x80808080807e00ul, 0x10101017e0100ul, 0x20202027c0200ul, 0x40404047a0400ul, 0x8080808760800ul, 0x101010106e1000ul, 0x202020205e2000ul, 0x404040403e4000ul, 0x808080807e8000ul, 0x101017e010100ul, 0x202027c020200ul, 0x404047a040400ul, 0x8080876080800ul, 0x1010106e101000ul, 0x2020205e202000ul, 0x4040403e404000ul, 0x8080807e808000ul, 0x1017e01010100ul, 0x2027c02020200ul, 0x4047a04040400ul, 0x8087608080800ul, 0x10106e10101000ul, 0x20205e20202000ul, 0x40403e40404000ul, 0x80807e80808000ul, 0x17e0101010100ul, 0x27c0202020200ul, 0x47a0404040400ul, 0x8760808080800ul, 0x106e1010101000ul, 0x205e2020202000ul, 0x403e4040404000ul, 0x807e8080808000ul, 0x7e010101010100ul, 0x7c020202020200ul, 0x7a040404040400ul, 0x76080808080800ul, 0x6e101010101000ul, 0x5e202020202000ul, 0x3e404040404000ul, 0x7e808080808000ul, 0x7e01010101010100ul, 0x7c02020202020200ul, 0x7a04040404040400ul, 0x7608080808080800ul, 0x6e10101010101000ul, 0x5e20202020202000ul, 0x3e40404040404000ul, 0x7e80808080808000ul 
        //};
        //    public static ulong[] occupancyMaskBishop = {
        //    0x40201008040200ul, 0x402010080400ul, 0x4020100a00ul, 0x40221400ul, 0x2442800ul, 0x204085000ul, 0x20408102000ul, 0x2040810204000ul, 0x20100804020000ul, 0x40201008040000ul, 0x4020100a0000ul, 0x4022140000ul, 0x244280000ul, 0x20408500000ul, 0x2040810200000ul, 0x4081020400000ul, 0x10080402000200ul, 0x20100804000400ul, 0x4020100a000a00ul, 0x402214001400ul, 0x24428002800ul, 0x2040850005000ul, 0x4081020002000ul, 0x8102040004000ul, 0x8040200020400ul, 0x10080400040800ul, 0x20100a000a1000ul, 0x40221400142200ul, 0x2442800284400ul, 0x4085000500800ul, 0x8102000201000ul, 0x10204000402000ul, 0x4020002040800ul, 0x8040004081000ul, 0x100a000a102000ul, 0x22140014224000ul, 0x44280028440200ul, 0x8500050080400ul, 0x10200020100800ul, 0x20400040201000ul, 0x2000204081000ul, 0x4000408102000ul, 0xa000a10204000ul, 0x14001422400000ul, 0x28002844020000ul, 0x50005008040200ul, 0x20002010080400ul, 0x40004020100800ul, 0x20408102000ul, 0x40810204000ul, 0xa1020400000ul, 0x142240000000ul, 0x284402000000ul, 0x500804020000ul, 0x201008040200ul, 0x402010080400ul, 0x2040810204000ul, 0x4081020400000ul, 0xa102040000000ul, 0x14224000000000ul, 0x28440200000000ul, 0x50080402000000ul, 0x20100804020000ul, 0x40201008040200L     
        //};
        //public static ulong[] occupancyMaskRook = new ulong[64]
        //            {	
        //    (0x000101010101017E), (0x000202020202027C), (0x000404040404047A), (0x0008080808080876),
        //    (0x001010101010106E), (0x002020202020205E), (0x004040404040403E), (0x008080808080807E),
        //    (0x0001010101017E00), (0x0002020202027C00), (0x0004040404047A00), (0x0008080808087600),
        //    (0x0010101010106E00), (0x0020202020205E00), (0x0040404040403E00), (0x0080808080807E00),
        //    (0x00010101017E0100), (0x00020202027C0200), (0x00040404047A0400), (0x0008080808760800),
        //    (0x00101010106E1000), (0x00202020205E2000), (0x00404040403E4000), (0x00808080807E8000),
        //    (0x000101017E010100), (0x000202027C020200), (0x000404047A040400), (0x0008080876080800),
        //    (0x001010106E101000), (0x002020205E202000), (0x004040403E404000), (0x008080807E808000),
        //    (0x0001017E01010100), (0x0002027C02020200), (0x0004047A04040400), (0x0008087608080800),
        //    (0x0010106E10101000), (0x0020205E20202000), (0x0040403E40404000), (0x0080807E80808000),
        //    (0x00017E0101010100), (0x00027C0202020200), (0x00047A0404040400), (0x0008760808080800),
        //    (0x00106E1010101000), (0x00205E2020202000), (0x00403E4040404000), (0x00807E8080808000),
        //    (0x007E010101010100), (0x007C020202020200), (0x007A040404040400), (0x0076080808080800),
        //    (0x006E101010101000), (0x005E202020202000), (0x003E404040404000), (0x007E808080808000),
        //    (0x7E01010101010100), (0x7C02020202020200), (0x7A04040404040400), (0x7608080808080800),
        //    (0x6E10101010101000), (0x5E20202020202000), (0x3E40404040404000), (0x7E80808080808000)
        //};

        //public static ulong[] occupancyMaskBishop = new ulong[64]
        //            {
        //    (0x0040201008040200), (0x0000402010080400), (0x0000004020100A00), (0x0000000040221400),
        //    (0x0000000002442800), (0x0000000204085000), (0x0000020408102000), (0x0002040810204000),
        //    (0x0020100804020000), (0x0040201008040000), (0x00004020100A0000), (0x0000004022140000),
        //    (0x0000000244280000), (0x0000020408500000), (0x0002040810200000), (0x0004081020400000),
        //    (0x0010080402000200), (0x0020100804000400), (0x004020100A000A00), (0x0000402214001400),
        //    (0x0000024428002800), (0x0002040850005000), (0x0004081020002000), (0x0008102040004000),
        //    (0x0008040200020400), (0x0010080400040800), (0x0020100A000A1000), (0x0040221400142200),
        //    (0x0002442800284400), (0x0004085000500800), (0x0008102000201000), (0x0010204000402000),
        //    (0x0004020002040800), (0x0008040004081000), (0x00100A000A102000), (0x0022140014224000),
        //    (0x0044280028440200), (0x0008500050080400), (0x0010200020100800), (0x0020400040201000),
        //    (0x0002000204081000), (0x0004000408102000), (0x000A000A10204000), (0x0014001422400000),
        //    (0x0028002844020000), (0x0050005008040200), (0x0020002010080400), (0x0040004020100800),
        //    (0x0000020408102000), (0x0000040810204000), (0x00000A1020400000), (0x0000142240000000),
        //    (0x0000284402000000), (0x0000500804020000), (0x0000201008040200), (0x0000402010080400),
        //    (0x0002040810204000), (0x0004081020400000), (0x000A102040000000), (0x0014224000000000),
        //    (0x0028440200000000), (0x0050080402000000), (0x0020100804020000), (0x0040201008040200)
        //};


        public static ulong[] occupancyMaskRook = new ulong[64]
        {
        282578800148862,565157600297596,1130315200595066,2260630401190006,4521260802379886,9042521604759646,18085043209519166,36170086419038334,282578800180736,565157600328704,1130315200625152,2260630401218048,4521260802403840,9042521604775424,18085043209518592,36170086419037696,282578808340736,565157608292864,1130315208328192,2260630408398848,4521260808540160,9042521608822784,18085043209388032,36170086418907136,282580897300736,565159647117824,1130317180306432,2260632246683648,4521262379438080,9042522644946944,18085043175964672,36170086385483776,283115671060736,565681586307584,1130822006735872,2261102847592448,4521664529305600,9042787892731904,18085034619584512,36170077829103616,420017753620736,699298018886144,1260057572672512,2381576680245248,4624614895390720,9110691325681664,18082844186263552,36167887395782656,35466950888980736,34905104758997504,34344362452452352,33222877839362048,30979908613181440,26493970160820224,17522093256097792,35607136465616896,9079539427579068672,8935706818303361536,8792156787827803136,8505056726876686336,7930856604974452736,6782456361169985536,4485655873561051136,9115426935197958144
        };
        public static ulong[] occupancyMaskBishop = new ulong[64]
        {
            18049651735527936,70506452091904,275415828992,1075975168,38021120,8657588224,2216338399232,567382630219776,9024825867763712,18049651735527424,70506452221952,275449643008,9733406720,2216342585344,567382630203392,1134765260406784,4512412933816832,9024825867633664,18049651768822272,70515108615168,2491752130560,567383701868544,1134765256220672,2269530512441344,2256206450263040,4512412900526080,9024834391117824,18051867805491712,637888545440768,1135039602493440,2269529440784384,4539058881568768,1128098963916800,2256197927833600,4514594912477184,9592139778506752,19184279556981248,2339762086609920,4538784537380864,9077569074761728,562958610993152,1125917221986304,2814792987328512,5629586008178688,11259172008099840,22518341868716544,9007336962655232,18014673925310464,2216338399232,4432676798464,11064376819712,22137335185408,44272556441600,87995357200384,35253226045952,70506452091904,567382630219776,1134765260406784,2832480465846272,5667157807464448,11333774449049600,22526811443298304,9024825867763712,18049651735527936
        };
        //public void generateOccupancyMasks()
        //{
        //    int i, bitRef;
        //    ulong mask;
        //    for (bitRef = 0; bitRef <= 63; bitRef++)
        //    {
        //        mask = 0;
        //        for (i = bitRef + 8; i <= 55; i += 8) mask |= (1ul << i);
        //        for (i = bitRef - 8; i >= 8; i -= 8) mask |= (1ul << i);
        //        for (i = bitRef + 1; i % 8 != 7 && i % 8 != 0; i++) mask |= (1ul << i);
        //        for (i = bitRef - 1; i % 8 != 7 && i % 8 != 0 && i >= 0; i--) mask |= (1ul << i);
        //        occupancyMaskRook[bitRef] = mask;

        //        mask = 0;
        //        for (i = bitRef + 9; i % 8 != 7 && i % 8 != 0 && i <= 55; i += 9) mask |= (1ul << i);
        //        for (i = bitRef - 9; i % 8 != 7 && i % 8 != 0 && i >= 8; i -= 9) mask |= (1ul << i);
        //        for (i = bitRef + 7; i % 8 != 7 && i % 8 != 0 && i <= 55; i += 7) mask |= (1ul << i);
        //        for (i = bitRef - 7; i % 8 != 7 && i % 8 != 0 && i >= 8; i -= 7) mask |= (1ul << i);
        //        occupancyMaskBishop[bitRef] = mask;
        //    }
        //}
        public static int[] magicNumberShiftsRook = new int[64]
    {
        52,53,53,53,53,53,53,52,53,54,54,54,54,54,54,53,53,54,54,54,54,54,54,53,53,54,54,54,54,54,54,53,53,54,54,54,54,54,54,53,53,54,54,54,54,54,54,53,53,54,54,54,54,54,54,53,52,53,53,53,53,53,53,52
    };
        public static int[] magicNumberShiftsBishop = new int[64]
        {
     58,59,59,59,59,59,59,58,59,59,59,59,59,59,59,59,59,59,57,57,57,57,59,59,59,59,57,55,55,57,59,59,59,59,57,55,55,57,59,59,59,59,57,57,57,57,59,59,59,59,59,59,59,59,59,59,58,59,59,59,59,59,59,58
        };
        public static ulong[,] occupancyVariation = new ulong[64, 4096];
        public static ulong[,] occupancyAttackSet = new ulong[64, 4096];
        public static ulong[,] magicMovesRook = new ulong[64, 4096];
        public static ulong[,] magicMovesBishop = new ulong[64, 4096];
        public static void generateOccupancyMasks()
        {
            int i, bitRef;
            ulong mask;
            for (bitRef = 0; bitRef <= 63; bitRef++)
            {
                mask = 0;
                for (i = bitRef + 8; i <= 55; i += 8) mask |= (1ul << i);
                for (i = bitRef - 8; i >= 8; i -= 8) mask |= (1ul << i);
                for (i = bitRef + 1; i % 8 != 7 && i % 8 != 0; i++) mask |= (1ul << i);
                for (i = bitRef - 1; i % 8 != 7 && i % 8 != 0 && i >= 0; i--) mask |= (1ul << i);
                occupancyMaskRook[bitRef] = mask;
                //Moves.drawBitboard(mask);
                //Console.WriteLine();
                mask = 0;
                for (i = bitRef + 9; i % 8 != 7 && i % 8 != 0 && i <= 55; i += 9) mask |= (1ul << i);
                for (i = bitRef - 9; i % 8 != 7 && i % 8 != 0 && i >= 8; i -= 9) mask |= (1ul << i);
                for (i = bitRef + 7; i % 8 != 7 && i % 8 != 0 && i <= 55; i += 7) mask |= (1ul << i);
                for (i = bitRef - 7; i % 8 != 7 && i % 8 != 0 && i >= 8; i -= 7) mask |= (1ul << i);
                occupancyMaskBishop[bitRef] = mask;
                //Moves.drawBitboard(mask);
                //Console.WriteLine();
            }
        }

        public static void generateOccupancyVariations(bool isRook)
        {
            int i, j, bitRef;
            ulong mask;
            int variationCount;
            int[] setBitsInMask, setBitsInIndex;
            int[] bitCount = new int[64];

            for (bitRef = 0; bitRef <= 63; bitRef++)
            {
                mask = isRook ? occupancyMaskRook[bitRef] : occupancyMaskBishop[bitRef];
                setBitsInMask = BitHelper.getSetBits(mask);
                bitCount[bitRef] = BitHelper.NumberOfSetBits(mask);
                variationCount = (int)(1ul << bitCount[bitRef]);
                for (i = 0; i < variationCount; i++)
                {
                    occupancyVariation[bitRef, i] = 0;

                    // find bits set in index "i" and map them to bits in the 64 bit "occupancyVariation"
                    setBitsInIndex = BitHelper.getSetBits((ulong)i); // an array of integers showing which bits are set
                    for (j = 0; setBitsInIndex[j] != -1; j++)
                    {
                        occupancyVariation[bitRef, i] |= (1ul << setBitsInMask[setBitsInIndex[j]]);
                    }

                    if (isRook)
                    {
                        for (j = bitRef + 8; j <= 55 && (occupancyVariation[bitRef, i] & (1ul << j)) == 0; j += 8) ;
                        if (j >= 0 && j <= 63) occupancyAttackSet[bitRef, i] |= (1ul << j);
                        for (j = bitRef - 8; j >= 8 && (occupancyVariation[bitRef, i] & (1ul << j)) == 0; j -= 8) ;
                        if (j >= 0 && j <= 63) occupancyAttackSet[bitRef, i] |= (1ul << j);
                        for (j = bitRef + 1; j % 8 != 7 && j % 8 != 0 && (occupancyVariation[bitRef, i] & (1ul << j)) == 0; j++) ;
                        if (j >= 0 && j <= 63) occupancyAttackSet[bitRef, i] |= (1ul << j);
                        for (j = bitRef - 1; j % 8 != 7 && j % 8 != 0 && j >= 0 && (occupancyVariation[bitRef, i] & (1ul << j)) == 0; j--) ;
                        if (j >= 0 && j <= 63) occupancyAttackSet[bitRef, i] |= (1ul << j);
                    }
                    else
                    {
                        for (j = bitRef + 9; j % 8 != 7 && j % 8 != 0 && j <= 55 && (occupancyVariation[bitRef, i] & (1ul << j)) == 0; j += 9) ;
                        if (j >= 0 && j <= 63) occupancyAttackSet[bitRef, i] |= (1ul << j);
                        for (j = bitRef - 9; j % 8 != 7 && j % 8 != 0 && j >= 8 && (occupancyVariation[bitRef, i] & (1ul << j)) == 0; j -= 9) ;
                        if (j >= 0 && j <= 63) occupancyAttackSet[bitRef, i] |= (1ul << j);
                        for (j = bitRef + 7; j % 8 != 7 && j % 8 != 0 && j <= 55 && (occupancyVariation[bitRef, i] & (1ul << j)) == 0; j += 7) ;
                        if (j >= 0 && j <= 63) occupancyAttackSet[bitRef, i] |= (1ul << j);
                        for (j = bitRef - 7; j % 8 != 7 && j % 8 != 0 && j >= 8 && (occupancyVariation[bitRef, i] & (1ul << j)) == 0; j -= 7) ;
                        if (j >= 0 && j <= 63) occupancyAttackSet[bitRef, i] |= (1ul << j);
                    }
                }
            }
        }

        public static void generateMoveDatabase(bool isRook)
        {
            //if (isRook) generateOV(false); else generateOV(true);
            ulong validMoves;
            int variations, bitCount;
            int bitRef, i, j, magicIndex;

            for (bitRef = 0; bitRef <= 63; bitRef++)
            {
                bitCount = isRook ? BitHelper.NumberOfSetBits(occupancyMaskRook[bitRef]) : BitHelper.NumberOfSetBits(occupancyMaskBishop[bitRef]);
                variations = (int)(1ul << bitCount);

                for (i = 0; i < variations; i++)
                {
                    validMoves = 0;
                    if (isRook)
                    {
                        magicIndex = (int)((occupancyVariation[bitRef, i] * magicNumberRook[bitRef]) >> magicNumberShiftsRook[bitRef]);

                        for (j = bitRef + 8; j <= 63; j += 8) { validMoves |= (1ul << j); if ((occupancyVariation[bitRef, i] & (1ul << j)) != 0) break; }
                        for (j = bitRef - 8; j >= 0; j -= 8) { validMoves |= (1ul << j); if ((occupancyVariation[bitRef, i] & (1ul << j)) != 0) break; }
                        for (j = bitRef + 1; j % 8 != 0; j++) { validMoves |= (1ul << j); if ((occupancyVariation[bitRef, i] & (1ul << j)) != 0) break; }
                        for (j = bitRef - 1; j % 8 != 7 && j >= 0; j--) { validMoves |= (1ul << j); if ((occupancyVariation[bitRef, i] & (1ul << j)) != 0) break; }

                        magicMovesRook[bitRef, magicIndex] = validMoves;
                    }
                    else
                    {
                        //generateOV(true);
                        magicIndex = (int)((occupancyVariation[bitRef, i] * magicNumberBishop[bitRef]) >> magicNumberShiftsBishop[bitRef]);

                        for (j = bitRef + 9; j % 8 != 0 && j <= 63; j += 9) { validMoves |= (1ul << j); if ((occupancyVariation[bitRef, i] & (1ul << j)) != 0) break; }
                        for (j = bitRef - 9; j % 8 != 7 && j >= 0; j -= 9) { validMoves |= (1ul << j); if ((occupancyVariation[bitRef, i] & (1ul << j)) != 0) break; }
                        for (j = bitRef + 7; j % 8 != 7 && j <= 63; j += 7)
                        {
                            validMoves |= (1ul << j);
                            if ((occupancyVariation[bitRef, i] & (1ul << j)) != 0)
                                break;
                        }
                        for (j = bitRef - 7; j % 8 != 0 && j >= 0; j -= 7)
                        {
                            validMoves |= (1ul << j);
                            if ((occupancyVariation[bitRef, i] & (1ul << j)) != 0)
                                break;
                        }

                        magicMovesBishop[bitRef, magicIndex] = validMoves;
                    }
                }
            }
        }
        public static void generateMagicNumbers(bool isRook)
        {
            int i, j, bitRef, variationCount;

            Random r = new Random();
            ulong magicNumber = 0;
            int index;

            for (bitRef = 0; bitRef <= 63; bitRef++)
            {
                int bitCount = BitHelper.NumberOfSetBits(isRook ? occupancyMaskRook[bitRef] : occupancyMaskBishop[bitRef]);
                variationCount = (int)(1ul << bitCount);
                bool fail = true;
                ulong[] usedBy = new ulong[(int)(1ul << bitCount)];

                int attempts = 0;

                do
                {
                    magicNumber = uLongRandom(r) & uLongRandom(r) & uLongRandom(r); // generate a random number with not many bits set
                    for (j = 0; j < variationCount; j++) usedBy[j] = 0;
                    attempts++;
                    //if(((ulong)BitHelper.NumberOfSetBits(isRook ? occupancyMaskRook[bitRef] : occupancyMaskBishop[bitRef]) & 0xFF00000000000000ul) < 6) continue;
                    for (i = 0, fail = false; i < variationCount && !fail; i++)
                    {
                        index = (int)((occupancyVariation[bitRef, i] * magicNumber) >> (64 - bitCount));

                        // fail if this index is used by an attack set that is incorrect for this occupancy variation
                        //fail = usedBy[index] != 0 && usedBy[index] != occupancyAttackSet[bitRef, i];
                        //if(i>=3000)
                        //{
                        //    attempts++;
                        //}
                        //usedBy[index] = occupancyAttackSet[bitRef, i];
                        //if(fail==false)
                        //{
                        //    attempts++;
                        //}
                        if (usedBy[index] == 0) usedBy[index] = occupancyAttackSet[bitRef, i];
                        else if (usedBy[index] != occupancyAttackSet[bitRef, i]) fail = true;
                    }
                }
                while (fail);

                if (isRook)
                {
                    magicNumberRook[bitRef] = magicNumber;
                    magicNumberShiftsRook[bitRef] = (64 - bitCount);
                }
                else
                {
                    magicNumberBishop[bitRef] = magicNumber;
                    magicNumberShiftsBishop[bitRef] = (64 - bitCount);
                }
            }
        }
        public static void init()
        {
            //findNumber();
            //generateOV(false);
            //System.IO.StreamWriter f = new System.IO.StreamWriter("rs.txt");
            generateOccupancyVariations(true);
            generateMoveDatabase(true);
            generateOccupancyVariations(false);
            generateMoveDatabase(false);
            //f.WriteLine("Magic number of Rook");
            //for(int i=0;i<64;i++)
            //{
            //    f.Write(magicNumberRook[i] + ",");
            //}
            //f.WriteLine();
            //f.WriteLine("Magic number of Bishop");
            //for (int i = 0; i < 64; i++)
            //{
            //    f.Write(magicNumberBishop[i] + ",");
            //}
            //f.WriteLine();
            //f.WriteLine("Shift count of Rook");
            //for (int i = 0; i < 64; i++)
            //{
            //    f.Write(magicNumberShiftsRook[i] + ",");
            //}
            //f.WriteLine();
            //f.WriteLine("Shift count of Bishop");
            //    for(int i=0;i<64;i++)
            //{
            //    f.Write(magicNumberShiftsBishop[i] + ",");
            //}
            //    f.WriteLine();
            //    f.WriteLine("Occupancy mask of Rook");
            //    for (int i = 0; i < 64; i++)
            //    {
            //        f.Write(occupancyMaskRook[i] + ",");
            //    }
            //    f.WriteLine();
            //    f.WriteLine("Occupancy mask of Bishop");
            //    for (int i = 0; i < 64; i++)
            //    {
            //        f.Write(occupancyMaskBishop[i] + ",");
            //    }

            //f.Close();

            //Console.WriteLine("Masks");
            //generateOccupancyMasks();
            //Console.WriteLine("ro");
            //generateOccupancyVariations(false);
            //generateOccupancyVariations(true);
            //Console.WriteLine("rn");
            //generateMagicNumbers(false);
            ////Console.WriteLine("rd");
            ////generateMoveDatabase(false);
            ////Console.WriteLine("bo");

            //Console.WriteLine("br");
            //generateMagicNumbers(true);
            //Console.WriteLine("bd");
            //generateMoveDatabase(true);

        }
        static ulong uLongRandom(Random rand)
        {
            byte[] buf = new byte[sizeof(ulong)];
            rand.NextBytes(buf);
            ulong uLongRand = BitConverter.ToUInt64(buf, 0);

            return uLongRand;
        }
        public static int count_1s(ulong b)
        {
            int r;
            for (r = 0; b != 0; r++, b &= b - 1) ;
            return r;
        }

        static int[] BitTable = {
  63, 30, 3, 32, 25, 41, 22, 33, 15, 50, 42, 13, 11, 53, 19, 34, 61, 29, 2,
  51, 21, 43, 45, 10, 18, 47, 1, 54, 9, 57, 0, 35, 62, 31, 40, 4, 49, 5, 52,
  26, 60, 6, 23, 44, 46, 27, 56, 16, 7, 39, 48, 24, 59, 14, 12, 55, 38, 28,
  58, 20, 37, 17, 36, 8
};

        static int pop_1st_bit(ref ulong bb)
        {
            ulong b = bb ^ (bb - 1);
            uint fold = (uint)((b & 0xffffffff) ^ (b >> 32));
            bb &= (bb - 1);
            return BitTable[(fold * 0x783a9b23) >> 26];
        }

        static ulong index_to_ulong(int index, int bits, ulong M)
        {
            int i, j;
            ulong m = M;
            ulong result = 0UL;
            for (i = 0; i < bits; i++)
            {
                j = pop_1st_bit(ref m);
                if ((index & (1 << i)) != 0) result |= (1UL << j);
            }
            return result;
        }

        static ulong rmask(int sq)
        {
            ulong result = 0UL;
            int rk = sq / 8, fl = sq % 8, r, f;
            for (r = rk + 1; r <= 6; r++) result |= (1UL << (fl + r * 8));
            for (r = rk - 1; r >= 1; r--) result |= (1UL << (fl + r * 8));
            for (f = fl + 1; f <= 6; f++) result |= (1UL << (f + rk * 8));
            for (f = fl - 1; f >= 1; f--) result |= (1UL << (f + rk * 8));
            return result;
        }

        static ulong bmask(int sq)
        {
            ulong result = 0UL;
            int rk = sq / 8, fl = sq % 8, r, f;
            for (r = rk + 1, f = fl + 1; r <= 6 && f <= 6; r++, f++) result |= (1UL << (f + r * 8));
            for (r = rk + 1, f = fl - 1; r <= 6 && f >= 1; r++, f--) result |= (1UL << (f + r * 8));
            for (r = rk - 1, f = fl + 1; r >= 1 && f <= 6; r--, f++) result |= (1UL << (f + r * 8));
            for (r = rk - 1, f = fl - 1; r >= 1 && f >= 1; r--, f--) result |= (1UL << (f + r * 8));
            return result;
        }

        static ulong ratt(int sq, ulong block)
        {
            ulong result = 0UL;
            int rk = sq / 8, fl = sq % 8, r, f;
            for (r = rk + 1; r <= 7; r++)
            {
                result |= (1UL << (fl + r * 8));
                if ((block & (1UL << (fl + r * 8))) != 0) break;
            }
            for (r = rk - 1; r >= 0; r--)
            {
                result |= (1UL << (fl + r * 8));
                if ((block & (1UL << (fl + r * 8))) != 0) break;
            }
            for (f = fl + 1; f <= 7; f++)
            {
                result |= (1UL << (f + rk * 8));
                if ((block & (1UL << (f + rk * 8))) != 0) break;
            }
            for (f = fl - 1; f >= 0; f--)
            {
                result |= (1UL << (f + rk * 8));
                if ((block & (1UL << (f + rk * 8))) != 0) break;
            }
            return result;
        }

        static ulong batt(int sq, ulong block)
        {
            ulong result = 0UL;
            int rk = sq / 8, fl = sq % 8, r, f;
            for (r = rk + 1, f = fl + 1; r <= 7 && f <= 7; r++, f++)
            {
                result |= (1UL << (f + r * 8));
                if ((block & (1UL << (f + r * 8))) != 0) break;
            }
            for (r = rk + 1, f = fl - 1; r <= 7 && f >= 0; r++, f--)
            {
                result |= (1UL << (f + r * 8));
                if ((block & (1UL << (f + r * 8))) != 0) break;
            }
            for (r = rk - 1, f = fl + 1; r >= 0 && f <= 7; r--, f++)
            {
                result |= (1UL << (f + r * 8));
                if ((block & (1UL << (f + r * 8))) != 0) break;
            }
            for (r = rk - 1, f = fl - 1; r >= 0 && f >= 0; r--, f--)
            {
                result |= (1UL << (f + r * 8));
                if ((block & (1UL << (f + r * 8))) != 0) break;
            }
            return result;
        }
        public static int transform(ulong b, ulong magic, int bits)
        {
            //#if defined(USE_32_BIT_MULTIPLICATIONS)
            //  return
            //    (unsigned)((int)b*(int)magic ^ (int)(b>>32)*(int)(magic>>32)) >> (32-bits);
            //#else
            return (int)((b * magic) >> (64 - bits));
            //#endif
        }

        //public static ulong find_magic1(int sq, int m, bool bishop)
        //{
        //    ulong[] b = new ulong[4096], a = new ulong[4096], used = new ulong[4096];
        //    ulong mask, magic;
        //    int i, j, k, n;
        //    bool fail;
        //    Random r = new Random();
        //    mask = bishop ? bmask(sq) : rmask(sq);
        //    n = count_1s(mask);

        //    for (i = 0; i < (1 << n); i++)
        //    {
        //        b[i] = index_to_ulong(i, n, mask);
        //        a[i] = bishop ? batt(sq, b[i]) : ratt(sq, b[i]);
        //    }
        //    for (k = 0; k < 100000000; k++)
        //    {
        //        magic = uLongRandom(r) & uLongRandom(r) & uLongRandom(r);
        //        if (count_1s((mask * magic) & 0xFF00000000000000UL) < 6) continue;
        //        for (i = 0; i < 4096; i++) used[i] = 0UL;
        //        for (i = 0, fail = false; !fail && i < (1 << n); i++)
        //        {
        //            j = transform(b[i], magic, m);
        //            if (used[j] == 0UL) used[j] = a[i];
        //            else if (used[j] != a[i]) fail = true;
        //        }
        //        if (!fail)
        //        {
        //            //occupancyVariation[sq, i] = b[i];
        //            if (bishop)
        //            {
        //                occupancyMaskBishop[sq] = mask;
        //                magicNumberShiftsBishop[sq] = 64 - m;
        //            }
        //            else
        //            {
        //                occupancyMaskRook[sq] = mask;
        //                magicNumberShiftsRook[sq] = 64 - m;
        //            }
        //            return magic;
        //        }
        //    }
        //    Console.WriteLine(false.ToString());
        //    return 0UL;
        //}
        public static ulong find_magic(int sq, int m, bool bishop)
        {
            ulong[] used = new ulong[4096];
            ulong mask, magic;
            int i, j, k, n;
            bool fail;
            Random r = new Random();
            mask = bishop ? bMasks[sq] : rMasks[sq];
            n = count_1s(mask);

            //for (i = 0; i < (1 << n); i++)
            //{
            //    b[i] = index_to_ulong(i, n, mask);
            //    a[i] = bishop ? batt(sq, b[i]) : ratt(sq, b[i]);
            //}

            for (k = 0; k < 100000000; k++)
            {
                magic = uLongRandom(r) & uLongRandom(r) & uLongRandom(r);
                if (count_1s((mask * magic) & 0xFF00000000000000UL) < 6) continue;
                Array.Clear(used, 0, 4096);
                used = new ulong[4096];
                for (i = 0, fail = false; !fail && i < (1 << n); i++)
                {
                    j = (int)((AV[sq, i] * magic) >> (64 - m));
                    if (used[j] == 0UL) used[j] = OV[sq, i];
                    else if (used[j] != OV[sq, i]) fail = true;
                }
                if (!fail)
                {
                    Console.WriteLine("Tryed {0} times", k);
                    //occupancyVariation[sq, i] = b[i];
                    if (bishop)
                    {
                        occupancyMaskBishop[sq] = mask;
                        magicNumberShiftsBishop[sq] = 64 - m;
                    }
                    else
                    {
                        occupancyMaskRook[sq] = mask;
                        magicNumberShiftsRook[sq] = 64 - m;
                    }
                    return magic;
                }
            }
            Console.WriteLine("Warrning! Generate not successfull.");
            return 0UL;
        }
        public static void generateOV(bool bishop)
        {
            ulong mask;
            int n;
            if (bishop)
            {


                for (int i = 0; i < 64; i++)
                {
                    mask = bmask(i);
                    n = count_1s(mask);
                    for (int j = 0; j < (1 << n); j++)
                    {
                        occupancyVariation[i, j] = index_to_ulong(i, n, mask);
                    }
                }
            }
            else
            {
                for (int i = 0; i < 64; i++)
                {
                    mask = rmask(i);
                    n = count_1s(mask);
                    for (int j = 0; j < (1 << n); j++)
                    {
                        occupancyVariation[i, j] = index_to_ulong(i, n, mask);
                    }
                }
            }
        }
        public static ulong[] bMasks, rMasks;
        public static ulong[,] OV, AV;
        public static int[] RBits = {
  12, 11, 11, 11, 11, 11, 11, 12,
  11, 10, 10, 10, 10, 10, 10, 11,
  11, 10, 10, 10, 10, 10, 10, 11,
  11, 10, 10, 10, 10, 10, 10, 11,
  11, 10, 10, 10, 10, 10, 10, 11,
  11, 10, 10, 10, 10, 10, 10, 11,
  11, 10, 10, 10, 10, 10, 10, 11,
  12, 11, 11, 11, 11, 11, 11, 12
};

        public static int[] BBits = {
  6, 5, 5, 5, 5, 5, 5, 6,
  5, 5, 5, 5, 5, 5, 5, 5,
  5, 5, 7, 7, 7, 7, 5, 5,
  5, 5, 7, 9, 9, 7, 5, 5,
  5, 5, 7, 9, 9, 7, 5, 5,
  5, 5, 7, 7, 7, 7, 5, 5,
  5, 5, 5, 5, 5, 5, 5, 5,
  6, 5, 5, 5, 5, 5, 5, 6
};
        public static int findMagicNumber()
        {
            int square;
            bMasks = new ulong[64];
            rMasks = new ulong[64];
            AV = new ulong[64, 4096];
            OV = new ulong[64, 4096];
            Console.WriteLine("Init for Rook");
            for (square = 0; square < 64; square++)
            {
                rMasks[square] = rmask(square);
                int n = count_1s(rMasks[square]);
                for (int i = 0; i < (1 << n); i++)
                {
                    AV[square, i] = index_to_ulong(i, n, rMasks[square]);
                    OV[square, i] = ratt(square, AV[square, i]);
                }
                //rtt=
            }
            Console.WriteLine("Finding Rook Move");
            for (square = 0; square < 64; square++)
            {
                magicNumberRook[square] = find_magic(square, RBits[square], false);
                // magicNumberRook[square] = find_magic1(square, RBits[square], false);
            }
            Console.WriteLine("init for Bishop");
            OV = new ulong[64, 4096];
            AV = new ulong[64, 4096];
            for (square = 0; square < 64; square++)
            {
                bMasks[square] = bmask(square);
                int n = count_1s(bMasks[square]);
                for (int i = 0; i < (1 << n); i++)
                {

                    AV[square, i] = index_to_ulong(i, n, bMasks[square]);
                    OV[square, i] = batt(square, AV[square, i]);
                }
                //rtt=
            }

            Console.WriteLine("Finding Bishop Move");
            for (square = 0; square < 64; square++)
            {
                magicNumberBishop[square] = find_magic(square, BBits[square], true);
            }

            return 0;
        }
        public static string captureMoves(ulong WP, ulong WN, ulong WB, ulong WR, ulong WQ, ulong WK, ulong BP, ulong BN, ulong BB, ulong BR, ulong BQ, ulong BK, ulong EP, bool CWK, bool CWQ, bool CBK, bool CBQ, bool white)
        {
            if(white)
            {
                NOT_MY_PIECES = BP|BN|BB|BR|BQ;
                MY_PIECES = WP | WN | WB | WR | WQ;
                OCCUPIED = WP | WN | WB | WR | WQ | WK | BP | BN | BB | BR | BQ | BK;
                EMPTY = ~OCCUPIED;
                StringBuilder list = new StringBuilder();
                list.Append(possibleWP(WP, BP, EP)).Append(possibleN(OCCUPIED, WN)).Append(possibleB(OCCUPIED, WB)).Append(possibleR(OCCUPIED, WR)).Append(possibleQ(OCCUPIED, WQ)).Append(possibleK(OCCUPIED, WK)).Append(possibleCW(WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK, CWK, CWQ));
                return list.ToString();
            }
            else
            {
                NOT_MY_PIECES = WP|WN|WB|WR|WQ;
                MY_PIECES = BP | BN | BB | BR | BQ;
                OCCUPIED = WP | WN | WB | WR | WQ | WK | BP | BN | BB | BR | BQ | BK;
                EMPTY = ~OCCUPIED;
                StringBuilder list = new StringBuilder();
                       list.Append(possibleBP(BP, WP, EP)).Append(possibleN(OCCUPIED, BN)).Append(possibleB(OCCUPIED, BB)).Append(possibleR(OCCUPIED, BR)).Append(possibleQ(OCCUPIED, BQ)).Append(possibleK(OCCUPIED, BK)).Append(possibleCB(WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK, CBK, CBQ));
                return list.ToString();
            }
        }
    }
}
