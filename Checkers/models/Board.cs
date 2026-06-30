namespace Checkers.models;

public enum Cell
{
    EMPTY_C,
    BLACKN_C,
    WHITEN_C,
    BLACKQ_C,
    WHITEQ_C,
}

public enum CellP
{
    EMPTY_C,
    WHITE_C,
    BLACK_C,
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

        ResetBoard(0, 3, Cell.BLACKN_C);
        ResetBoard(5 * 8, 3, Cell.WHITEN_C);
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

    public CellP getCellPlayer(int i)
    {
        if (_board[i] == Cell.EMPTY_C)
            return CellP.EMPTY_C;
        if (_board[i] == Cell.BLACKQ_C || _board[i] == Cell.BLACKN_C)
            return CellP.BLACK_C;
        return CellP.WHITE_C;
    }

    public PlayerType CellTypeToPlayerType(CellP c) {
        if (c == CellP.BLACK_C)
            return PlayerType.BLACK;
        return PlayerType.WHITE;
    }

    public void Move(int s1, int s2) {
        
    }
}
