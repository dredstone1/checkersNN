using Engine.Models;

namespace Engine;

public static class Constants
{
#if BOARD_10
    public const int BoardSize = 10;
#elif BOARD_8
    public const int BoardSize = 8;
#endif

    public const int AlphaStart = -10_000_000;
    public const int BetaStart = 10_000_000;

    public const int MaxDepth = 32;
    public const int MaxMoves = 128;

    public const int NullMoveReduction = 2;
    public static readonly int DeltaPruningMargin = MaterialValue.GetPieceValue(PieceType.Man);
    public static readonly int[,] LmrTable = CreateLMR();

    static int[,] CreateLMR()
    {
        int[,] lmr = new int[MaxDepth, MaxMoves];
        for (int depth = 1; depth < MaxDepth; depth++)
        {
            for (int move = 1; move < MaxMoves; move++)
            {
                int reduction = (int)Math.Round(0.99 + Math.Log(depth) * Math.Log(move) / 3.14);
                if (depth > 2 && depth - reduction < 2)
                {
                    reduction = depth - 2;
                }

                lmr[depth, move] = reduction;
            }
        }
        return lmr;
    }
}
