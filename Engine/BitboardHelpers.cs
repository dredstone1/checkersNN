using System.Numerics;
using System.Runtime.CompilerServices;

namespace Engine;

public static class BitboardHelpers
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte BitScanForward(ref BoardBits bitboard)
    {
        if (bitboard == 0)
        {
            throw new InvalidOperationException("Cannot scan forward on an empty bitboard");
        }

        ulong low = (ulong)bitboard;
        int index;

        if (low != 0)
        {
            index = BitOperations.TrailingZeroCount(low);
        }
        else
        {
            ulong high = (ulong)(bitboard >> 64);
            index = 64 + BitOperations.TrailingZeroCount(high);
        }

        bitboard &= bitboard - 1;
        return (byte)index;
    }
}
