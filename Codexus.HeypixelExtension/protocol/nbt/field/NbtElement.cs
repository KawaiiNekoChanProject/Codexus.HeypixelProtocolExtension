namespace Codexus.HeypixelExtension.protocol.nbt.field;

public record NbtElement(byte Type, object Value): NbtAny(Type);