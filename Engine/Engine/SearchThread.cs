using Engine.Evaluators;
using Engine.Models;

namespace Engine;

internal sealed class SearchThread(TranspositionTable transpositionTable, int maxDepth)
{
    private readonly int _maxDepth = maxDepth;

    private readonly Move[,] _killerMoves = new Move[maxDepth + 1, 2];
    private readonly int[,] _historyHeuristic = new int[
        Constants.SquareCount,
        Constants.SquareCount
    ];
    private readonly TranspositionTable _transpositionTable = transpositionTable;

    public int Negamax(
        BitBoard board,
        int depth,
        int alpha,
        int beta,
        bool isLastMoveCapture = false
    )
    {
        if (
            _transpositionTable.TryProbe(
                board.ZobristKey,
                depth,
                alpha,
                beta,
                out int ttScore,
                out int ttMove
            )
        )
        {
            return ttScore;
        }

        if (Evaluator.TryEvaluateTermination(board, depth, out int terminationEval))
        {
            return terminationEval;
        }

        if (depth <= 0)
        {
            return isLastMoveCapture
                ? Quiescence(board, alpha, beta, depth)
                : Evaluator.Evaluate(board);
        }

        Span<Move> moves = stackalloc Move[Constants.MaxMoves];
        int moveCount = 0;
        MoveGenerator.Generate(board, moves, ref moveCount);
        if (moveCount == 0)
        {
            return 0;
        }

        if (depth > Constants.NullMoveReduction + 1)
        {
            NullMoveUndoState undo = board.MakeNullMove();
            int score = -Negamax(
                board,
                depth - 1 - Constants.NullMoveReduction,
                alpha: -beta,
                beta: -beta + 1
            );
            board.UndoNullMove(undo);

            if (score >= beta)
            {
                return beta;
            }
        }

        Span<int> scores = stackalloc int[moveCount];
        MoveOrdering.ScoreMoves(
            board,
            depth,
            _killerMoves,
            _historyHeuristic,
            packedTtMove: ttMove,
            scores,
            moves,
            moveCount
        );

        int originalAlpha = alpha;
        int bestMovePacked = 0;
        for (int i = 0; i < moveCount; i++)
        {
            Move move = MoveOrdering.GetNextHighestMove(i, moves, scores, moveCount);
            bool isCapture = move.CapturesMask != 0;
            bool isQuiet = !isCapture && !move.IsPromotion;

            MoveUndoState undo = board.MakeMove(move);

            int searchDepth = depth - 1;
            bool reduce = i > 0 && depth >= 3 && isQuiet;
            if (reduce)
            {
                searchDepth -= Constants.LmrTable[depth, i];
            }

            int score = -Negamax(
                board,
                searchDepth,
                alpha: -beta,
                beta: -alpha,
                isLastMoveCapture: isCapture
            );

            if (reduce && score > alpha)
            {
                score = -Negamax(board, depth - 1, -beta, -alpha, isLastMoveCapture: isCapture);
            }

            board.UndoMove(undo);

            if (score > alpha)
            {
                alpha = score;
                bestMovePacked = move.Pack();
            }
            if (alpha >= beta)
            {
                _killerMoves[depth, 1] = _killerMoves[depth, 0];
                _killerMoves[depth, 0] = move;
                if (move.CapturesMask == 0 && !move.IsPromotion)
                {
                    _historyHeuristic[move.From, move.To] += depth * depth;
                }
                break;
            }
        }

        NodeType type;

        if (alpha <= originalAlpha)
        {
            type = NodeType.UpperBound;
        }
        else if (alpha >= beta)
        {
            type = NodeType.LowerBound;
        }
        else
        {
            type = NodeType.Exact;
        }

        _transpositionTable.Store(board.ZobristKey, depth, alpha, type, bestMovePacked);

        return alpha;
    }

    private int Quiescence(BitBoard board, int alpha, int beta, int depth)
    {
        if (
            _transpositionTable.TryProbe(
                board.ZobristKey,
                depth,
                alpha,
                beta,
                out int ttScore,
                out int ttMove
            )
        )
        {
            return ttScore;
        }

        if (Evaluator.TryEvaluateTermination(board, depth, out int terminationEval))
        {
            return terminationEval;
        }

        int standPat = Evaluator.Evaluate(board);

        if (standPat >= beta)
        {
            return beta;
        }
        if (standPat > alpha)
        {
            alpha = standPat;
        }

        Span<Move> moves = stackalloc Move[Constants.MaxMoves];
        int moveCount = 0;
        MoveGenerator.Generate(board, moves, ref moveCount);
        if (moveCount == 0)
        {
            return 0;
        }

        // because captures are forced we know if any move is a capture all moves are captures
        if (moves[0].CapturesMask == 0)
        {
            return standPat;
        }

        Span<int> scores = stackalloc int[moveCount];
        MoveOrdering.ScoreMoves(
            board,
            depth,
            _killerMoves,
            _historyHeuristic,
            packedTtMove: ttMove,
            scores,
            moves,
            moveCount
        );

        for (int i = 0; i < moveCount; i++)
        {
            Move move = MoveOrdering.GetNextHighestMove(i, moves, scores, moveCount);

            MoveUndoState undo = board.MakeMove(move);
            int score = -Quiescence(board, -beta, -alpha, depth);
            board.UndoMove(undo);

            if (score >= beta)
            {
                return beta;
            }
            if (score > alpha)
            {
                alpha = score;
            }
        }

        return alpha;
    }
}
