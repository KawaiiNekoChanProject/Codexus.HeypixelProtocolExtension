namespace Codexus.HeypixelExtension.protocol.nbt.field;

public record NbtObject(): NbtAny(10)
{
    public readonly SortedDictionary<string, NbtAny> Data = new();
}