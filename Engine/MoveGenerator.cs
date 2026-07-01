using System.Runtime.CompilerServices;
using Engine.Models;
using Engine.Pieces;

namespace Engine;

public class MoveGenerator
{
    private static readonly IPieceDefinition[] _pieceDefinitions =
    [
        new ManDefinition(),
        new KingDefinition(),
    ];

    public static void Generate(BitBoard board, Span<Move> moves, ref int moveCount)
    {
        PieceColor color = board.IsWhiteToMove ? PieceColor.White : PieceColor.Black;

        for (int pieceTypeIdx = 0; pieceTypeIdx < board.Bitboards.GetLength(1); pieceTypeIdx++)
        {
            PieceType pieceType = (PieceType)pieceTypeIdx;
            IPieceDefinition definition = _pieceDefinitions[pieceTypeIdx];

            UInt128 colorBitboard = board.BitboardFor(pieceType, color);
            GenerateForPieces(
                board,
                colorBitboard,
                definition,
                piece: new Piece() { Type = pieceType, Color = color },
                moves,
                ref moveCount
            );
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void GenerateForPieces(
        BitBoard board,
        UInt128 bitboard,
        IPieceDefinition definition,
        Piece piece,
        Span<Move> moves,
        ref int moveCount
    )
    {
        while (bitboard != 0)
        {
            int squareIndex = BitboardHelpers.BitScanForward(ref bitboard);
            byte position = (byte)squareIndex;

            definition.GenerateMoves(board, piece, position, moves, ref moveCount);
        }
    }
}
