using Codexus.HeypixelExtension.protocol.nbt.field;

namespace Codexus.HeypixelExtension.protocol.packet.helper;

public class MessageBuilder
{

    public static MessageBuilder Builder()
    {
        return new MessageBuilder();
    }
    
    private readonly NbtObject _obj = new();
    
    public MessageBuilder Text(string message)
    {
        _obj["text"] = new NbtElement(8, message);
        _obj["type"] = new NbtElement(8, "text");
        return this;
    }

    public MessageBuilder OnClick(OnClick data)
    {
        _obj["clickEvent"] = data.ToNbt();
        return this;
    }

    public MessageBuilder OnHover(OnHover data)
    {
        _obj["hoverEvent"] = data.ToNbt();
        return this;
    }

    public NbtObject Build()
    {
        if (!_obj.Data.ContainsKey("text")) throw new InvalidOperationException("Message must have text");
        
        return _obj;
    }
}