using SFML.System;

namespace Checkers.models;

public enum PlayerType
{
    BLACK,
    WHITE,
}

public class Game
{
    IntPtr model;
    bool BlackAI = false,
        WhiteAI = false;

    private PlayerType _currentPlayer = PlayerType.WHITE;
    private bool _running = false;

    private readonly Board _board;
    private readonly Display _display;

    public Game()
    {
        _board = new();
        _display = new Display(_board);
    }

    void toggleCurrentPlayer()
    {
        _currentPlayer = (_currentPlayer == PlayerType.BLACK) ? PlayerType.WHITE : PlayerType.BLACK;
    }

    bool checkMovmentDirection(int s1, int s2)
    {
        return (_board.getCellPlayer(s1) == CellP.BLACK_C) ? (s1 > s2) : (s1 < s2);
    }

    bool checkMovmentType(int s1, int s2)
    {
        Vector2i pos1 = Board.IndexToPos(s1);
        Vector2i pos2 = Board.IndexToPos(s2);

        int dx = Math.Abs(pos1.X - pos2.X);
        int dy = Math.Abs(pos1.Y - pos2.Y);

        if (dx != dy)
            return true;

        if (_board.PModeFromCellIndex(s1) == PlayerMode.NORMAL)
        {
            if (checkMovmentDirection(s1, s2))
                return true;

            if (dx == 2)
            {
                Vector2i pos3 = pos1 + pos2 / 2;

                if (_board.cells[Board.PosToIndex(pos3)] == Cell.EMPTY_C)
                    return true;
            }
            else if (_board.PModeFromCellIndex(s1) == PlayerMode.NORMAL && dx != 1)
                return true;
        }
        else if (dx > 1 && _board.GetDDiagonl(s1, s2) != dx)
            return true;

        return false;
    }

    bool AttemptMove(int s1, int s2)
    {
        CellP cellType1 = _board.getCellPlayer(s1);
        CellP cellType2 = _board.getCellPlayer(s2);

        if (
            cellType2 != CellP.EMPTY_C
            || Board.CellTypeToPlayerType(cellType1) != _currentPlayer
            || checkMovmentType(s1, s2)
        )
            return false;

        _board.Move(s1, s2);
        toggleCurrentPlayer();
        _display.cancelAI = false;
        return true;
    }

    bool isModel()
    {
        return model != 0;
    }

    public static (int h1, int h2) TransformAiOut(float[] list)
    {
        int h1 = 0;
        int h2 = 0;

        for (int i = 0; i < 64; i++)
        {
            if (list[h1] < list[i])
                h1 = i;
        }

        for (int i = 0; i < 64; i++)
        {
            if (list[h2 + 64] < list[i + 64])
                h2 = i;
        }

        return (h1, h2);
    }

    void runAi()
    {
        float[] list = new float[128];

        Model.Model_Run(model, _board.getNNData(), list);

        var (h1, h2) = TransformAiOut(list);
        Console.WriteLine($"model out s1: {h1}, s2: {h2}");
    }

    private bool isAiTurn()
    {
        if (_display.cancelAI)
            return false;

        return _currentPlayer switch
        {
            PlayerType.BLACK => BlackAI,
            PlayerType.WHITE => WhiteAI,
            _ => false,
        };
    }

    void update()
    {
        _display.Update();

        if (isModel() && isAiTurn())
        {
            _display.resetSelection();

            runAi();
            toggleCurrentPlayer();
        }
        else if (_display.cellselected2 != -1)
        {
            AttemptMove(_display.cellselected1, _display.cellselected2);
            _display.resetSelection();
        }
    }

    bool IsRunning()
    {
        return (_running && _display.IsRunning);
    }

    public void GameLoop()
    {
        _running = true;

        while (IsRunning())
        {
            update();
        }
    }

    bool make_model()
    {
        model = Model.Model_Create();

        if (model == 0)
        {
            Console.WriteLine("Out of memory");
            return true;
        }

        return false;
    }

    public void Start(
        bool AIB = false,
        bool AIW = false,
        bool load = false,
        bool save = false,
        bool train = false
    )
    {
        WhiteAI = AIW;
        BlackAI = AIB;

        Console.WriteLine($"test: {WhiteAI}, {BlackAI}");
        bool AI = AIB || AIW;
        load &= AI;
        save &= AI;
        train &= AI;

        if (AI && make_model())
            return;

        _display.StartDisplay();

        Console.WriteLine("Checkers starting");

        char[] Path = "../Params.Param".ToCharArray();

        if (load)
            Model.Model_Load(model, Path, Path.Length);

        if (train)
        {
            // TODO:
            // add the nndb file path
            String PD = "";
            String PE = "";

            Model.Model_train(model, PD.ToCharArray(), PE.ToCharArray());
        }

        GameLoop();

        if (save)
            Model.Model_Save(model, Path, Path.Length);

        _display.CloseDisplay();
        Model.Model_Delete(out model);
    }
}
