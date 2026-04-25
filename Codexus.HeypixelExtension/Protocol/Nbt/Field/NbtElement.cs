namespace Codexus.HeypixelExtension.Protocol.Nbt.Field;

public record NbtElement(byte Type, object Value): NbtAny(Type);