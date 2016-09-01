using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitBoardChessCreating
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
        static ulong EMPTY;
        static ulong OCCUPIED;
        static ulong OCCUPIED_H;
        static ulong OCCUPIED_D;
        static ulong OCCUPIED_AD;
        static ulong WHITE_PIECES;
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
            //REMINDER: requires OCCUPIED to be up to date
            ulong binaryS = 1ul << s;
            ulong possibilitiesHorizontal = (OCCUPIED - 2 * binaryS) ^ BitHelper.reverse(BitHelper.reverse(OCCUPIED) - 2 * BitHelper.reverse(binaryS));
            ulong possibilitiesVertical = ((OCCUPIED & FileMasks8[s % 8]) - (2 * binaryS)) ^ BitHelper.reverse(BitHelper.reverse(OCCUPIED & FileMasks8[s % 8]) - (2 * BitHelper.reverse(binaryS)));
            return (possibilitiesHorizontal & RankMasks8[s / 8]) | (possibilitiesVertical & FileMasks8[s % 8]);
        }
        static ulong DAndAntiDMoves(int s)
        {
            //REMINDER: requires OCCUPIED to be up to date
            ulong binaryS = 1ul << s;
            ulong possibilitiesDiagonal = ((OCCUPIED & DiagonalMasks8[(s / 8) + (s % 8)]) - (2 * binaryS)) ^ BitHelper.reverse(BitHelper.reverse(OCCUPIED & DiagonalMasks8[(s / 8) + (s % 8)]) - (2 * BitHelper.reverse(binaryS)));
            ulong possibilitiesAntiDiagonal = ((OCCUPIED & AntiDiagonalMasks8[(s / 8) + 7 - (s % 8)]) - (2 * binaryS)) ^ BitHelper.reverse(BitHelper.reverse(OCCUPIED & AntiDiagonalMasks8[(s / 8) + 7 - (s % 8)]) - (2 * BitHelper.reverse(binaryS)));
            return (possibilitiesDiagonal & DiagonalMasks8[(s / 8) + (s % 8)]) | (possibilitiesAntiDiagonal & AntiDiagonalMasks8[(s / 8) + 7 - (s % 8)]);
        }
        public static ulong makeMove(ulong board, string move, char type) {
        if (char.IsDigit(move[3])) {//'regular' move
            int start = (move[0]-'0')*8 + move[1]-'0';
            int end = (move[2]-'0')*8 + move[3]-'0';
            if (((board>>start)&1)==1) {board&=~(1ul<<start); board|=(1ul<<end);} else {board&=~(1ul<<end);}
        } else if (move[3]=='P') {//pawn promotion
            int start, end;
            if (char.IsUpper(move[2])) {
                start=BitHelper.getNumberOfTrailingZeros(FileMasks8[move[0]-'0']&RankMasks8[1]);
                end=BitHelper.getNumberOfTrailingZeros(FileMasks8[move[1]-'0']&RankMasks8[0]);
            } else {
                start=BitHelper.getNumberOfTrailingZeros(FileMasks8[move[0]-'0']&RankMasks8[6]);
                end=BitHelper.getNumberOfTrailingZeros(FileMasks8[move[1]-'0']&RankMasks8[7]);
            }
            if (type==move[2]) {board|=(1ul<<end);} else {board&=~(1ul<<start); board&=~(1ul<<end);}
        } else if (move[3]=='E') {//en passant
            int start, end;
            if (move[2]=='W') {
                start=BitHelper.getNumberOfTrailingZeros(FileMasks8[move[0]-'0']&RankMasks8[3]);
                end=BitHelper.getNumberOfTrailingZeros(FileMasks8[move[1]-'0']&RankMasks8[2]);
                board&=~(FileMasks8[move[1]-'0']&RankMasks8[3]);
            } else {
                start=BitHelper.getNumberOfTrailingZeros(FileMasks8[move[0]-'0']&RankMasks8[4]);
                end=BitHelper.getNumberOfTrailingZeros(FileMasks8[move[1]-'0']&RankMasks8[5]);
                board&=~(FileMasks8[move[1]-'0']&RankMasks8[4]);
            }
            if (((board>>start)&1)==1) {board&=~(1ul<<start); board|=(1ul<<end);}
        } else {
            Console.WriteLine("ERROR: Invalid move type");
        }
        return board;
    }
        public static ulong makeMoveCastle(ulong rookBoard, ulong kingBoard, string move, char type) {
            int start = (move[0]-'0')*8 + move[1]-'0';
        if ((((kingBoard>>start)&1)==1)&&(("0402".Equals(move))||("0406".Equals(move))||("7472".Equals(move))||("7476".Equals(move)))) {//'regular' move
            if (type=='R') {
                switch (move) {
                    case "7472": rookBoard&=~(1ul<<CASTLE_ROOKS[1]); rookBoard|=(1ul<<(CASTLE_ROOKS[1]+3));
                        break;
                    case "7476": rookBoard&=~(1ul<<CASTLE_ROOKS[0]); rookBoard|=(1ul<<(CASTLE_ROOKS[0]-2));
                        break;
                }
            } else {
                switch (move) {
                    case "0402": rookBoard&=~(1ul<<CASTLE_ROOKS[3]); rookBoard|=(1ul<<(CASTLE_ROOKS[3]+3));
                        break;
                    case "0406": rookBoard&=~(1ul<<CASTLE_ROOKS[2]); rookBoard|=(1ul<<(CASTLE_ROOKS[2]-2));
                        break;
                }
            }
        }
        return rookBoard;
    }
        public static ulong makeMoveEP(ulong board, string move) {
        if (char.IsDigit(move[3])) {
            int start = (move[0]-'0')*8 + move[1]-'0';
            if ((Math.Abs(move[0]-move[2])==2)&&(((board>>start)&1)==1)) {//pawn double push
                return FileMasks8[move[1]-'0'];
            }
        }
        return 0;
    }
        public static string possibleMovesW(ulong WP, ulong WN, ulong WB, ulong WR, ulong WQ, ulong WK, ulong BP, ulong BN, ulong BB, ulong BR, ulong BQ, ulong BK, ulong EP, bool CWK, bool CWQ, bool CBK, bool CBQ)
        {
            NOT_MY_PIECES = ~(WP | WN | WB | WR | WQ | WK | BK);//added BK to avoid illegal capture
            MY_PIECES = WP | WN | WB | WR | WQ;//omitted WK to avoid illegal capture
            OCCUPIED = WP | WN | WB | WR | WQ | WK | BP | BN | BB | BR | BQ | BK;
            EMPTY = ~OCCUPIED;
            string list = possibleWP(WP, BP, EP) +
                    possibleN(OCCUPIED, WN) +
                    possibleB(OCCUPIED, WB) +
                    possibleR(OCCUPIED, WR) +
                    possibleQ(OCCUPIED, WQ) +
                    possibleK(OCCUPIED, WK) +
                    possibleCW(WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK, CWK, CWQ);
            return list;
        }
        public static string possibleMovesB(ulong WP, ulong WN, ulong WB, ulong WR, ulong WQ, ulong WK, ulong BP, ulong BN, ulong BB, ulong BR, ulong BQ, ulong BK, ulong EP, bool CWK, bool CWQ, bool CBK, bool CBQ)
        {
            NOT_MY_PIECES = ~(BP | BN | BB | BR | BQ | BK | WK);//added WK to avoid illegal capture
            MY_PIECES = BP | BN | BB | BR | BQ;//omitted BK to avoid illegal capture
            OCCUPIED = WP | WN | WB | WR | WQ | WK | BP | BN | BB | BR | BQ | BK;
            EMPTY = ~OCCUPIED;
            string list = possibleBP(BP, WP, EP) +
                    possibleN(OCCUPIED, BN) +
                    possibleB(OCCUPIED, BB) +
                    possibleR(OCCUPIED, BR) +
                    possibleQ(OCCUPIED, BQ) +
                    possibleK(OCCUPIED, BK) +
                    possibleCB(WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK, CBK, CBQ);
            return list;
        }
        public static string possibleWP(ulong WP, ulong BP, ulong EP)
        {
            string list = "";
            //x1,y1,x2,y2
            ulong PAWN_MOVES = (WP >> 7) & NOT_MY_PIECES & OCCUPIED & ~RANK_8 & ~FILE_A;//capture right
            ulong possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            while (possibility != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(possibility);
                list += "" + (index / 8 + 1) + (index % 8 - 1) + (index / 8) + (index % 8);
                PAWN_MOVES &= ~possibility;
                possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            }
            PAWN_MOVES = (WP >> 9) & NOT_MY_PIECES & OCCUPIED & ~RANK_8 & ~FILE_H;//capture left
            possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            while (possibility != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(possibility);
                list += "" + (index / 8 + 1) + (index % 8 + 1) + (index / 8) + (index % 8);
                PAWN_MOVES &= ~possibility;
                possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            }
            PAWN_MOVES = (WP >> 8) & EMPTY & ~RANK_8;//move 1 forward
            possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            while (possibility != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(possibility);
                list += "" + (index / 8 + 1) + (index % 8) + (index / 8) + (index % 8);
                PAWN_MOVES &= ~possibility;
                possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            }
            PAWN_MOVES = (WP >> 16) & EMPTY & (EMPTY >> 8) & RANK_4;//move 2 forward
            possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            while (possibility != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(possibility);
                list += "" + (index / 8 + 2) + (index % 8) + (index / 8) + (index % 8);
                PAWN_MOVES &= ~possibility;
                possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            }
            //y1,y2,Promotion Type,"P"
            PAWN_MOVES = (WP >> 7) & NOT_MY_PIECES & OCCUPIED & RANK_8 & ~FILE_A;//pawn promotion by capture right
            possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            while (possibility != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(possibility);
                list += "" + (index % 8 - 1) + (index % 8) + "QP" + (index % 8 - 1) + (index % 8) + "RP" + (index % 8 - 1) + (index % 8) + "BP" + (index % 8 - 1) + (index % 8) + "NP";
                PAWN_MOVES &= ~possibility;
                possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            }
            PAWN_MOVES = (WP >> 9) & NOT_MY_PIECES & OCCUPIED & RANK_8 & ~FILE_H;//pawn promotion by capture left
            possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            while (possibility != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(possibility);
                list += "" + (index % 8 + 1) + (index % 8) + "QP" + (index % 8 + 1) + (index % 8) + "RP" + (index % 8 + 1) + (index % 8) + "BP" + (index % 8 + 1) + (index % 8) + "NP";
                PAWN_MOVES &= ~possibility;
                possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            }
            PAWN_MOVES = (WP >> 8) & EMPTY & RANK_8;//pawn promotion by move 1 forward
            possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            while (possibility != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(possibility);
                list += "" + (index % 8) + (index % 8) + "QP" + (index % 8) + (index % 8) + "RP" + (index % 8) + (index % 8) + "BP" + (index % 8) + (index % 8) + "NP";
                PAWN_MOVES &= ~possibility;
                possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            }
            //y1,y2,"WE"
            //en passant right
            possibility = (WP << 1) & BP & RANK_5 & ~FILE_A & EP;//shows piece to remove, not the destination
            if (possibility != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(possibility);
                list += "" + (index % 8 - 1) + (index % 8) + "WE";
            }
            //en passant left
            possibility = (WP >> 1) & BP & RANK_5 & ~FILE_H & EP;//shows piece to remove, not the destination
            if (possibility != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(possibility);
                list += "" + (index % 8 + 1) + (index % 8) + "WE";
            }
            return list;
        }
        public static string possibleBP(ulong BP, ulong WP, ulong EP)
        {
            string list = "";
            //x1,y1,x2,y2
            ulong PAWN_MOVES = (BP << 7) & NOT_MY_PIECES & OCCUPIED & ~RANK_1 & ~FILE_H;//capture right
            ulong possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            while (possibility != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(possibility);
                list += "" + (index / 8 - 1) + (index % 8 + 1) + (index / 8) + (index % 8);
                PAWN_MOVES &= ~possibility;
                possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            }
            PAWN_MOVES = (BP << 9) & NOT_MY_PIECES & OCCUPIED & ~RANK_1 & ~FILE_A;//capture left
            possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            while (possibility != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(possibility);
                list += "" + (index / 8 - 1) + (index % 8 - 1) + (index / 8) + (index % 8);
                PAWN_MOVES &= ~possibility;
                possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            }
            PAWN_MOVES = (BP << 8) & EMPTY & ~RANK_1;//move 1 forward
            possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            while (possibility != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(possibility);
                list += "" + (index / 8 - 1) + (index % 8) + (index / 8) + (index % 8);
                PAWN_MOVES &= ~possibility;
                possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            }
            PAWN_MOVES = (BP << 16) & EMPTY & (EMPTY << 8) & RANK_5;//move 2 forward
            possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            while (possibility != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(possibility);
                list += "" + (index / 8 - 2) + (index % 8) + (index / 8) + (index % 8);
                PAWN_MOVES &= ~possibility;
                possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            }
            //y1,y2,Promotion Type,"P"
            PAWN_MOVES = (BP << 7) & NOT_MY_PIECES & OCCUPIED & RANK_1 & ~FILE_H;//pawn promotion by capture right
            possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            while (possibility != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(possibility);
                list += "" + (index % 8 + 1) + (index % 8) + "qP" + (index % 8 + 1) + (index % 8) + "rP" + (index % 8 + 1) + (index % 8) + "bP" + (index % 8 + 1) + (index % 8) + "nP";
                PAWN_MOVES &= ~possibility;
                possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            }
            PAWN_MOVES = (BP << 9) & NOT_MY_PIECES & OCCUPIED & RANK_1 & ~FILE_A;//pawn promotion by capture left
            possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            while (possibility != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(possibility);
                list += "" + (index % 8 - 1) + (index % 8) + "qP" + (index % 8 - 1) + (index % 8) + "rP" + (index % 8 - 1) + (index % 8) + "bP" + (index % 8 - 1) + (index % 8) + "nP";
                PAWN_MOVES &= ~possibility;
                possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            }
            PAWN_MOVES = (BP << 8) & EMPTY & RANK_1;//pawn promotion by move 1 forward
            possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            while (possibility != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(possibility);
                list += "" + (index % 8) + (index % 8) + "qP" + (index % 8) + (index % 8) + "rP" + (index % 8) + (index % 8) + "bP" + (index % 8) + (index % 8) + "nP";
                PAWN_MOVES &= ~possibility;
                possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
            }
            //y1,y2,"BE"
            //en passant right
            possibility = (BP >> 1) & WP & RANK_4 & ~FILE_H & EP;//shows piece to remove, not the destination
            if (possibility != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(possibility);
                list += "" + (index % 8 + 1) + (index % 8) + "BE";
            }
            //en passant left
            possibility = (BP << 1) & WP & RANK_4 & ~FILE_A & EP;//shows piece to remove, not the destination
            if (possibility != 0)
            {
                int index = BitHelper.getNumberOfTrailingZeros(possibility);
                list += "" + (index % 8 - 1) + (index % 8) + "BE";
            }
            return list;
        }
        public static string possibleN(ulong OCCUPIED, ulong N)
        {
            string list = "";
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
                    list += "" + (iLocation / 8) + (iLocation % 8) + (index / 8) + (index % 8);
                    possibility &= ~j;
                    j = possibility & ~(possibility - 1);
                }
                N &= ~i;
                i = N & ~(N - 1);
            }
            return list;
        }
        public static string possibleB(ulong OCCUPIED, ulong B)
        {
            string list = "";
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
                    list += "" + (iLocation / 8) + (iLocation % 8) + (index / 8) + (index % 8);
                    possibility &= ~j;
                    j = possibility & ~(possibility - 1);
                }
                B &= ~i;
                i = B & ~(B - 1);
            }
            return list;
        }
        public static string possibleR(ulong OCCUPIED, ulong R)
        {
            string list = "";
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
                    list += "" + (iLocation / 8) + (iLocation % 8) + (index / 8) + (index % 8);
                    possibility &= ~j;
                    j = possibility & ~(possibility - 1);
                }
                R &= ~i;
                i = R & ~(R - 1);
            }
            return list;
        }
        public static string possibleQ(ulong OCCUPIED, ulong Q)
        {
            string list = "";
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
                    list += "" + (iLocation / 8) + (iLocation % 8) + (index / 8) + (index % 8);
                    possibility &= ~j;
                    j = possibility & ~(possibility - 1);
                }
                Q &= ~i;
                i = Q & ~(Q - 1);
            }
            return list;
        }
        public static string possibleK(ulong OCCUPIED, ulong K)
        {
            string list = "";
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
                list += "" + (iLocation / 8) + (iLocation % 8) + (index / 8) + (index % 8);
                possibility &= ~j;
                j = possibility & ~(possibility - 1);
            }
            return list;
        }
        public static string possibleCW(ulong WP, ulong WN, ulong WB, ulong WR, ulong WQ, ulong WK, ulong BP, ulong BN, ulong BB, ulong BR, ulong BQ, ulong BK, bool CWK, bool CWQ)
        {
            string list = "";
            ulong UNSAFE = unSafeForWhite(WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK);
            if ((UNSAFE & WK) == 0)
            {
                if (CWK && (((1ul << CASTLE_ROOKS[0]) & WR) != 0))
                {
                    if (((OCCUPIED | UNSAFE) & ((1ul << 61) | (1ul << 62))) == 0)
                    {
                        list += "7476";
                    }
                }
                if (CWQ && (((1ul << CASTLE_ROOKS[1]) & WR) != 0))
                {
                    if (((OCCUPIED | (UNSAFE & ~(1ul << 57))) & ((1ul << 57) | (1ul << 58) | (1ul << 59))) == 0)
                    {
                        list += "7472";
                    }
                }
            }
            return list;
        }
        public static string possibleCB(ulong WP, ulong WN, ulong WB, ulong WR, ulong WQ, ulong WK, ulong BP, ulong BN, ulong BB, ulong BR, ulong BQ, ulong BK, bool CBK, bool CBQ)
        {
            string list = "";
            ulong UNSAFE = unSafeForBlack(WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK);
            if ((UNSAFE & BK) == 0)
            {
                if (CBK && (((1ul << CASTLE_ROOKS[2]) & BR) != 0))
                {
                    if (((OCCUPIED | UNSAFE) & ((1ul << 5) | (1ul << 6))) == 0)
                    {
                        list += "0406";
                    }
                }
                if (CBQ && (((1ul << CASTLE_ROOKS[3]) & BR) != 0))
                {
                    if (((OCCUPIED | (UNSAFE & ~(1ul << 1))) & ((1ul << 1) | (1ul << 2) | (1ul << 3))) == 0)
                    {
                        list += "0402";
                    }
                }
            }
            return list;
        }
        public static ulong unSafeForBlack(ulong WP, ulong WN, ulong WB, ulong WR, ulong WQ, ulong WK, ulong BP, ulong BN, ulong BB, ulong BR, ulong BQ, ulong BK) {
        ulong unSafe;
        OCCUPIED=WP|WN|WB|WR|WQ|WK|BP|BN|BB|BR|BQ|BK;
        //pawn
        unSafe=((WP>>7)&~FILE_A);//pawn capture right
        unSafe|=((WP>>9)&~FILE_H);//pawn capture left
        ulong possibility;
        //knight
        ulong i=WN&~(WN-1);
        while(i != 0)
        {
            int iLocation=BitHelper.getNumberOfTrailingZeros(i);
            if (iLocation>18)
            {
                possibility=KNIGHT_SPAN<<(iLocation-18);
            }
            else {
                possibility=KNIGHT_SPAN>>(18-iLocation);
            }
            if (iLocation%8<4)
            {
                possibility &=~FILE_GH;
            }
            else {
                possibility &=~FILE_AB;
            }
            unSafe |= possibility;
            WN&=~i;
            i=WN&~(WN-1);
        }
        //bishop/queen
        ulong QB=WQ|WB;
        i=QB&~(QB-1);
        while(i != 0)
        {
            int iLocation=BitHelper.getNumberOfTrailingZeros(i);
            possibility=DAndAntiDMoves(iLocation);
            unSafe |= possibility;
            QB&=~i;
            i=QB&~(QB-1);
        }
        //rook/queen
        ulong QR=WQ|WR;
        i=QR&~(QR-1);
        while(i != 0)
        {
            int iLocation=BitHelper.getNumberOfTrailingZeros(i);
            possibility=HAndVMoves(iLocation);
            unSafe |= possibility;
            QR&=~i;
            i=QR&~(QR-1);
        }
        //king
        int iLocationK=BitHelper.getNumberOfTrailingZeros(WK);
        if (iLocationK>9)
        {
            possibility=KING_SPAN<<(iLocationK-9);
        }
        else {
            possibility=KING_SPAN>>(9-iLocationK);
        }
        if (iLocationK%8<4)
        {
            possibility &=~FILE_GH;
        }
        else {
            possibility &=~FILE_AB;
        }
        unSafe |= possibility;
        return unSafe;
    }
        public static ulong unSafeForWhite(ulong WP, ulong WN, ulong WB, ulong WR, ulong WQ, ulong WK, ulong BP, ulong BN, ulong BB, ulong BR, ulong BQ, ulong BK) {
        ulong unSafe;
        OCCUPIED=WP|WN|WB|WR|WQ|WK|BP|BN|BB|BR|BQ|BK;
        //pawn
        unSafe=((BP<<7)&~FILE_H);//pawn capture right
        unSafe|=((BP<<9)&~FILE_A);//pawn capture left
        ulong possibility;
        //knight
        ulong i=BN&~(BN-1);
        while(i != 0)
        {
            int iLocation=BitHelper.getNumberOfTrailingZeros(i);
            if (iLocation>18)
            {
                possibility=KNIGHT_SPAN<<(iLocation-18);
            }
            else {
                possibility=KNIGHT_SPAN>>(18-iLocation);
            }
            if (iLocation%8<4)
            {
                possibility &=~FILE_GH;
            }
            else {
                possibility &=~FILE_AB;
            }
            unSafe |= possibility;
            BN&=~i;
            i=BN&~(BN-1);
        }
        //bishop/queen
        ulong QB=BQ|BB;
        i=QB&~(QB-1);
        while(i != 0)
        {
            int iLocation=BitHelper.getNumberOfTrailingZeros(i);
            possibility=DAndAntiDMoves(iLocation);
            unSafe |= possibility;
            QB&=~i;
            i=QB&~(QB-1);
        }
        //rook/queen
        ulong QR=BQ|BR;
        i=QR&~(QR-1);
        while(i != 0)
        {
            int iLocation=BitHelper.getNumberOfTrailingZeros(i);
            possibility=HAndVMoves(iLocation);
            unSafe |= possibility;
            QR&=~i;
            i=QR&~(QR-1);
        }
        //king
        int iLocationK=BitHelper.getNumberOfTrailingZeros(BK);
        if (iLocationK>9)
        {
            possibility=KING_SPAN<<(iLocationK-9);
        }
        else {
            possibility=KING_SPAN>>(9-iLocationK);
        }
        if (iLocationK%8<4)
        {
            possibility &=~FILE_GH;
        }
        else {
            possibility &=~FILE_AB;
        }
        unSafe |= possibility;
        return unSafe;
    }
        public static void drawBitboard(ulong bitBoard) {
        string[,] chessBoard=new string[8,8];
        for (int i=0;i<64;i++) {
            chessBoard[i/8,i%8]="";
        }
        for (int i=0;i<64;i++) {
            if (((bitBoard>>i)&1)==1) {chessBoard[i/8,i%8]="P";}
            if ("".Equals(chessBoard[i/8,i%8])) {chessBoard[i/8,i%8]=" ";}
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
        public static void timeExperiment(ulong WP)
        {
            int loopLength = 1000;
            DateTime startTime = DateTime.Now;
            tEMethodA(loopLength, WP);
            DateTime endTime = DateTime.Now;
            Console.WriteLine("That took " + (endTime - startTime).TotalMilliseconds + " milliseconds for the first method");
            startTime = DateTime.Now;
            tEMethodB(loopLength, WP);
            endTime = DateTime.Now;
            Console.WriteLine("That took " + (endTime - startTime).TotalMilliseconds + " milliseconds for the second method");
        }
        public static void tEMethodA(int loopLength, ulong WP)
        {
            for (int loop = 0; loop < loopLength; loop++)
            {
                ulong PAWN_MOVES = (WP >> 7) & MY_PIECES & ~RANK_8 & ~FILE_A;//capture right
                string list = "";
                for (int i = 0; i < 64; i++)
                {
                    if (((PAWN_MOVES >> i) & 1) == 1)
                    {
                        list += "" + (i / 8 + 1) + (i % 8 - 1) + (i / 8) + (i % 8);
                    }
                }
            }
        }
        public static void tEMethodB(int loopLength, ulong WP)
        {
            for (int loop = 0; loop < loopLength; loop++)
            {
                ulong PAWN_MOVES = (WP >> 7) & MY_PIECES & ~RANK_8 & ~FILE_A;//capture right
                string list = "";
                ulong possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
                while (possibility != 0)
                {
                    //drawBitboard(PAWN_MOVES);
                    int index = BitHelper.getNumberOfTrailingZeros(possibility);
                    list += "" + (index / 8 + 1) + (index % 8 - 1) + (index / 8) + (index % 8);
                    PAWN_MOVES &= ~(possibility);
                    possibility = PAWN_MOVES & ~(PAWN_MOVES - 1);
                }
            }
        }
    }
}
