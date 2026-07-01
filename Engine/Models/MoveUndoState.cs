namespace Engine.Models;

public unsafe struct MoveUndoState
{
    private const int SquareBits = 7; // 0-99
    private const int PieceBits = 6; // up to 63 pieces
    private const int ColorBits = 2;

    private const int PieceShift = SquareBits;
    private const int ColorShift = SquareBits + PieceBits;

    private const ushort SquareMask = (1 << SquareBits) - 1;
    private const ushort PieceMask = (1 << PieceBits) - 1;

    private const int MaxCaptures = 16;

    public required byte From;
    public required byte To;
    public required Piece Piece;
    public required BoardBits CaptureMask;

    private fixed ushort _capturesPacked[MaxCaptures];
    public byte CaptureCount;

    public required bool IsPromotion;
    public required bool IsWhiteToMove;

    public required ulong ZobristKey;

    public void AddCapture(byte square, PieceType piece, PieceColor color)
    {
        if (CaptureCount >= MaxCaptures)
        {
            throw new InvalidOperationException("Too many captures");
        }

        // [color bits][piece bits][square bits]
        ushort data = (ushort)(((byte)color << ColorShift) | ((byte)piece << PieceShift) | square);
        _capturesPacked[CaptureCount++] = data;
    }

    public readonly (byte Position, PieceType PieceType, PieceColor Color) GetCapture(int index)
    {
        if (index >= CaptureCount)
        {
            throw new IndexOutOfRangeException();
        }

        ushort data = _capturesPacked[index];
        byte square = (byte)(data & SquareMask);
        PieceType piece = (PieceType)((data >> PieceShift) & PieceMask);
        PieceColor color = (PieceColor)(data >> ColorShift);

        return (Position: square, PieceType: piece, Color: color);
    }
}
