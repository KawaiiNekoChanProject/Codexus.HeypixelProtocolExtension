using Codexus.HeypixelExtension.protocol.nbt.field;

namespace Codexus.HeypixelExtension.protocol.packet;

public static class ComponentHelper
{
    public static NbtElement Text(string message)
    {
        return new NbtElement(8, message);
    }
    
    public static NbtObject TextClick(string message, string hover, string command)
    {
        return new NbtObject
        {
            ["clickEvent"] = new NbtObject
            {
                ["action"] = new NbtElement(8, "run_command"),
                ["value"]  = new NbtElement(8, command)
            },
            ["hoverEvent"] = new NbtObject
            {
                ["action"] = new NbtElement(8, "show_text"),
                ["contents"] = new NbtObject
                {
                    ["text"] = new NbtElement(8, hover),
                    ["type"] = new NbtElement(8, "text")
                }
            },
            ["text"] = new NbtElement(8, message),
            ["type"] = new NbtElement(8, "text")
        };
    }
}