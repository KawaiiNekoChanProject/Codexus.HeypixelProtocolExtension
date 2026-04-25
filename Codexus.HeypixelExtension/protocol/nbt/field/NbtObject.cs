namespace Codexus.HeypixelExtension.protocol.nbt.field;

public record NbtObject(): NbtAny(10)
{
    public readonly SortedDictionary<string, NbtAny> Data = new();
    
    public NbtAny this[string key]
    {
        get => Data[key];
        set => Data[key] = value;
    }
}