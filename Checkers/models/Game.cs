namespace Checkers.models;

public class Game
{
    private readonly PlayerType _currentPlayer = PlayerType.WHITE;
    private bool _running = false;

    private readonly Board _board;
    private readonly Display _display;

    public Game()
    {
        _board = new();
        _display = new Display(_board);
    }

    public void GameLoop()
    {
        _running = true;

        while (_running && _display.IsRunning)
        {
            _display.Update();
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
