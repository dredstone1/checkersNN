namespace Checkers.models;

public enum Cell
{
    EMPTY_C,
    BLACK_C,
    WHITE_C,
}

public class Board
{
    private readonly Cell[] _board = new Cell[64];

    public void ResetBoard()
    {
        ClearBoard();

        ResetBoard(0);
        ResetBoard(5);
    }

    public void ResetBoard(int offset)
    {
        for (int i = offset; i < 3 * 8; ++i)
        {
            if ((i + i / 8) % 2 == 0)
            {
                _board[i] = Cell.BLACK_C;
            }
        }
    }

    public void ClearBoard()
    {
        for (int i = 0; i < 64; ++i)
        {
            _board[i] = Cell.EMPTY_C;
        }
    }
}
