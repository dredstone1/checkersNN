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
                .WithDescription("White man goes forwards")
        );

        Add(
            PieceTestCase
                .From("e3", blackMan)
                .GoesTo("d2", "f2")
                .WithDescription("Black man goes backwards")
        );
    }
}
