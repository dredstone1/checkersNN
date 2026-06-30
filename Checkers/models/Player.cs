namespace Checkers.models;

public enum PlayerType
{
    BLACK,
    WHITE,
}

public interface IPlayer
{
    PlayerType Type { get; }
}

public class Players
{
    public IPlayer Black { get; }
    public IPlayer White { get; }

    public Players(IPlayer black, IPlayer white)
    {
        Black = black;
        White = white;
    }
}
