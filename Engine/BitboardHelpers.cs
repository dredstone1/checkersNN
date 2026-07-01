using System.Numerics;
using System.Runtime.CompilerServices;

namespace Engine;

public static class BitboardHelpers
{
#if BOARD_10
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

    public static BoardBits One => UInt128.One;
#elif BOARD_8
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte BitScanForward(ref BoardBits bitboard)
    {
        if (bitboard == 0)
        {
            throw new InvalidOperationException("Cannot scan forward on an empty bitboard");
        }

        int index = BitOperations.TrailingZeroCount(bitboard);
        bitboard &= bitboard - 1;
        return (byte)index;
    }

    public static BoardBits One => 1ul;
#else
#error Unsupported board size
#endif

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoardBits ShiftRight(BoardBits mask) =>
        (mask & BitboardConstants.NotRightEdgeMask) << 1;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoardBits ShiftLeft(BoardBits mask) =>
        (mask & BitboardConstants.NotLeftEdgeMask) >> 1;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    // we need to mask out the top edge because we might have left out bits at the top
    public static BoardBits ShiftUp(BoardBits mask) =>
        (mask & BitboardConstants.NotTopEdgeMask) << Constants.BoardSize;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoardBits ShiftUpRight(BoardBits mask) =>
        (mask & BitboardConstants.NotTopRightEdgeMask) << Constants.BoardSize + 1;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoardBits ShiftUpLeft(BoardBits mask) =>
        (mask & BitboardConstants.NotTopLeftEdgeMask) << Constants.BoardSize - 1;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoardBits ShiftDown(BoardBits mask) => mask >> Constants.BoardSize;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoardBits ShiftDownRight(BoardBits mask) =>
        (mask & BitboardConstants.NotBottomRightEdgeMask) >> Constants.BoardSize - 1;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoardBits ShiftDownLeft(BoardBits mask) =>
        (mask & BitboardConstants.NotBottomLeftEdgeMask) >> Constants.BoardSize + 1;
}
