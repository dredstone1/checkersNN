using Engine;
using Engine.Models;

BitBoard board = new(
    pieces: new()
    {
        [0] = new() { Type = PieceType.Man, Color = PieceColor.White },
        [2] = new() { Type = PieceType.Man, Color = PieceColor.White },
        [4] = new() { Type = PieceType.Man, Color = PieceColor.White },
        [6] = new() { Type = PieceType.Man, Color = PieceColor.White },
        [9] = new() { Type = PieceType.Man, Color = PieceColor.White },
        [11] = new() { Type = PieceType.Man, Color = PieceColor.White },
        [13] = new() { Type = PieceType.Man, Color = PieceColor.White },
        [15] = new() { Type = PieceType.Man, Color = PieceColor.White },
        [16] = new() { Type = PieceType.Man, Color = PieceColor.White },
        [18] = new() { Type = PieceType.Man, Color = PieceColor.White },
        [20] = new() { Type = PieceType.Man, Color = PieceColor.White },
        [22] = new() { Type = PieceType.Man, Color = PieceColor.White },

        [41] = new() { Type = PieceType.Man, Color = PieceColor.Black },
        [43] = new() { Type = PieceType.Man, Color = PieceColor.Black },
        [45] = new() { Type = PieceType.Man, Color = PieceColor.Black },
        [47] = new() { Type = PieceType.Man, Color = PieceColor.Black },
        [48] = new() { Type = PieceType.Man, Color = PieceColor.Black },
        [50] = new() { Type = PieceType.Man, Color = PieceColor.Black },
        [52] = new() { Type = PieceType.Man, Color = PieceColor.Black },
        [54] = new() { Type = PieceType.Man, Color = PieceColor.Black },
        [57] = new() { Type = PieceType.Man, Color = PieceColor.Black },
        [59] = new() { Type = PieceType.Man, Color = PieceColor.Black },
        [61] = new() { Type = PieceType.Man, Color = PieceColor.Black },
        [63] = new() { Type = PieceType.Man, Color = PieceColor.Black },
    },
    isWhiteToMove: true
);

var (bestMove, evalForBot) = CheckersEngine.FindBestMove(board, depth: 20);
if (bestMove is null)
{
    return;
}
Console.WriteLine(
    $"From: {bestMove.Value.From}, To: {bestMove.Value.To}, evalForBot: {evalForBot}"
);
