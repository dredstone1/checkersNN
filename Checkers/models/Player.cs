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

public record Players(IPlayer Black, IPlayer White);
