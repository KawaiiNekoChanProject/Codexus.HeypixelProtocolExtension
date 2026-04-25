using System.Text;
using Codexus.HeypixelExtension.protocol.nbt;
using Codexus.HeypixelExtension.protocol.nbt.field;
using DotNetty.Buffers;

namespace Codexus.HeypixelExtension.protocol.packet;

public static class ComponentHelper
{
    private static readonly byte[] TextPrefix = [
        0x08
    ];
    
    private static readonly byte[] TextClickPrefix =
    [
        0x0A, 0x0A, 0x00, 0x0A,
        0x63, 0x6C, 0x69, 0x63, 0x6B, 0x45, 0x76, 0x65, 0x6E, 0x74,
        0x08, 0x00, 0x06,
        0x61, 0x63, 0x74, 0x69, 0x6F, 0x6E,
        0x00, 0x0B,
        0x72, 0x75, 0x6E, 0x5F, 0x63, 0x6F, 0x6D, 0x6D, 0x61, 0x6E, 0x64,
        0x08, 0x00, 0x05,
        0x76, 0x61, 0x6C, 0x75, 0x65
    ];

    private static readonly byte[] TextClickMid =
    [
        0x00, 0x08, 0x00, 0x04, 0x74, 0x65, 0x78, 0x74
    ];
    private static readonly byte[] TextClickSuffix =
    [
        0x08, 0x00, 0x04,
        0x74, 0x79, 0x70, 0x65, 0x00, 0x04, 0x74, 0x65, 0x78, 0x74, 0x00
    ];
    
    public static NbtObject Text(string message)
    {
        var byteMessage = Encoding.UTF8.GetBytes(message);

        var buffer = Unpooled.Buffer();
        
        buffer.WriteBytes(TextPrefix);
        buffer.WriteShort(byteMessage.Length);
        buffer.WriteBytes(byteMessage);
        
        return (NbtObject) buffer.ReadNbt();
    }
    
    public static NbtObject TextClick(string message, string command)
    {
        var byteCommand = Encoding.UTF8.GetBytes(command);
        var byteMessage = Encoding.UTF8.GetBytes(message);

        var buffer = Unpooled.Buffer();
        
        buffer.WriteBytes(TextClickPrefix);
        buffer.WriteShort(byteCommand.Length);
        buffer.WriteBytes(byteCommand);
        buffer.WriteBytes(TextClickMid);
        buffer.WriteShort(byteMessage.Length);
        buffer.WriteBytes(byteMessage);
        buffer.WriteBytes(TextClickSuffix);

        return (NbtObject) buffer.ReadNbt();
    }
    
    private static byte[] ToArray(this IByteBuffer buffer)
    {
        var result = new byte[buffer.ReadableBytes];
        buffer.ReadBytes(result);
        return result;
    }
    
}