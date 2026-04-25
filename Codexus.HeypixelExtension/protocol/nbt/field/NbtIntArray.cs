namespace Codexus.HeypixelExtension.protocol.nbt.field;

public record NbtIntArray(int[] Members) : NbtAny(11)
{
    public NbtIntArray(int length) : this(new int[length]) { }
}