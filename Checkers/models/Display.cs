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
        DrawPlayers();
    }

    Color getPLayerColor(Cell c)
    {
        if (c == Cell.BLACK_C)
        {
            return Color.Black;
        }

        return Color.White;
    }

    void DrawPlayers()
    {
        for (int i = 0; i < GRID_SIZE * GRID_SIZE; ++i)
        {
            if (_board.cells[i] == Cell.EMPTY_C)
                continue;

            int x = (i % GRID_SIZE) * SQUARE_RES;
            int y = (i / GRID_SIZE) * SQUARE_RES;

            DrawPlayer((x, y), getPLayerColor(_board.cells[i]));
        }
    }

    public void DrawPlayer(Vector2f pos, Color c)
    {
        CircleShape rect = new CircleShape
        {
            Radius = SQUARE_RES * 0.3f,
            Position = (pos + (20f, 20f)) + (SQUARE_RES * 0.2f, SQUARE_RES * 0.2f),
            FillColor = c,
        };
        _window.Draw(rect);
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
