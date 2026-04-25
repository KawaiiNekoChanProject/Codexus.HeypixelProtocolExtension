namespace Codexus.HeypixelExtension.Protocol.Nbt.Field;

public record NbtIntArray(int[] Members) : NbtAny(11)
{
    public NbtIntArray(int length) : this(new int[length]) { }
}