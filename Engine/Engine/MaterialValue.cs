using Engine.Models;

namespace Engine;

public static class MaterialValue
{
    public static int GetPieceValue(PieceType type) =>
        type switch
        {
            PieceType.Man => 100,
            PieceType.King => 300,

            _ => 0,
        };
}
