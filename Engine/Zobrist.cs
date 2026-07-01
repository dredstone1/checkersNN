using Engine.Models;

namespace Engine;

public static class Zobrist
{
    public static ulong[,,] PieceSquare { get; }
    public static ulong SideToMove { get; }

    private static readonly int PieceTypes = Enum.GetValues<PieceType>().Length;
    private static readonly int Colors = Enum.GetValues<PieceColor>().Length;

    static Zobrist()
    {
        Random rng = new(6767);

        PieceSquare = new ulong[PieceTypes, Colors, 100];
        for (int piece = 0; piece < PieceTypes; piece++)
        {
            for (int color = 0; color < Colors; color++)
            {
                for (int square = 0; square < 100; square++)
                {
                    PieceSquare[piece, color, square] = NextULong(rng);
                }
            }
        }

        SideToMove = NextULong(rng);
    }

    private static ulong NextULong(Random rng)
    {
        byte[] bytes = new byte[8];
        rng.NextBytes(bytes);
        return BitConverter.ToUInt64(bytes, 0);
    }

    public static ulong Compute(BitBoard board)
    {
        ulong hash = 0;

        for (byte square = 0; square < 100; square++)
        {
            if (board.TryGetPieceAt(square, out var piece))
            {
                hash ^= PieceSquare[(int)piece.Value.Type, (int)piece.Value.Color, square];
            }
        }

        if (!board.IsWhiteToMove)
        {
            hash ^= SideToMove;
        }

        return hash;
    }
}
