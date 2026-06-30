namespace Checkers.models;

public class Game
{
    PlayerType CP = PlayerType.WHITE; // Current Player

    bool running = false;

    Board board;

    Display display;

    public Game()
    {
        display = new Display(board);
    }

    public void GameLoop()
    {
        running = true;

        while (running && display.IsRunning)
        {
            display.Update();
        }
    }

    public void Start()
    {
        Console.WriteLine("Checkers starting");
        display.StartDisplay();

        GameLoop();

        display.CloseDisplay();
    }
}
