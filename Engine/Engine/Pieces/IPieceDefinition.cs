using Engine.Models;

namespace Engine.Pieces;

public interface IPieceDefinition
{
    void GenerateMoves(
        BitBoard board,
        Piece piece,
        byte position,
        Span<Move> moves,
        ref int moveCount
    );
}
