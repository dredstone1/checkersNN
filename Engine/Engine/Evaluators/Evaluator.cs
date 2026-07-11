namespace Engine.Evaluators;

public struct EvaluationResult
{
    public int WhiteScore;
    public int BlackScore;
}

public static class Evaluator
{
    public static int Evaluate(BitBoard board)
    {
        // EvaluationResult centerResult = CenterEvaluator.Evaluate(board);
        EvaluationResult backrankResult = BackrankControlEvaluator.Evaluate(board);

        int whiteScore = backrankResult.WhiteScore
            + board.WhiteMaterial;
        int blackScore =  backrankResult.BlackScore
            + board.BlackMaterial;

        return board.IsWhiteToMove
            ? whiteScore - blackScore
            : blackScore - whiteScore;
    }

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
