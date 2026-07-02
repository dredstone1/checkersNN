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

        // for testing
        _board[4] = Cell.WHITEQ_C;
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
            handleDestroyN(s1, s2);
    }

    void handleDestroyN(int s1, int s2)
    {
        Vector2i pos1 = IndexToPos(s1);
        Vector2i pos2 = IndexToPos(s2);

        int dx = Math.Abs(pos1.X - pos2.X);
        int dy = Math.Abs(pos1.Y - pos2.Y);

        if (dx != dy || dx != 2)
            return;

        Vector2i pos3 = pos1 + pos2 / 2;

        int i = PosToIndex(pos3);
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

    public int PosToIndex(Vector2i pos)
    {
        return PosToIndex(pos.X, pos.Y);
    }

    public Vector2i IndexToPos(int i)
    {
        int x = i % 8;
        int y = i / 8;

        return new Vector2i(x, y);
    }

    public int GetDDiagonl(int s1, int s2)
    {
        Vector2i pos1 = IndexToPos(s1);
        Vector2i pos2 = IndexToPos(s2);

        int d = Math.Abs(pos1.X - pos2.X);

        int dx = 1,
            dy = 1;
        if (pos2.X - pos1.X < 0)
            dx = -1;
        if (pos2.Y - pos1.Y < 0)
            dy = -1;

        for (int i = 1; i < d; ++i)
        {
            if (_board[PosToIndex(pos1.X + i * dx, pos1.Y + i * dy)] == Cell.EMPTY_C)
                continue;
            return i + 1;
        }

        return 0;
    }

    public float[] getNNData()
    {
        float[] data = new float[192];

        for (int i = 0; i < 64; ++i)
        {
            if (_board[i] == Cell.EMPTY_C)
            {
                data[i] = 1;
            }
        }

        for (int i = 64; i < 64 * 2; ++i)
        {
            if (_board[i - 64] == Cell.BLACKN_C)
                data[i] = 1;
            else if (_board[i - 64] == Cell.BLACKQ_C)
                data[i] = 0.5f;
        }

        for (int i = 64 * 2; i < 64 * 3; ++i)
        {
            if (_board[i - 128] == Cell.WHITEN_C)
                data[i] = 1;
            else if (_board[i - 128] == Cell.WHITEQ_C)
                data[i] = 0.5f;
        }
        return data;
    }
}
