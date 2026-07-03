namespace Engine.Models;

public struct Move
{
    public required byte From;
    public required byte To;
    public required Piece Piece;

    public BoardBits CapturesMask;
    public bool IsPromotion;

    public readonly int Pack() => From | (To << 7) | ((IsPromotion ? (byte)1 : (byte)0) << 14);
}
