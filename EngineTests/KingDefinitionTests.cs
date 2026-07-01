using Engine.Models;

namespace EngineTests;

public class KingDefinitionTests : PieceDefinitionTestBase
{
    [Theory]
    [ClassData(typeof(KingTestData))]
    public void KingDefinition_evaluates_expected_positions(PieceTestCase testCase) =>
        TestMoves(testCase);
}

public class KingTestData : TheoryData<PieceTestCase>
{
    public KingTestData()
    {
        Piece king = new() { Type = PieceType.King, Color = PieceColor.White };
        Add(
            PieceTestCase
                .From("e5", king)
                .WithFriendlyPieceAt("f6")
                .WithEnemyPieceAt("c3")
                .WithFriendlyPieceAt("g3")
                .GoesTo("d6", "c7", "b8", "f4", "d4")
                .GoesTo("b2", captures: ["c3"])
        );
    }
}
