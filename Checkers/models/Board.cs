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
    public IReadOnlyList<Cell> cells => _board;

    public Board()
    {
        ResetBoard();
    }

    public void ResetBoard()
    {
        ClearBoard();

        ResetBoard(0, 3, Cell.BLACK_C);
        ResetBoard(5 * 8, 3, Cell.WHITE_C);
    }

    void ResetBoard(int offset, int layers, Cell c)
    {
        for (int i = offset; i < offset + layers * 8; ++i)
        {
            if ((i + i / 8) % 2 == 0)
            {
                _board[i] = c;
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
