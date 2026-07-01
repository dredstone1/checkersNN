using System.Runtime.CompilerServices;
using Engine.Models;
using Engine.Pieces;

namespace Engine;

public class MoveGenerator
{
    private static readonly IPieceDefinition _manDefinition = new ManDefinition();
    private static readonly IPieceDefinition _kingDefinition = new KingDefinition();

    public static void Generate(BitBoard board, Span<Move> moves, ref int moveCount)
    {
        PieceColor color = board.IsWhiteToMove ? PieceColor.White : PieceColor.Black;

        GenerateForPieces(
            board,
            piece: new Piece() { Type = PieceType.Man, Color = color },
            _manDefinition,
            moves,
            ref moveCount
        );

        GenerateForPieces(
            board,
            piece: new Piece() { Type = PieceType.King, Color = color },
            _kingDefinition,
            moves,
            ref moveCount
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void GenerateForPieces(
        BitBoard board,
        Piece piece,
        IPieceDefinition definition,
        Span<Move> moves,
        ref int moveCount
    )
    {
        BoardBits bitboard = board.BitboardFor(piece.Type, piece.Color);
        while (bitboard != 0)
        {
            int squareIndex = BitboardHelpers.BitScanForward(ref bitboard);
            byte position = (byte)squareIndex;

            definition.GenerateMoves(board, piece, position, moves, ref moveCount);
        }
    }
}
