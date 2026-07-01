using AwesomeAssertions;
using Engine;
using Engine.Models;

namespace EngineTests;

public abstract class PieceDefinitionTestBase
{
    protected void TestMoves(PieceTestCase testCase)
    {
        Dictionary<int, Piece> pieces = new(testCase.BlockedBy)
        {
            [testCase.Origin] = testCase.Piece,
        };
        BitBoard board = new(pieces, isWhiteToMove: testCase.IsWhiteToMove);

        Span<Move> moves = stackalloc Move[256];
        int moveCount = 0;
        MoveGenerator.GenerateForPiece(
            board,
            testCase.Origin,
            testCase.Piece,
            moves,
            ref moveCount
        );

        List<Move> expectedMoves = [.. testCase.ExpectedMoves];
        List<Move> result = [.. moves[..moveCount]];

        var expectedMoveSorted = expectedMoves.OrderBy(m => m.To).ToList();
        var resultSorted = result.OrderBy(m => m.To).ToList();
        resultSorted.Should().BeEquivalentTo(expectedMoveSorted);
    }
}
