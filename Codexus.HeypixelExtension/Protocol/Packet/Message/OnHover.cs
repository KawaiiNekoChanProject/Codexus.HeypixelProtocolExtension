using Codexus.HeypixelExtension.Protocol.Nbt.Field;

namespace Codexus.HeypixelExtension.Protocol.Packet.Message;

public record OnHover(string Action, NbtAny Value)
{
    public NbtObject ToNbt()
    {
        return new NbtObject
        {
            ["action"] = new NbtElement(8, Action),
            ["contents"] = Value
        };
    }
    
    public static OnHover ShowText(string text)
    {
        return new OnHover("show_text", new NbtObject
        {
            ["text"] = new NbtElement(8, text),
            ["type"] = new NbtElement(8, "text")
        });
    }
}