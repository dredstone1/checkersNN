namespace Checkers.models;

public class Game
{
    Display display = new Display();

    PlayerType CP = PlayerType.WHITE; // Current Player

    bool running = false;

    public Game( ) 
    { 
    }

    public void GameLoop() {
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
