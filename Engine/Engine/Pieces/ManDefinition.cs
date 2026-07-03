using Engine.Models;

namespace Engine.Pieces;

public class ManDefinition : IPieceDefinition
{
    public void GenerateMoves(
        BitBoard board,
        Piece piece,
        byte position,
        Span<Move> moves,
        ref int moveCount
    )
    {
        BoardBits positionBit = BitboardConstants.One << position;
        var (left, right, jumpLeft, jumpRight, diagonals) =
            piece.Color is PieceColor.White
                ? GenerateWhite(board, piece, positionBit)
                : GenerateBlack(board, piece, positionBit);
        BoardBits promotionMask =
            piece.Color is PieceColor.White
                ? BitboardConstants.TopEdgeMask
                : BitboardConstants.BottomEdgeMask;

        BoardBits nonJump = diagonals & board.Empty;
        while (nonJump != 0)
        {
            byte to = BitboardHelpers.BitScanForward(ref nonJump);
            BoardBits toBit = BitboardConstants.One << to;
            moves[moveCount++] = new()
            {
                From = position,
                To = to,
                Piece = piece,
                IsPromotion = (toBit & promotionMask) != 0,
            };
        }

        if (jumpLeft != 0)
        {
            bool isPromotion = (jumpLeft & promotionMask) != 0;
            moves[moveCount++] = new()
            {
                From = position,
                To = BitboardHelpers.BitScanForward(ref jumpLeft),
                Piece = piece,
                CapturesMask = left,
                IsPromotion = isPromotion,
            };
        }
        if (jumpRight != 0)
        {
            bool isPromotion = (jumpRight & promotionMask) != 0;
            moves[moveCount++] = new()
            {
                From = position,
                To = BitboardHelpers.BitScanForward(ref jumpRight),
                Piece = piece,
                CapturesMask = right,
                IsPromotion = isPromotion,
            };
        }
    }

    public static (
        BoardBits left,
        BoardBits right,
        BoardBits leftJump,
        BoardBits rightJump,
        BoardBits diagonals
    ) GenerateWhite(BitBoard board, Piece piece, BoardBits positionBit)
    {
        BoardBits left = BitboardHelpers.ShiftUpLeft(positionBit);
        BoardBits right = BitboardHelpers.ShiftUpRight(positionBit);
        BoardBits diagonals = left | right;

        BoardBits enemies = board.BitboardForEnemyOf(piece.Color);
        BoardBits jumpOver = diagonals & enemies;
        BoardBits leftJump = BitboardHelpers.ShiftUpLeft(jumpOver & left);
        BoardBits rightJump = BitboardHelpers.ShiftUpRight(jumpOver & right);

        return (left, right, leftJump, rightJump, diagonals);
    }

    public static (
        BoardBits left,
        BoardBits right,
        BoardBits leftJump,
        BoardBits rightJump,
        BoardBits diagonals
    ) GenerateBlack(BitBoard board, Piece piece, BoardBits positionBit)
    {
        BoardBits left = BitboardHelpers.ShiftDownLeft(positionBit);
        BoardBits right = BitboardHelpers.ShiftDownRight(positionBit);
        BoardBits diagonals = left | right;

        BoardBits enemies = board.BitboardForEnemyOf(piece.Color);
        BoardBits jumpOver = diagonals & enemies;
        BoardBits leftJump = BitboardHelpers.ShiftDownLeft(jumpOver & left);
        BoardBits rightJump = BitboardHelpers.ShiftDownRight(jumpOver & right);

        return (left, right, leftJump, rightJump, diagonals);
    }
}
