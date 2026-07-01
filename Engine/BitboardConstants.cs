namespace Engine;

public static class BitboardConstants
{
    public static readonly BoardBits LeftEdgeMask = MakeFileMask(0);
    public static readonly BoardBits RightEdgeMask = MakeFileMask(Constants.BoardSize - 1);
    public static readonly BoardBits BottomEdgeMask = MakeRankMask(0);
    public static readonly BoardBits TopEdgeMask = MakeRankMask(Constants.BoardSize - 1);

    public static readonly BoardBits TopRightEdgeMask = TopEdgeMask | RightEdgeMask;
    public static readonly BoardBits TopLeftEdgeMask = TopEdgeMask | LeftEdgeMask;
    public static readonly BoardBits BottomRightEdgeMask = BottomEdgeMask | RightEdgeMask;
    public static readonly BoardBits BottomLeftEdgeMask = BottomEdgeMask | LeftEdgeMask;

    public static readonly BoardBits EdgeMasks =
        LeftEdgeMask | RightEdgeMask | BottomEdgeMask | TopEdgeMask;

    public static readonly BoardBits NotLeftEdgeMask = ~LeftEdgeMask;
    public static readonly BoardBits NotRightEdgeMask = ~RightEdgeMask;
    public static readonly BoardBits NotBottomEdgeMask = ~BottomEdgeMask;
    public static readonly BoardBits NotTopEdgeMask = ~TopEdgeMask;

    public static readonly BoardBits NotTopRightEdgeMask = ~TopRightEdgeMask;
    public static readonly BoardBits NotTopLeftEdgeMask = ~TopLeftEdgeMask;
    public static readonly BoardBits NotBottomRightEdgeMask = ~BottomRightEdgeMask;
    public static readonly BoardBits NotBottomLeftEdgeMask = ~BottomLeftEdgeMask;

    private static BoardBits MakeFileMask(int file)
    {
        BoardBits mask = 0;
        for (int rank = 0; rank < Constants.BoardSize; rank++)
        {
            mask |= BitboardHelpers.One << (rank * Constants.BoardSize + file);
        }
        return mask;
    }

    private static BoardBits MakeRankMask(int rank)
    {
        BoardBits mask = 0;
        for (int file = 0; file < Constants.BoardSize; file++)
        {
            mask |= BitboardHelpers.One << (rank * Constants.BoardSize + file);
        }
        return mask;
    }
}
