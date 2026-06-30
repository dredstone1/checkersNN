namespace Checkers.models;

public class Game
{
    Display display = new Display();

    public Game() { }

    public void Start()
    {
        Console.WriteLine("Checkers starting");
        display.StartDisplay();
    }
}
