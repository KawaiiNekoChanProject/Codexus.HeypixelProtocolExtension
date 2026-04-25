namespace Codexus.HeypixelExtension.Protocol.Nbt.Field;

public record NbtObject(): NbtAny(10)
{
    public readonly SortedDictionary<string, NbtAny> Data = new();
    
    public NbtAny this[string key]
    {
        get => Data[key];
        set => Data[key] = value;
    }
}