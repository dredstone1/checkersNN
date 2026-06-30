using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Checkers.models;

public class Display
{
    private bool running = false;
    public bool IsRunning
    {
        get { return running; }
    }

    RenderWindow window;

    public Display()
    {
        window = new RenderWindow(new VideoMode((800, 1000)), "Checkers");
        window.SetFramerateLimit(60);

        events();
    }

    public void StartDisplay()
    {
        running = true;
    }

    public void CloseDisplay()
    {
        window.Close();
        running = false;
    }

    void Draw()
    {
        DrawBoard();
    }

    void DrawBoard() { }

    public void Update()
    {
        window.DispatchEvents();
        window.Clear(Color.White);

        Draw();

        window.Display();
    }

    void events()
    {
        window.Closed += (_, __) =>
        {
            CloseDisplay();
        };
    }
}
