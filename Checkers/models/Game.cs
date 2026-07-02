using SFML.System;

namespace Checkers.models;

public class Game
{
    IntPtr model;

    private PlayerType _currentPlayer = PlayerType.WHITE;
    private bool _running = false;

    private readonly Board _board;
    private readonly Display _display;

    public Game()
    {
        _board = new();
        _display = new Display(_board);
        model = Model.Model_Create();
    }

    void toggleCurrentPlayer()
    {
        _currentPlayer = (_currentPlayer == PlayerType.BLACK) ? PlayerType.WHITE : PlayerType.BLACK;
    }

    bool checkMovmentDirection(int s1, int s2)
    {
        return (_board.getCellPlayer(s1) == CellP.BLACK_C) ? (s1 > s2) : (s1 < s2);
    }

    bool checkMovmentType(int s1, int s2)
    {
        Vector2i pos1 = _board.IndexToPos(s1);
        Vector2i pos2 = _board.IndexToPos(s2);

        int dx = Math.Abs(pos1.X - pos2.X);
        int dy = Math.Abs(pos1.Y - pos2.Y);

        if (dx != dy)
            return true;

        if (_board.PModeFromCellIndex(s1) == PlayerMode.NORMAL)
        {
            if (checkMovmentDirection(s1, s2))
                return true;

            if (dx == 2)
            {
                Vector2i pos3 = pos1 + pos2 / 2;

                if (_board.cells[_board.PosToIndex(pos3)] == Cell.EMPTY_C)
                    return true;
            }
            else if (_board.PModeFromCellIndex(s1) == PlayerMode.NORMAL && dx != 1)
                return true;
        }
        else if (dx > 1 && _board.GetDDiagonl(s1, s2) != dx)
            return true;

        return false;
    }

    bool AttemptMove(int s1, int s2)
    {
        CellP cellType1 = _board.getCellPlayer(s1);
        CellP cellType2 = _board.getCellPlayer(s2);

        if (
            cellType2 != CellP.EMPTY_C
            || _board.CellTypeToPlayerType(cellType1) != _currentPlayer
            || checkMovmentType(s1, s2)
        )
            return false;

        _board.Move(s1, s2);
        toggleCurrentPlayer();
        return true;
    }

    void update()
    {
        _display.Update();

        if (_currentPlayer == PlayerType.BLACK)
        {
            float[] list = new float[128];

            Model.Model_Run(model, _board.getNNData(), list);

            int h1 = 0;
            int h2 = 0;
            for (int i = 0; i < 64; ++i)
            {
                if (list[h1] < list[i])
                    h1 = i;
            }
            for (int i = 0; i < 64; ++i)
            {
                if (list[h2 + 64] < list[i + 64])
                    h2 = i;
            }
            Console.WriteLine($"s1: {h1}, s2: {h2}");

            toggleCurrentPlayer();
        }
        else if (_display.cellselected2 != -1)
        {
            AttemptMove(_display.cellselected1, _display.cellselected2);
            _display.resetSelection();
        }
    }

    public void GameLoop()
    {
        _running = true;

        while (_running && _display.IsRunning)
        {
            update();
        }
    }

    public void Start()
    {
        Console.WriteLine("Checkers starting");
        _display.StartDisplay();

        GameLoop();

        _display.CloseDisplay();
    }
}
