using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
namespace MyChess
{
    class Zobrist
    {
        public static RandomNumberGenerator random = new RNGCryptoServiceProvider();
        static ulong[, ,] zArray = new ulong[2, 6, 64];
    static ulong[] zEnPassant = new ulong[8];
    static ulong[] zCastle = new ulong[4];
    static ulong zWhite;
    public static ulong random64() {
        byte[] number=new byte[8];
        random.GetBytes(number);
        //ulong a= BitConverter.ToUInt64(number,0);
        //for (int i = 0; i < 2; i++)
        //{
        //    for (int j = 0; j < 6; j++)
        //        for (int k = 0; k < 64; k++)
        //            if (a == zArray[i, j, k])
        //                Console.WriteLine();
        //}
        return BitConverter.ToUInt64(number, 0);
    }
    public static void zobristFillArray() {
        for (int color = 0; color < 2; color++)
        {
            for (int pieceType = 0; pieceType < 6; pieceType++)
            {
                for (int square = 0; square < 64; square++)
                {
                    zArray[color,pieceType,square] = random64();
                }
            }
        }
        for (int column = 0; column < 8; column++)
        {
            zEnPassant[column] = random64();
        }
        for (int i = 0; i < 4; i++)
        {
            zCastle[i] = random64();
        }
        zWhite = random64();
    }
    public static ulong getZobristHash(ulong WP,ulong WN,ulong WB,ulong WR,ulong WQ,ulong WK,ulong BP,ulong BN,ulong BB,ulong BR,ulong BQ,ulong BK,ulong EP,bool CWK,bool CWQ,bool CBK,bool CBQ,bool white) {
        ulong returnZKey = 0;
        for (int square = 0; square < 64; square++)
        {
            if (((WP >> square) & 1) == 1)
            {
                returnZKey ^= zArray[0,0,square];
            }
            else if (((BP >> square) & 1) == 1)
            {
                returnZKey ^= zArray[1,0,square];
            }
            else if (((WN >> square) & 1) == 1)
            {
                returnZKey ^= zArray[0,1,square];
            }
            else if (((BN >> square) & 1) == 1)
            {
                returnZKey ^= zArray[1,1,square];
            }
            else if (((WB >> square) & 1) == 1)
            {
                returnZKey ^= zArray[0,2,square];
            }
            
            else if (((BB >> square) & 1) == 1)
            {
                returnZKey ^= zArray[1,2,square];
            }
            else if (((WR >> square) & 1) == 1)
            {
                returnZKey ^= zArray[0,3,square];
            }
            else if (((BR >> square) & 1) == 1)
            {
                returnZKey ^= zArray[1,3,square];
            }
            else if (((WQ >> square) & 1) == 1)
            {
                returnZKey ^= zArray[0,4,square];
            }
            else if (((BQ >> square) & 1) == 1)
            {
                returnZKey ^= zArray[1,4,square];
            }
            else if (((WK >> square) & 1) == 1)
            {
                returnZKey ^= zArray[0,5,square];
            }
            else if (((BK >> square) & 1) == 1)
            {
                returnZKey ^= zArray[1,5,square];
            }
        }
        for (int column = 0; column < 8; column++)
        {
            if (EP == Moves.FileMasks8[column])
            {
                returnZKey ^= zEnPassant[column];
            }
        }
        if (CWK)
            returnZKey ^= zCastle[0];
        if (CWQ)
            returnZKey ^= zCastle[1];
        if (CBK)
            returnZKey ^= zCastle[2];
        if (CBQ)
            returnZKey ^= zCastle[3];
        if (white)
            returnZKey ^= zWhite;
        return returnZKey;
    }
    public static void testDistribution() {
        int sampleSize = 2000;
        int sampleSeconds = 10;
        DateTime startTime = DateTime.Now;
        double endTime = sampleSeconds * 1000;
        int[] distArray;
        distArray = new int[sampleSize];
        while ((DateTime.Now-startTime).TotalMilliseconds < endTime)
        {
            for (int i = 0; i < 10000; i++)
            {
                distArray[(int)(random64()% (ulong) (sampleSize / 2)) + (sampleSize / 2)]++;
            }
        }
        for (int i = 0; i < sampleSize; i++)
        {
            Console.WriteLine(distArray[i]);
        }
    }
    }
}
