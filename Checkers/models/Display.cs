using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Checkers.models;

public class Display
{
    const int GRID_RES = 600;
    const int GRID_SIZE = 8;
    const int SQUARE_RES = GRID_RES / GRID_SIZE;

    private bool running = false;
    public bool IsRunning
    {
        get { return running; }
    }

    private readonly RenderWindow _window;

    private readonly Board _board;

    public Display(Board board)
    {
        _board = board;
        _window = new RenderWindow(new VideoMode((800, 1000)), "Checkers");
        _window.SetFramerateLimit(60);

        Events();
    }

    public void StartDisplay()
    {
        running = true;
    }

    public void CloseDisplay()
    {
        _window.Close();
        running = false;
    }

    public void Draw()
    {
        DrawBoard();
    }

    public static Color GetSquareColor(int i) =>
        ((i + i / GRID_SIZE) % 2 == 0) ? new Color(205, 133, 63) : new Color(139, 69, 19);

    public void DrawBoard()
    {
        for (int i = 0; i < GRID_SIZE * GRID_SIZE; ++i)
        {
            int x = (i % GRID_SIZE) * SQUARE_RES;
            int y = (i / GRID_SIZE) * SQUARE_RES;

            DrawSquare((x, y), GetSquareColor(i));
        }
    }

    public void DrawSquare(Vector2f pos, Color c)
    {
        RectangleShape rect = new(new Vector2f(SQUARE_RES, SQUARE_RES))
        {
            Position = pos + (20, 20),
            FillColor = c,
        };
        _window.Draw(rect);
    }

    public void Update()
    {
        _window.DispatchEvents();
        _window.Clear(Color.Yellow);

        Draw();

        _window.Display();
    }

    public void Events()
    {
        _window.Closed += (_, __) =>
        {
            CloseDisplay();
        };
    }
}
