using Engine.Models;

namespace EngineTests;

public class ManDefinitionTests : PieceDefinitionTestBase
{
    [Theory]
    [ClassData(typeof(ManTestData))]
    public void ManDefinition_evaluates_expected_positions(PieceTestCase testCase) =>
        TestMoves(testCase);
}

public class ManTestData : TheoryData<PieceTestCase>
{
    public ManTestData()
    {
        Piece whiteMan = new() { Type = PieceType.Man, Color = PieceColor.White };
        Piece blackMan = new() { Type = PieceType.Man, Color = PieceColor.Black };

        Add(
            PieceTestCase
                .From("e3", whiteMan)
                .GoesTo("d4", "f4")
                .WithDescription("White goes forwards")
        );

        Add(
            PieceTestCase
                .From("e3", whiteMan)
                .WithFriendlyPieceAt("d4")
                .WithEnemyPieceAt("f4")
                .GoesTo("g5", captures: ["f4"])
                .WithDescription("White only captures enemy")
        );

        Add(
            PieceTestCase
                .From("e3", blackMan)
                .GoesTo("d2", "f2")
                .WithDescription("Black goes backwards")
        );

        Add(
            PieceTestCase
                .From("e5", blackMan)
                .WithFriendlyPieceAt("d4")
                .WithEnemyPieceAt("f4")
                .GoesTo("g3", captures: ["f4"])
                .WithDescription("Black only captures enemy")
        );
    }
}
