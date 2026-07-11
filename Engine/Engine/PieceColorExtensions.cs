using Engine.Models;

namespace Engine;

public static class PieceColorExtensions
{
    public static PieceColor Invert(this PieceColor color) => (PieceColor)(1 ^ (int)color);
}
