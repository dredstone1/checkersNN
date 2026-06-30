using SFML.System;

namespace Checkers.models;

public class Game
{
    private PlayerType _currentPlayer = PlayerType.WHITE;
    private bool _running = false;

    private readonly Board _board;
    private readonly Display _display;

    public Game()
    {
        _board = new();
        _display = new Display(_board);
    }

    void toggleCurrentPlayer()
    {
        _currentPlayer = (_currentPlayer == PlayerType.BLACK) ? PlayerType.WHITE : PlayerType.BLACK;
    }

    bool checkMovmentType(int s1, int s2)
    {
        int x1 = s1 % 8;
        int y1 = s1 / 8;

        int x2 = s2 % 8;
        int y2 = s2 / 8;

        int dx = Math.Abs(x1 - x2);
        int dy = Math.Abs(y1 - y2);

        if (dx != dy)
            return true;

        if (dx == 2)
        {
            int x3 = (x1 + x2) / 2;
            int y3 = (y1 + y2) / 2;

            if (_board.cells[_board.PosToIndex(x3, y3)] == Cell.EMPTY_C)
                return true;
            
        }
        else if (_board.PModeFromCellIndex(s1) == PlayerMode.NORMAL && dx != 1)
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

        if (_display.cellselected2 != -1)
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
