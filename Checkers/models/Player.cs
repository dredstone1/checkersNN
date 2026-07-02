namespace Checkers.models;


public interface IPlayer
{
    PlayerType Type { get; }
}

public record Players(IPlayer Black, IPlayer White);
