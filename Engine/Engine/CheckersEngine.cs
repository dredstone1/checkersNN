using Engine.Models;

namespace Engine;

public static class CheckersEngine
{
    private static readonly TranspositionTable _transpositionTable = new();

    public static (Move? BestMove, int EvalForBot) FindBestMove(BitBoard board, int depth)
    {
        Move? bestMove = null;
        int evalForBot = 0;

        for (int currentDepth = 1; currentDepth <= depth; currentDepth++)
        {
            (bestMove, evalForBot) = SearchRoot(board, currentDepth);
        }

        return (bestMove, evalForBot);
    }

    private static (Move? BestMove, int EvalForBot) SearchRoot(BitBoard board, int depth)
    {
        Move[] moves = new Move[Constants.MaxMoves];
        int moveCount = 0;

        MoveGenerator.Generate(board, moves, ref moveCount);
        if (moveCount == 0)
        {
            return (BestMove: null, EvalForBot: 0);
        }

        MoveOrdering.SortMoves(
            board,
            depth,
            new Move[depth + 1, 2],
            new int[Constants.SquareCount, Constants.SquareCount],
            packedTtMove: 0,
            moves,
            moveCount
        );

        BitBoard olderBoardCopy = new(board);
        Move olderMove = moves[0];
        olderBoardCopy.MakeMove(olderMove);

        int alpha = -new SearchThread(_transpositionTable, depth).Negamax(
            olderBoardCopy,
            depth - 1,
            alpha: Constants.AlphaStart,
            beta: Constants.BetaStart
        );

        int[] scores = new int[moveCount];
        scores[0] = alpha;

        Parallel.For(
            1,
            moveCount,
            new() { MaxDegreeOfParallelism = 4 },
            i =>
            {
                Move move = moves[i];
                BitBoard boardCopy = new(board);
                boardCopy.MakeMove(move);

                SearchThread search = new(_transpositionTable, depth);

                int localAlpha = alpha;
                int score = -search.Negamax(
                    boardCopy,
                    depth - 1,
                    alpha: -localAlpha - 1,
                    beta: -localAlpha
                );

                if (score > localAlpha)
                {
                    score = -search.Negamax(
                        boardCopy,
                        depth - 1,
                        alpha: Constants.AlphaStart,
                        beta: Constants.BetaStart
                    );
                    scores[i] = score;
                }
                else
                {
                    scores[i] = int.MinValue;
                }

                int oldAlpha;
                do
                {
                    oldAlpha = alpha;
                    if (score <= oldAlpha)
                    {
                        break;
                    }
                } while (Interlocked.CompareExchange(ref alpha, score, oldAlpha) != oldAlpha);
            }
        );

        Move bestMove = moves[0];
        int bestAlpha = scores[0];
        for (int i = 1; i < moveCount; i++)
        {
            if (scores[i] > bestAlpha)
            {
                bestAlpha = scores[i];
                bestMove = moves[i];
            }
        }

        return (BestMove: bestMove, EvalForBot: bestAlpha);
    }
}
