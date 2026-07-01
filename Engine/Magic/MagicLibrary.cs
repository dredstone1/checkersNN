using System.Runtime.CompilerServices;
using MessagePack;

namespace Engine.Magic;

public static class MagicLibrary
{
    public static MagicPieceTable KingTable { get; }
    public static MagicPieceTable KingCaptureTable { get; }

    private static readonly string _magicBasePath = Path.Combine(
        AppContext.BaseDirectory,
        "MagicTables"
    );

    static MagicLibrary()
    {
#if BOARD_10
        KingTable = LoadTable("KingMagic128.msgpack");
        KingCaptureTable = LoadTable("KingCaptureMagic128.msgpack");
#elif BOARD_8
        KingTable = LoadTable("KingMagic64.msgpack");
        KingCaptureTable = LoadTable("KingCaptureMagic64.msgpack");
#endif
    }

    private static MagicPieceTable LoadTable(string fileName)
    {
        byte[] bytes = File.ReadAllBytes(Path.Combine(_magicBasePath, fileName));
        return MessagePackSerializer.Deserialize<MagicPieceTable>(bytes);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoardBits GetAttacks(MagicPieceTable table, int square, BoardBits occupancy)
    {
        BoardBits blockers = occupancy & table.Masks[square];
        BoardBits index = (blockers * table.MagicNumbers[square]) >> table.Shifts[square];
        return table.AttackTable[square][(int)index];
    }
}
