using System.Diagnostics.CodeAnalysis;
using Engine.Models;

namespace Engine;

public class BitBoard
{
    public BoardBits[,] Bitboards { get; }
    public Piece?[] PieceAt { get; }

    public BoardBits WhitePieces { get; private set; }
    public BoardBits BlackPieces { get; private set; }
    public BoardBits Occupancy { get; private set; }

    public ulong ZobristKey { get; private set; }

    public bool IsWhiteToMove { get; private set; } = true;

    public BitBoard(Dictionary<int, Piece> pieces)
    {
        Bitboards = new BoardBits[
            Enum.GetValues<PieceColor>().Length,
            Enum.GetValues<PieceType>().Length
        ];
        PieceAt = new Piece?[Constants.BoardSize * Constants.BoardSize];

        foreach (var (point, piece) in pieces)
        {
            Bitboards[(int)piece.Color, (int)piece.Type] |= BoardBits.One << point;
            PieceAt[point] = piece;
        }

        for (int i = 0; i < Enum.GetValues<PieceType>().Length; i++)
        {
            WhitePieces |= Bitboards[(int)PieceColor.White, i];
            BlackPieces |= Bitboards[(int)PieceColor.Black, i];
        }

        ZobristKey = Zobrist.Compute(this);
    }

    public ref BoardBits BitboardFor(PieceType pieceType, PieceColor color) =>
        ref Bitboards[(int)color, (int)pieceType];

    public bool TryGetPieceAt(byte position, [NotNullWhen(true)] out Piece? piece)
    {
        piece = PieceAt[position];
        return piece is not null;
    }

    public Piece? GetPieceAt(byte position) => PieceAt[position];

    public NullMoveUndoState MakeNullMove()
    {
        NullMoveUndoState undo = new() { IsWhiteToMove = IsWhiteToMove, ZobristKey = ZobristKey };

        ZobristKey ^= Zobrist.SideToMove;
        IsWhiteToMove = !IsWhiteToMove;

        return undo;
    }

    public void UndoNullMove(NullMoveUndoState undo)
    {
        IsWhiteToMove = undo.IsWhiteToMove;
        ZobristKey = undo.ZobristKey;
    }

    public void UndoMove(MoveUndoState undoState)
    {
        if (undoState.IsPromotion)
        {
            RemovePiece(PieceType.King, undoState.Piece.Color, at: undoState.To);
            SpawnPiece(PieceType.Man, undoState.Piece.Color, at: undoState.From);
        }
        else
        {
            MovePiece(
                undoState.Piece.Type,
                undoState.Piece.Color,
                from: undoState.To,
                to: undoState.From,
                isPromotion: false
            );
        }

        for (int i = 0; i < undoState.CaptureCount; i++)
        {
            (byte position, PieceType pieceType, PieceColor color) = undoState.GetCapture(i);
            SpawnPiece(pieceType, color, position);
        }

        IsWhiteToMove = undoState.IsWhiteToMove;
        ZobristKey = undoState.ZobristKey;

        ComputeAggregateBitboards();
    }

    public MoveUndoState MakeMove(Move move)
    {
        MoveUndoState undoState = new()
        {
            From = move.From,
            To = move.To,
            Piece = move.Piece,
            CaptureMask = move.CapturesMask,
            IsPromotion = move.IsPromotion,
            IsWhiteToMove = IsWhiteToMove,
            ZobristKey = ZobristKey,
        };

        UInt128 captureMask = move.CapturesMask;
        while (captureMask != 0)
        {
            byte captureSquare = BitboardHelpers.BitScanForward(ref captureMask);
            if (TryGetPieceAt(captureSquare, out var piece))
            {
                RemovePiece(piece.Value.Type, piece.Value.Color, captureSquare);
                undoState.AddCapture(captureSquare, piece.Value.Type, piece.Value.Color);
            }
        }

        MovePiece(
            move.Piece.Type,
            move.Piece.Color,
            move.From,
            move.To,
            isPromotion: move.IsPromotion
        );

        ComputeAggregateBitboards();
        ZobristKey ^= Zobrist.SideToMove;
        IsWhiteToMove = !IsWhiteToMove;

        return undoState;
    }

    private void MovePiece(
        PieceType pieceType,
        PieceColor color,
        byte from,
        byte to,
        bool isPromotion
    )
    {
        if (isPromotion)
        {
            RemovePiece(pieceType, color, at: from);
            SpawnPiece(PieceType.King, color, at: to);
            return;
        }

        if (TryGetPieceAt(to, out var piece))
        {
            RemovePiece(piece.Value.Type, piece.Value.Color, to);
        }

        ref UInt128 bitboard = ref BitboardFor(pieceType, color);

        UInt128 fromMask = UInt128.One << from;
        UInt128 toMask = UInt128.One << to;
        bitboard &= ~fromMask;
        bitboard |= toMask;

        switch (color)
        {
            case PieceColor.White:
                WhitePieces = (WhitePieces & ~fromMask) | toMask;
                break;
            case PieceColor.Black:
                BlackPieces = (BlackPieces & ~fromMask) | toMask;
                break;
        }
        (PieceAt[from], PieceAt[to]) = (null, PieceAt[from]);

        ZobristKey ^= Zobrist.PieceSquare[(int)pieceType, (int)color, from];
        ZobristKey ^= Zobrist.PieceSquare[(int)pieceType, (int)color, to];
    }

    private void RemovePiece(PieceType pieceType, PieceColor color, byte at)
    {
        UInt128 atMask = UInt128.One << at;
        UInt128 inverseMask = ~atMask;

        ref UInt128 bitboard = ref BitboardFor(pieceType, color);
        bitboard &= inverseMask;

        switch (color)
        {
            case PieceColor.White:
                WhitePieces &= inverseMask;

                break;
            case PieceColor.Black:
                BlackPieces &= inverseMask;

                break;
        }
        PieceAt[at] = null;
        ZobristKey ^= Zobrist.PieceSquare[(int)pieceType, (int)color, at];
    }

    private void SpawnPiece(PieceType type, PieceColor color, byte at)
    {
        if (TryGetPieceAt(at, out var piece))
        {
            RemovePiece(piece.Value.Type, piece.Value.Color, at);
        }

        UInt128 mask = UInt128.One << at;
        ref UInt128 bitboard = ref BitboardFor(type, color);
        bitboard |= mask;

        switch (color)
        {
            case PieceColor.White:
                WhitePieces |= mask;
                break;
            case PieceColor.Black:
                BlackPieces |= mask;
                break;
        }
        PieceAt[at] = new Piece() { Type = type, Color = color };
        ZobristKey ^= Zobrist.PieceSquare[(int)type, (int)color, at];
    }

    private void ComputeAggregateBitboards()
    {
        Occupancy = WhitePieces | BlackPieces;
        //Empty = ~Occupancy;

        //WhiteEnemy = BlackPieces | NeutralPieces;
        //BlackEnemy = WhitePieces | NeutralPieces;
    }
}
