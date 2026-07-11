using MessagePack;

namespace Engine.Magic;

[MessagePackObject]
public class MagicPieceTable
{
    [Key(0)]
    public required BoardBits[] Masks { get; set; }

    [Key(1)]
    public required BoardBits[] MagicNumbers { get; set; }

    [Key(2)]
    public required int[] Shifts { get; set; }

    [Key(3)]
    public required BoardBits[][] AttackTable { get; set; }
}
