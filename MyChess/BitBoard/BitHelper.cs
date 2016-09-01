using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyChess
{
    class BitHelper
    {
        static int[] table = new int[]{0, 9, 1, 10, 13, 21, 2, 29, 11, 14, 16, 18, 22, 25, 3, 30,
                8, 12, 20, 28, 15, 17, 24, 7, 19, 27, 23, 6, 26, 5, 4, 31};
        public static int getNumberOfTrailingZeros(ulong x)
        {
            const ulong debruijn64 = 0x03f79d71b4cb0a89;
            if (x == 0) return 64;
            return index64[((x ^ (x - 1)) * debruijn64) >> 58];
        }
        public static int getNumberOfLeadingZeros(ulong x)
        {
            UInt32 First32Bit, Last32Bit;
            First32Bit = (UInt32)(x & 0xffffffff);
            Last32Bit = (UInt32)((x >> 32) & 0xffffffff);
            if (Last32Bit == 0)
                return getNumberOfLeadingZerosInt32(Last32Bit) + getNumberOfLeadingZerosInt32(First32Bit);
            return getNumberOfLeadingZerosInt32(Last32Bit);
        }
        public static int getNumberOfTrailingZerosInt32(UInt32 x)
        {
            return MultiplyDeBruijnBitPosition[((UInt32)((x & -x) * 0x077CB531U)) >> 27];
        }
        static int[] clz_table_4bit = new int[16] { 4, 3, 2, 2, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 };
        static int[] MultiplyDeBruijnBitPosition = new int[32] 
{
  0, 1, 28, 2, 29, 14, 24, 3, 30, 22, 20, 15, 25, 17, 4, 8, 
  31, 27, 13, 23, 21, 19, 16, 7, 26, 12, 18, 6, 11, 5, 10, 9
};
        public static int getNumberOfLeadingZerosInt32(UInt32 x)
        {
            int n;
            if ((x & 0xFFFF0000) == 0) { n = 16; x <<= 16; } else { n = 0; }
            if ((x & 0xFF000000) == 0) { n += 8; x <<= 8; }
            if ((x & 0xF0000000) == 0) { n += 4; x <<= 4; }
            n += (int)clz_table_4bit[x >> (28)];
            return n;

        }
        public static ulong reverse(ulong i)
        {
            i = (i & 0x5555555555555555L) << 1 | (i >> 1) & 0x5555555555555555L;
            i = (i & 0x3333333333333333L) << 2 | (i >> 2) & 0x3333333333333333L;
            i = (i & 0x0f0f0f0f0f0f0f0fL) << 4 | (i >> 4) & 0x0f0f0f0f0f0f0f0fL;
            i = (i & 0x00ff00ff00ff00ffL) << 8 | (i >> 8) & 0x00ff00ff00ff00ffL;
            i = (i << 48) | ((i & 0xffff0000L) << 16) |
                ((i >> 16) & 0xffff0000L) | (i >> 48);
            return i;
        }
        static byte[] BitReverseTable256 = new byte[256]
{
  0x00, 0x80, 0x40, 0xC0, 0x20, 0xA0, 0x60, 0xE0, 0x10, 0x90, 0x50, 0xD0, 0x30, 0xB0, 0x70, 0xF0, 
  0x08, 0x88, 0x48, 0xC8, 0x28, 0xA8, 0x68, 0xE8, 0x18, 0x98, 0x58, 0xD8, 0x38, 0xB8, 0x78, 0xF8, 
  0x04, 0x84, 0x44, 0xC4, 0x24, 0xA4, 0x64, 0xE4, 0x14, 0x94, 0x54, 0xD4, 0x34, 0xB4, 0x74, 0xF4, 
  0x0C, 0x8C, 0x4C, 0xCC, 0x2C, 0xAC, 0x6C, 0xEC, 0x1C, 0x9C, 0x5C, 0xDC, 0x3C, 0xBC, 0x7C, 0xFC, 
  0x02, 0x82, 0x42, 0xC2, 0x22, 0xA2, 0x62, 0xE2, 0x12, 0x92, 0x52, 0xD2, 0x32, 0xB2, 0x72, 0xF2, 
  0x0A, 0x8A, 0x4A, 0xCA, 0x2A, 0xAA, 0x6A, 0xEA, 0x1A, 0x9A, 0x5A, 0xDA, 0x3A, 0xBA, 0x7A, 0xFA,
  0x06, 0x86, 0x46, 0xC6, 0x26, 0xA6, 0x66, 0xE6, 0x16, 0x96, 0x56, 0xD6, 0x36, 0xB6, 0x76, 0xF6, 
  0x0E, 0x8E, 0x4E, 0xCE, 0x2E, 0xAE, 0x6E, 0xEE, 0x1E, 0x9E, 0x5E, 0xDE, 0x3E, 0xBE, 0x7E, 0xFE,
  0x01, 0x81, 0x41, 0xC1, 0x21, 0xA1, 0x61, 0xE1, 0x11, 0x91, 0x51, 0xD1, 0x31, 0xB1, 0x71, 0xF1,
  0x09, 0x89, 0x49, 0xC9, 0x29, 0xA9, 0x69, 0xE9, 0x19, 0x99, 0x59, 0xD9, 0x39, 0xB9, 0x79, 0xF9, 
  0x05, 0x85, 0x45, 0xC5, 0x25, 0xA5, 0x65, 0xE5, 0x15, 0x95, 0x55, 0xD5, 0x35, 0xB5, 0x75, 0xF5,
  0x0D, 0x8D, 0x4D, 0xCD, 0x2D, 0xAD, 0x6D, 0xED, 0x1D, 0x9D, 0x5D, 0xDD, 0x3D, 0xBD, 0x7D, 0xFD,
  0x03, 0x83, 0x43, 0xC3, 0x23, 0xA3, 0x63, 0xE3, 0x13, 0x93, 0x53, 0xD3, 0x33, 0xB3, 0x73, 0xF3, 
  0x0B, 0x8B, 0x4B, 0xCB, 0x2B, 0xAB, 0x6B, 0xEB, 0x1B, 0x9B, 0x5B, 0xDB, 0x3B, 0xBB, 0x7B, 0xFB,
  0x07, 0x87, 0x47, 0xC7, 0x27, 0xA7, 0x67, 0xE7, 0x17, 0x97, 0x57, 0xD7, 0x37, 0xB7, 0x77, 0xF7, 
  0x0F, 0x8F, 0x4F, 0xCF, 0x2F, 0xAF, 0x6F, 0xEF, 0x1F, 0x9F, 0x5F, 0xDF, 0x3F, 0xBF, 0x7F, 0xFF
};

        UInt32 v; // reverse 32-bit value, 8 bits at time
        UInt32 c; // c will get v reversed
        public static UInt32 resever32(UInt32 v)
        {
            return (UInt32)((BitReverseTable256[v & 0xff] << 24) |
            (BitReverseTable256[(v >> 8) & 0xff] << 16) |
            (BitReverseTable256[(v >> 16) & 0xff] << 8) |
            (BitReverseTable256[(v >> 24) & 0xff]));
        }
        static int[] index64 = new int[64]{
    0, 47,  1, 56, 48, 27,  2, 60,
   57, 49, 41, 37, 28, 16,  3, 61,
   54, 58, 35, 52, 50, 42, 21, 44,
   38, 32, 29, 23, 17, 11,  4, 62,
   46, 55, 26, 59, 40, 36, 15, 53,
   34, 51, 20, 43, 31, 22, 10, 45,
   25, 39, 14, 33, 19, 30,  9, 24,
   13, 18,  8, 12,  7,  6,  5, 63
};
        public static ulong pseudoRotate45clockwise(ulong x)
        {
            const ulong k1 = (ulong)(0xAAAAAAAAAAAAAAAA);
            const ulong k2 = (ulong)(0xCCCCCCCCCCCCCCCC);
            const ulong k4 = (ulong)(0xF0F0F0F0F0F0F0F0);
            x ^= k1 & (x ^ rotateRight(x, 8));
            x ^= k2 & (x ^ rotateRight(x, 16));
            x ^= k4 & (x ^ rotateRight(x, 32));
            return x;
        }
        public static ulong pseudoRotate45antiClockwise(ulong x)
        {
            const ulong k1 = (ulong)(0x5555555555555555);
            const ulong k2 = (ulong)(0x3333333333333333);
            const ulong k4 = (ulong)(0x0f0f0f0f0f0f0f0f);
            x ^= k1 & (x ^ rotateRight(x, 8));
            x ^= k2 & (x ^ rotateRight(x, 16));
            x ^= k4 & (x ^ rotateRight(x, 32));
            return x;
        }
        public static ulong rotate90antiClockwise(ulong x)
        {
            return flipDiagA1H8(flipVertical(x));
        }
        public static ulong rotate90clockwise(ulong x)
        {
            return flipVertical(flipDiagA1H8(x));
        }
        public static ulong rotate180(ulong x)
        {
            const ulong h1 = (ulong)(0x5555555555555555);
            const ulong h2 = (ulong)(0x3333333333333333);
            const ulong h4 = (ulong)(0x0F0F0F0F0F0F0F0F);
            const ulong v1 = (ulong)(0x00FF00FF00FF00FF);
            const ulong v2 = (ulong)(0x0000FFFF0000FFFF);
            x = ((x >> 1) & h1) | ((x & h1) << 1);
            x = ((x >> 2) & h2) | ((x & h2) << 2);
            x = ((x >> 4) & h4) | ((x & h4) << 4);
            x = ((x >> 8) & v1) | ((x & v1) << 8);
            x = ((x >> 16) & v2) | ((x & v2) << 16);
            x = (x >> 32) | (x << 32);
            return x;

        }
        public static ulong flipDiagA8H1(ulong x)
        {
            ulong t;
            const ulong k1 = (ulong)(0xaa00aa00aa00aa00);
            const ulong k2 = (ulong)(0xcccc0000cccc0000);
            const ulong k4 = (ulong)(0xf0f0f0f00f0f0f0f);
            t = x ^ (x << 36);
            x ^= k4 & (t ^ (x >> 36));
            t = k2 & (x ^ (x << 18));
            x ^= t ^ (t >> 18);
            t = k1 & (x ^ (x << 9));
            x ^= t ^ (t >> 9);
            return x;
        }
        public static ulong flipDiagA1H8(ulong x)
        {
            ulong t;
            const ulong k1 = (ulong)(0x5500550055005500);
            const ulong k2 = (ulong)(0x3333000033330000);
            const ulong k4 = (ulong)(0x0f0f0f0f00000000);
            t = k4 & (x ^ (x << 28));
            x ^= t ^ (t >> 28);
            t = k2 & (x ^ (x << 14));
            x ^= t ^ (t >> 14);
            t = k1 & (x ^ (x << 7));
            x ^= t ^ (t >> 7);
            return x;
        }
        public static ulong flipVertical(ulong x)
        {
            const ulong k1 = 0x00FF00FF00FF00FF;
            const ulong k2 = 0x0000FFFF0000FFFF;
            x = ((x >> 8) & k1) | ((x & k1) << 8);
            x = ((x >> 16) & k2) | ((x & k2) << 16);
            x = (x >> 32) | (x << 32);
            return x;

        }
        public static ulong rotateLeft(ulong x, int s) { return (x << s) | (x >> (64 - s)); }
        public static ulong rotateRight(ulong x, int s) { return (x >> s) | (x << (64 - s)); }
        public static int NumberOfSetBits(ulong i)
        {
            i = i - ((i >> 1) & 0x5555555555555555UL);
            i = (i & 0x3333333333333333UL) + ((i >> 2) & 0x3333333333333333UL);
            return (int)(unchecked(((i + (i >> 4)) & 0xF0F0F0F0F0F0F0FUL) * 0x101010101010101UL) >> 56);
        }
        public static int[] getSetBits(ulong bitboard)
        {
            List<int> indexes = new List<int>(64);
            while (bitboard != 0)
            {
                indexes.Add(getNumberOfTrailingZeros(bitboard));
                bitboard &= bitboard - 1;
            }
            indexes.Add(-1);
            return indexes.ToArray();
        }
    }
}
