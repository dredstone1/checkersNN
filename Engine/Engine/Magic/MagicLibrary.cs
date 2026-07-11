using System.Runtime.CompilerServices;
using MessagePack;

namespace Engine.Magic;

public static class MagicLibrary
{
    public static MagicPieceTable KingTable { get; }

    private static readonly string _magicBasePath = Path.Combine(AppContext.BaseDirectory, "Magic");

    static MagicLibrary()
    {
#if BOARD_10
        KingTable = LoadTable("KingMagic128.msgpack");
#elif BOARD_8
        KingTable = LoadTable("KingMagic64.msgpack");
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
