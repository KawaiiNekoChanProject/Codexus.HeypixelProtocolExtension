namespace Codexus.HeypixelExtension.Protocol.Nbt.Field;

public record NbtLongArray(long[] Members) : NbtAny(12)
{
    public NbtLongArray(int length) : this(new long[length]) { }
}