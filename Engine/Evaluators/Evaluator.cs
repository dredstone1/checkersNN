namespace Engine.Evaluators;

public static class Evaluator
{
    public static int Evaluate(BitBoard board) => 0;

    public static bool TryEvaluateTermination(BitBoard board, int depth, out int terminationEval)
    {
        if (board.WhitePieces == 0)
        {
            terminationEval = board.IsWhiteToMove ? -100_000 - depth : 100_000 + depth;
            return true;
        }
        else if (board.BlackPieces == 0)
        {
            terminationEval = board.IsWhiteToMove ? 100_000 + depth : -100_000 - depth;
            return true;
        }

        terminationEval = 0;
        return false;
    }
}
