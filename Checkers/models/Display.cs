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
    }

    public void StartDisplay()
    {
        running = true;
    }

    public void CloseDisplay()
    {
        running = false;
    }

    public void Update()
    {
        window.DispatchEvents();
        window.Clear(Color.White);




        window.Display();
    }
}
