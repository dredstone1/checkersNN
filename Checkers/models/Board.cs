using SFML.System;

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

public enum PlayerMode
{
    QUEEN,
    NORMAL,
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
            if ((i + i / 8) % 2 != 0)
                continue;

            _board[i] = c;
        }
    }

    public void ClearBoard()
    {
        for (int i = 0; i < 64; ++i)
            _board[i] = Cell.EMPTY_C;
    }

    public CellP getCellPlayer(int i)
    {
        if (_board[i] == Cell.EMPTY_C)
            return CellP.EMPTY_C;

        if (_board[i] == Cell.BLACKQ_C || _board[i] == Cell.BLACKN_C)
            return CellP.BLACK_C;

        return CellP.WHITE_C;
    }

    public PlayerType CellTypeToPlayerType(CellP c)
    {
        if (c == CellP.BLACK_C)
            return PlayerType.BLACK;

        return PlayerType.WHITE;
    }

    void handleDestroy(int s1, int s2)
    {
        if (PModeFromCellIndex(s1) == PlayerMode.NORMAL)
            handleDestroyN(s1, s2);
        else
            handleDestroyQ(s1, s2);
    }

    void handleDestroyN(int s1, int s2)
    {
        int x1 = s1 % 8;
        int y1 = s1 / 8;

        int x2 = s2 % 8;
        int y2 = s2 / 8;

        int dx = Math.Abs(x1 - x2);
        int dy = Math.Abs(y1 - y2);

        if (dx != dy || dx != 2)
            return;

        int x3 = (x1 + x2) / 2;
        int y3 = (y1 + y2) / 2;

        int i = PosToIndex(x3, y3);
        if (_board[i] != Cell.EMPTY_C)
            _board[i] = Cell.EMPTY_C;
    }

    void handleDestroyQ(int s1, int s2) { }

    public void Move(int s1, int s2)
    {
        handleDestroy(s1, s2);

        if (_board[s1] == Cell.BLACKN_C && s2 > 54)
            _board[s2] = Cell.BLACKQ_C;
        else if (_board[s1] == Cell.WHITEN_C && s2 < 8)
            _board[s2] = Cell.WHITEQ_C;
        else
            _board[s2] = _board[s1];

        _board[s1] = Cell.EMPTY_C;
    }

    public PlayerMode PModeFromCellIndex(int i)
    {
        if (_board[i] == Cell.BLACKQ_C || _board[i] == Cell.WHITEQ_C)
            return PlayerMode.QUEEN;

        return PlayerMode.NORMAL;
    }

    public int PosToIndex(int x, int y)
    {
        return y * 8 + x;
    }

    public Vector2f IndexToPos(int i)
    {
        int x = i % 8;
        int y = i / 8;

        return new Vector2f(x, y);
    }
}
