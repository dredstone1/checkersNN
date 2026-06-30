namespace Checkers.models;

public class Game
{
    private  PlayerType _currentPlayer = PlayerType.WHITE;
    private bool _running = false;

    private readonly Board _board;
    private readonly Display _display;

    public Game()
    {
        _board = new();
        _display = new Display(_board);
    }

    void toggleCurrentPlayer() {
        if (_currentPlayer == PlayerType.BLACK)
            _currentPlayer = PlayerType.WHITE;
        else
            _currentPlayer = PlayerType.BLACK;
    }

    bool AttemptMove(int s1, int s2)
    {
        CellP cellType1 = _board.getCellPlayer(s1);
        CellP cellType2 = _board.getCellPlayer(s2);
        if (cellType2 != CellP.EMPTY_C)
            return false;

        if (_board.CellTypeToPlayerType(cellType1) != _currentPlayer)
            return false;
    

        _board.Move(s1, s2);
        toggleCurrentPlayer();
        return true;
    }

    public void GameLoop()
    {
        _running = true;

        while (_running && _display.IsRunning)
        {
            _display.Update();

            if (_display.cellselected2 != -1)
            {
                Console.WriteLine($"test333: {AttemptMove(_display.cellselected1, _display.cellselected2)}");
                _display.resetSelection();
            }
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
