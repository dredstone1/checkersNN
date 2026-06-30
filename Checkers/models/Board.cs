namespace Checkers.models;

public enum cell
{
    EMPTY_C,
    BLACK_C,
    WHITE_C,
}

public class Board
{
    cell[] board = new cell[64];

    public Board() { }

    public cell getCell(int i)
    {
        return board[i];
    }

    public void setCell(int i, cell c)
    {
        board[i] = c;
    }

    public void resetBoard()
    {
        clearBoard();

        resetBoard(0);
        resetBoard(5);
    }

    public void resetBoard(int offset)
    {
        for (int i = offset; i < 3 * 8; ++i)
        {
            if ((i + i / 8) % 2 == 0)
            {
                setCell(i, cell.BLACK_C);
            }
        }
    }

    void clearBoard()
    {
        for (int i = 0; i < 64; ++i)
        {
            board[i] = cell.EMPTY_C;
        }
    }
}
