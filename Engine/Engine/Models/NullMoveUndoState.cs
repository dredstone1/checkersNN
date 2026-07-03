namespace Engine.Models;

public struct NullMoveUndoState
{
    public required bool IsWhiteToMove;
    public required ulong ZobristKey;
}
