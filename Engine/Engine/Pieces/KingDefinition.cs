using Engine.Magic;
using Engine.Models;

namespace Engine.Pieces;

public class KingDefinition : IPieceDefinition
{
    public void GenerateMoves(
        BitBoard board,
        Piece piece,
        byte position,
        Span<Move> moves,
        ref int moveCount
    )
    {
        BoardBits attacks = MagicLibrary.GetAttacks(
            MagicLibrary.KingTable,
            position,
            board.Occupancy
        );

        BoardBits nonCaptures = attacks & board.Empty;
        while (nonCaptures != 0)
        {
            byte to = BitboardHelpers.BitScanForward(ref nonCaptures);
            moves[moveCount++] = new()
            {
                From = position,
                To = to,
                Piece = piece,
            };
        }

        BoardBits captures = attacks & board.BitboardForEnemyOf(piece.Color);
        captures &= BitboardConstants.NotEdgeMasks;

        int y = position / Constants.BoardSize;
        int x = position % Constants.BoardSize;
        while (captures != 0)
        {
            byte to = BitboardHelpers.BitScanForward(ref captures);
            int toY = to / Constants.BoardSize;
            int toX = to % Constants.BoardSize;

            int dx = Math.Sign(x - toX);
            int dy = Math.Sign(y - toY);

            toX -= dx;
            toY -= dy;

            moves[moveCount++] = new()
            {
                From = position,
                To = (byte)(toY * Constants.BoardSize + toX),
                Piece = piece,
                CapturesMask = BitboardHelpers.One << to,
            };
        }
    }
}
