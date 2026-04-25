namespace Codexus.HeypixelExtension.protocol.nbt.field;

public record NbtList(byte MemberType, NbtAny[] Members) : NbtAny(9)
{
    public NbtList(byte type, int length) : this(type, new NbtAny[length]) { }
}