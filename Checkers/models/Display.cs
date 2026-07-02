using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Checkers.models;

public class Display
{
    public const int GRID_RES = 600;
    public const int GRID_SIZE = 8;
    public const int SQUARE_RES = GRID_RES / GRID_SIZE;

    private bool running = false;
    public bool IsRunning
    {
        get { return running; }
    }

    private readonly RenderWindow _window;

    private readonly Board _board;

    public int cellselected1 = -1;
    public int cellselected2 = -1;

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
        if (c == Cell.BLACKN_C || c == Cell.BLACKQ_C)
            return Color.Black;

        return Color.White;
    }

    void DrawPlayers()
    {
        for (int i = 0; i < GRID_SIZE * GRID_SIZE; ++i)
        {
            if (_board.cells[i] == Cell.EMPTY_C)
                continue;

            Vector2i pos = Board.IndexToPos(i);

            if (i == cellselected1)
                pos += (10, 10);

            DrawPlayer((Vector2f)pos, getPLayerColor(_board.cells[i]), isQueen(_board.cells[i]));
        }
    }

    static bool isQueen(Cell c)
    {
        return (c == Cell.BLACKQ_C || c == Cell.WHITEQ_C);
    }

    public void DrawPlayer(Vector2f pos, Color c, bool queen)
    {
        CircleShape rect = new CircleShape
        {
            Radius = SQUARE_RES * 0.3f,
            Position = pos + (20f, 20f) + (SQUARE_RES * 0.2f, SQUARE_RES * 0.2f),
            FillColor = c,
        };

        if (queen)
        {
            CircleShape rect1 = new CircleShape
            {
                Radius = SQUARE_RES * 0.3f,
                Position = pos + (30f, 30f) + (SQUARE_RES * 0.2f, SQUARE_RES * 0.2f),
                FillColor = c,
            };

            _window.Draw(rect1);
        }
        _window.Draw(rect);
    }

    public static Color GetSquareColor(int i) =>
        ((i + i / GRID_SIZE) % 2 == 0) ? new Color(205, 133, 63) : new Color(139, 69, 19);

    public void DrawBoard()
    {
        for (int i = 0; i < GRID_SIZE * GRID_SIZE; ++i)
        {
            DrawSquare(Board.IndexToPos(i), GetSquareColor(i));
        }
    }

    public void DrawSquare(Vector2i pos, Color c)
    {
        RectangleShape rect = new(new Vector2f(SQUARE_RES, SQUARE_RES))
        {
            Position = (Vector2f)pos + (20, 20),
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

        _window.MouseButtonPressed += (sender, e) =>
        {
            if (e.Button == Mouse.Button.Left)
            {
                HandleMove(e.Position);
            }
        };
    }

    int getIndexFromPos(Vector2i pos)
    {
        int x = pos.X / SQUARE_RES;
        int y = pos.Y / SQUARE_RES;

        return (y * GRID_SIZE + x);
    }

    void HandleMove(Vector2i pos)
    {
        pos -= (20, 20);

        if (pos.X < 0 || pos.Y < 0 || pos.X > GRID_RES || pos.Y > GRID_RES)
            return;

        if (cellselected1 != -1)
            cellselected2 = getIndexFromPos(pos);
        else
            cellselected1 = getIndexFromPos(pos);
    }

    public void resetSelection()
    {
        cellselected1 = -1;
        cellselected2 = -1;
    }
}
