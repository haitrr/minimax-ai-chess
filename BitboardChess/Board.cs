using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class Board
    {
        //#region Pieces
        ////Piece
        //ulong Pawns;
        //ulong Knights;
        //ulong Bishops;
        //ulong Rooks;
        //ulong Queens;
        //ulong Kings;
        ////color
        //ulong black;
        //ulong white;
        ulong[] pieces;
        ////common use table
        ulong allWhitePiece;
        
            //get
            //{
            //    //return (Pawns | Rooks | Knights | Bishops |
            //    //        Queens | Kings) & white;
            //}

        
        ulong allBlackPiece;
        
            //get
            //{
            //    //return (Pawns | Rooks | Knights | Bishops |
            //    //    Queens | Kings) & black;
            //}
        
        #region Pieces
        //white Pieces
        ulong whitePawns;
        ulong whiteKnights;
        ulong whiteBishops;
        ulong whiteRooks;
        ulong whiteQueens;
        ulong whiteKing;
        //black Pieces
        ulong blackPawns;
        ulong blackKnights;
        ulong blackBishops;
        ulong blackRooks;
        ulong blackQueens;
        ulong blackKing;
        string[,] chessBoard;
        ulong occupied;
        #endregion
        #region clearMashRankFileDiagonal
        public const ulong clearFile0Mask = 0xFEFEFEFEFEFEFEFE;
        public const ulong clearFile1Mask = 0xFDFDFDFDFDFDFDFD;
        public const ulong clearFile6Mask = 0xBFBFBFBFBFBFBFBF;
        public const ulong clearFile7Mask = 0x7F7F7F7F7F7F7F7F;
        #endregion
        public void init()
        {
            chessBoard=new string[8,8]{
                {"r","n","b","q","k","b","n","r"},
                {"p","p","p","p","p","p","p","p"},
                {"","","","","","","",""},
                {"","","","","","","",""},
                {"","","","","","","",""},
                {"","","","","","","",""},
                {"P","P","P","P","P","P","P","P"},
                {"R","N","B","Q","K","B","N","R"}
            };
        }
        public void arrayToBitBoard()
        {
            string binary;
            for(int i=0;i<64;i++)
            {
                binary="0000000000000000000000000000000000000000000000000000000000000000";
                binary=binary.Substring(i+1)+"1"+binary.Substring(0,i);
                switch(chessBoard[i/8,i%8])
                {
                    case "P": whitePawns+= convertStringToBitboard(binary);break;
                        case "N": whiteKnights+= convertStringToBitboard(binary);break;
                        case "R": whiteRooks+= convertStringToBitboard(binary);break;
                        case "K": whiteKing+= convertStringToBitboard(binary);break;
                        case "Q": whiteQueens+= convertStringToBitboard(binary);break;
                        case "B": whiteBishops+= convertStringToBitboard(binary);break;
                        case "p": blackPawns+= convertStringToBitboard(binary);break;
                        case "n": blackKnights+= convertStringToBitboard(binary);break;
                        case "r": blackRooks+= convertStringToBitboard(binary);break;
                        case "k": blackKing+= convertStringToBitboard(binary);break;
                        case "q": blackQueens+= convertStringToBitboard(binary);break;
                        case "b": blackBishops+= convertStringToBitboard(binary);break;
                    default: break;
                }
            }
        
        }
        public ulong convertStringToBitboard(string binary)
        {
                return Convert.ToUInt64(binary,2);
        }
        public void drawArray()
        {

        }
        ulong rankMask(int sq) { return (ulong)(0xff) << (sq & 56); }

        ulong fileMask(int sq) { return (ulong)0x0101010101010101 << (sq & 7); }

        ulong diagonalMask(int sq)
        {
            const ulong maindia = (ulong)(0x8040201008040201);
            int diag = 8 * (sq & 7) - (sq & 56);
            int nort = -diag & (diag >> 31);
            int sout = diag & (-diag >> 31);
            return (maindia >> sout) << nort;

        }
        ulong singleBit(int sq) { return (ulong)1 << sq; }
        ulong rankMaskEx(int sq) { return ((ulong)(1) << sq) ^ rankMask(sq); }
        ulong fileMaskEx(int sq) { return ((ulong)(1) << sq) ^ fileMask(sq); }
        ulong diagonalMaskEx(int sq) { return ((ulong)(1) << sq) ^ diagonalMask(sq); }
        ulong antiDiagMaskEx(int sq) { return ((ulong)(1) << sq) ^ antiDiagMask(sq); }
        ulong antiDiagMask(int sq)
        {
            const ulong maindia = (ulong)(0x0102040810204080);
            int diag = 56 - 8 * (sq & 7) - (sq & 56);
            int nort = -diag & (diag >> 31);
            int sout = diag & (-diag >> 31);
            return (maindia >> sout) << nort;
        }
        public ulong SwapBytes(ulong value)
        {
            ulong uvalue = value;
            ulong swapped =
                 ((0x00000000000000FF) & (uvalue >> 56)
                 | (0x000000000000FF00) & (uvalue >> 40)
                 | (0x0000000000FF0000) & (uvalue >> 24)
                 | (0x00000000FF000000) & (uvalue >> 8)
                 | (0x000000FF00000000) & (uvalue << 8)
                 | (0x0000FF0000000000) & (uvalue << 24)
                 | (0x00FF000000000000) & (uvalue << 40)
                 | (0xFF00000000000000) & (uvalue << 56));
            return swapped;
        }
        public ulong getKingAvailableMove(ulong location, ulong allAllyPieces)
        {
            ulong kingClipFile7 = location & clearFile7Mask;
            ulong kingClipFile0 = location & clearFile0Mask;
            ulong dest1 = kingClipFile7 << 7;
            ulong dest2 = location << 8;
            ulong dest3 = kingClipFile7 << 9;
            ulong dest4 = kingClipFile7 << 1;

            ulong dest5 = kingClipFile0 >> 7;
            ulong dest6 = location >> 8;
            ulong dest7 = kingClipFile0 >> 9;
            ulong dest8 = kingClipFile0 >> 1;
            ulong kingMoves = dest1 | dest2 | dest3 | dest4 | dest5 | dest6 | dest7 | dest8;
            ulong validMoves = kingMoves & ~allBlackPiece;
            return validMoves;
        }

        public ulong getKnightAvailableMove(ulong location, ulong allAllyPieces)
        {
            ulong dest1Clip = clearFile0Mask & clearFile1Mask;
            ulong dest2Clip = clearFile0Mask;
            ulong dest3Clip = clearFile7Mask;
            ulong dest4Clip = clearFile7Mask & clearFile6Mask;

            ulong dest5Clip = clearFile7Mask & clearFile6Mask;
            ulong dest6Clip = clearFile7Mask;
            ulong dest7Clip = clearFile0Mask;
            ulong dest8Clip = clearFile0Mask & clearFile1Mask;

            /* The clipping masks we just created will be used to ensure that no
                under or overflow positions are computed when calculating the
                possible moves of the knight in certain files. */

            ulong dest1 = (location & dest1Clip) << 6;
            ulong dest2 = (location & dest2Clip) << 15;
            ulong dest3 = (location & dest3Clip) << 17;
            ulong dest4 = (location & dest4Clip) << 10;

            ulong dest5 = (location & dest5Clip) >> 6;
            ulong dest6 = (location & dest6Clip) >> 15;
            ulong dest7 = (location & dest7Clip) >> 17;
            ulong dest8 = (location & dest8Clip) >> 10;

            ulong knightMove = dest1 | dest2 | dest3 | dest4 | dest5 | dest6 |
                            dest7 | dest8;
            ulong validMove = knightMove & ~allAllyPieces;
            return validMove;
        }
        public ulong getBlackPawnAvailableMove(ulong location, ulong allPieces, ulong allWhitePieces)
        {
            /* check the single space infront of the white pawn */
            ulong oneStep = (location << 8) & ~allPieces;

            /* for all moves that came from rank 2 (home row) and passed the above 
                filter, thereby being on rank 3, check and see if I can move forward 
                one more */
            ulong twoStep = ((oneStep & rankMask(9)) << 8) & ~allPieces;

            /* the union of the movements dictate the possible moves forward 
                available */
            ulong validMove = oneStep | twoStep;

            /* next we calculate the pawn attacks */

            /* check the left side of the pawn, minding the underflow File A */
            ulong leftAttack = (location & clearFile0Mask) << 7;

            /* then check the right side of the pawn, minding the overflow File H */
            ulong rightAttack = (location & clearFile7Mask) << 9;

            /* the union of the left and right attacks together make up all the 
                possible attacks */
            ulong attacks = leftAttack | rightAttack;

            /* Calculate where I can _actually_ attack something */
            ulong validAttacks = attacks & allWhitePieces;

            /* then we combine the two situations in which a white pawn can legally 
                attack/move. */
            validMove = validMove | validAttacks;

            return validMove;
        }
        public ulong getWhitePawnAvailableMove(ulong location, ulong allPieces, ulong allWhitePieces)
        {
            /* check the single space infront of the white pawn */
            ulong oneStep = (location >> 8) & ~allPieces;

            /* for all moves that came from rank 2 (home row) and passed the above 
                filter, thereby being on rank 3, check and see if I can move forward 
                one more */
            ulong twoStep = ((oneStep & rankMask(22)) >> 8) & ~allPieces;

            /* the union of the movements dictate the possible moves forward 
                available */
            ulong validMove = oneStep | twoStep;

            /* next we calculate the pawn attacks */

            /* check the left side of the pawn, minding the underflow File A */
            ulong leftAttack = (location & clearFile0Mask) >> 7;

            /* then check the right side of the pawn, minding the overflow File H */
            ulong rightAttack = (location & clearFile7Mask) >> 9;

            /* the union of the left and right attacks together make up all the 
                possible attacks */
            ulong attacks = leftAttack | rightAttack;

            /* Calculate where I can _actually_ attack something */
            ulong validAttacks = attacks & allWhitePieces;

            /* then we combine the two situations in which a white pawn can legally 
                attack/move. */
            validMove = validMove | validAttacks;

            return validMove;
        }
        public ulong getBishopAvailableMove(int location, ulong allAllyPieces, ulong allPieces)
        {
            ulong diagnalMoves, antiDiagonalMoves, moves, validMoves;
            diagnalMoves = getDiagonalSlidingMoves(location, allPieces);
            antiDiagonalMoves = getAntiDiagonalSlidingMoves(location, allPieces);
            moves = diagnalMoves | antiDiagonalMoves;
            validMoves = moves & ~allAllyPieces;
            return validMoves;
        }
        public ulong getRookAvailableMove(int location, ulong allAllyPieces, ulong allPieces)
        {
            ulong rankMoves, fileMoves, moves, validMoves;
            rankMoves = getRankSlidingMoves(location, allPieces);
            fileMoves = getFileSlidingMoves(location, allPieces);
            moves = rankMoves | fileMoves;
            validMoves = moves & ~allPieces;
            return validMoves;
        }
        public ulong getQueenAvailableMoves(int location, ulong allAllyPieces, ulong allPieces)
        {
            return getBishopAvailableMove(location, allAllyPieces, allPieces) | getRookAvailableMove(location, allAllyPieces, allPieces);
        }
        public ulong getRankSlidingMoves(int location, ulong allPieces)
        {
            ulong locationBit = singleBit(location);
            return (allPieces - 2 * locationBit) ^ (Reverse(allPieces) - 2 * Reverse(locationBit));
        }
        public ulong getFileSlidingMoves(int location, ulong allPieces)
        {
            ulong forward, reverse, slider, lineMask;

            lineMask = fileMaskEx(location); // excludes square of slider
            slider = singleBit(location); // single bit 1 << sq, 2^sq

            forward = allPieces & lineMask; // also performs the first subtraction by clearing the s in o
            reverse = SwapBytes(forward); // o'-s'
            forward -= (slider); // o -2s
            reverse -= SwapBytes(slider); // o'-2s'
            forward ^= SwapBytes(reverse);
            return forward & lineMask;      // mask the line again
        }
        public ulong getDiagonalSlidingMoves(int location, ulong allPieces)
        {
            ulong forward, reverse, slider, lineMask;

            lineMask = diagonalMaskEx(location); // excludes square of slider
            slider = singleBit(location); // single bit 1 << sq, 2^sq

            forward = allPieces & lineMask; // also performs the first subtraction by clearing the s in o
            reverse = SwapBytes(forward); // o'-s'
            forward -= (slider); // o -2s
            reverse -= SwapBytes(slider); // o'-2s'
            forward ^= SwapBytes(reverse);
            return forward & lineMask;      // mask the line again
        }
        public ulong getAntiDiagonalSlidingMoves(int location, ulong allPieces)
        {
            ulong forward, reverse, slider, lineMask;

            lineMask = antiDiagMaskEx(location); // excludes square of slider
            slider = singleBit(location); // single bit 1 << sq, 2^sq

            forward = allPieces & lineMask; // also performs the first subtraction by clearing the s in o
            reverse = SwapBytes(forward); // o'-s'
            forward -= (slider); // o -2s
            reverse -= SwapBytes(slider); // o'-2s'
            forward ^= SwapBytes(reverse);
            return forward & lineMask;      // mask the line again
        }
        static ulong Reverse(ulong x)
        {
            ulong y = 0;
            for (int i = 0; i < 64; ++i)
            {
                y <<= 1;
                y |= (x & (1));
                x >>= 1;
            }
            return y;

        }
        public void makeMove(Move move)
        {
            ulong empty=0;
            ulong fromBB = (ulong)(1) << move.source;
            ulong toBB = (ulong)(1) << move.dest;
            ulong fromToBB = fromBB ^ toBB; // |+
            pieces[move.piece] ^= fromToBB;   // update piece bitboard
            pieces[move.color] ^= fromToBB;   // update white or black color bitboard
            pieces[move.capturedPiece] ^= toBB;       // reset the captured piece
            pieces[move.capturedPiece] ^= toBB;       // update color bitboard by captured piece
            occupied ^= fromBB;     // update occupied, only from becomes empty
            empty ^= fromBB;     // update empty bitboard
        }
    }
}
