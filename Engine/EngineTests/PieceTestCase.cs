using Bogus;
using Engine;
using Engine.Models;

namespace EngineTests;

public class PieceTestCase
{
    public required Piece Piece { get; init; }
    public required byte Origin { get; init; }
    public bool IsWhiteToMove { get; set; }

    public List<Move> ExpectedMoves { get; init; } = [];
    public Dictionary<int, Piece> BlockedBy { get; } = [];

    public string TestDecription { get; set; } = "";

    private readonly Faker _faker = new();

    public static PieceTestCase From(string from, Piece piece)
    {
        var origin = AlgebraicToIdx(from);
        PieceTestCase testCase = new()
        {
            Piece = piece,
            Origin = origin,
            IsWhiteToMove = piece.Color is PieceColor.White,
        };

        return testCase;
    }

    public PieceTestCase GoesTo(
        string to,
        IEnumerable<string>? captures = null,
        bool isPromotion = false
    )
    {
        Move move = BuildMove(Origin, to, Piece, captures, isPromotion);
        ExpectedMoves.Add(move);

        return this;
    }

    public PieceTestCase GoesTo(params string[] to)
    {
        foreach (var position in to)
        {
            GoesTo(position);
        }
        return this;
    }

    public PieceTestCase WithPieceAt(string position, Piece piece)
    {
        int idx = AlgebraicToIdx(position);
        BlockedBy.Add(idx, piece);
        return this;
    }

    public PieceTestCase WithWhitePieceAt(string position, PieceType? pieceType = null) =>
        WithPieceAt(
            position,
            new Piece()
            {
                Type = pieceType ?? _faker.PickRandom<PieceType>(),
                Color = PieceColor.White,
            }
        );

    public PieceTestCase WithBlackPieceAt(string position, PieceType? pieceType = null) =>
        WithPieceAt(
            position,
            new Piece()
            {
                Type = pieceType ?? _faker.PickRandom<PieceType>(),
                Color = PieceColor.Black,
            }
        );

    public PieceTestCase WithFriendlyPieceAt(string position) =>
        WithPieceAt(
            position,
            new Piece() { Type = _faker.PickRandom<PieceType>(), Color = Piece.Color }
        );

    public PieceTestCase WithEnemyPieceAt(string position) =>
        WithPieceAt(
            position,
            new Piece() { Type = _faker.PickRandom<PieceType>(), Color = Piece.Color.Invert() }
        );

    public PieceTestCase WithWhiteToMove(bool isWhiteToMove)
    {
        IsWhiteToMove = isWhiteToMove;
        return this;
    }

    public PieceTestCase WithDescription(string testDescription)
    {
        TestDecription = testDescription;
        return this;
    }

    public PieceTestCase ForEach<T>(IEnumerable<T> items, Action<T, PieceTestCase> action)
    {
        foreach (var item in items)
        {
            action(item, this);
        }
        return this;
    }

    public override string ToString() =>
        string.IsNullOrWhiteSpace(TestDecription)
            ? $"Piece under test at {Origin}"
            : TestDecription;

    private static Move BuildMove(
        byte from,
        string to,
        Piece piece,
        IEnumerable<string>? captures,
        bool? isPromotion
    )
    {
        BoardBits captureMask = 0;
        foreach (string capture in captures ?? [])
        {
            captureMask |= 1ul << AlgebraicToIdx(capture);
        }

        return new()
        {
            From = from,
            To = AlgebraicToIdx(to),
            Piece = piece,
            CapturesMask = captureMask,
            IsPromotion = isPromotion ?? false,
        };
    }

    private static byte AlgebraicToIdx(string algebraic)
    {
        int x = algebraic[0] - 'a';
        int y = int.Parse(algebraic[1..]) - 1;
        return (byte)(y * Constants.BoardSize + x);
    }
}
