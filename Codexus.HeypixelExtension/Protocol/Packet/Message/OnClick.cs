using Codexus.HeypixelExtension.Protocol.Nbt.Field;

namespace Codexus.HeypixelExtension.Protocol.Packet.Message;

public record OnClick(string Action, string Value)
{
    public NbtObject ToNbt()
    {
        return new NbtObject
        {
            ["action"] = new NbtElement(8, Action),
            ["value"]  = new NbtElement(8, Value)
        };
    }
    
    public static OnClick RunCommand(string command)
    {
        return new OnClick("run_command", command);
    }
    
    public static OnClick SuggestCommand(string command)
    {
        return new OnClick("suggest_command", command);
    }
    
    public static OnClick OpenUrl(string command)
    {
        return new OnClick("open_url", command);
    }
      
    public static OnClick CopyToClipboard(string command)
    {
        return new OnClick("copy_to_clipboard", command);
    }  
}