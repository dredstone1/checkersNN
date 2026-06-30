using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Checkers.models;

public class Display
{
    const int GRID_RES = 600;

    const int GRID_SIZE = 8;
    const int SQUARE_RES = GRID_RES/GRID_SIZE;

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

    Color getSquareColor(int i)
    {
        if ((i + (i / GRID_SIZE)) % 2 == 0)
        {
            return Color.White;
        }
        else
        {
            return Color.Black;
        }
    }

    void DrawBoard()
    {
        for (int i = 0; i < GRID_SIZE*GRID_SIZE; ++i)
        {
            DrawSquare(((i % GRID_SIZE) * SQUARE_RES, (i / GRID_SIZE) * SQUARE_RES), getSquareColor(i));
        }
    }

    void DrawSquare(Vector2f pos, Color c)
    {
        RectangleShape rect = new RectangleShape(new Vector2f(SQUARE_RES, SQUARE_RES));
        rect.Position = pos + (20, 20);
        rect.FillColor = c;
        window.Draw(rect);
    }

    public void Update()
    {
        window.DispatchEvents();
        window.Clear(Color.Yellow);

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
