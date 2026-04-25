using Codexus.Development.SDK.Connection;
using Codexus.Development.SDK.Enums;
using Codexus.Development.SDK.Packet;
using Codexus.HeypixelExtension.protocol.nbt;
using Codexus.HeypixelExtension.protocol.nbt.field;
using DotNetty.Buffers;

namespace Codexus.HeypixelExtension.protocol.packet;

[RegisterPacket(EnumConnectionState.Play, EnumPacketDirection.ClientBound, [104, 108], [EnumProtocolVersion.V1200, EnumProtocolVersion.V1206 ])]
public class SPacketChatMessageSystem : IPacket
{
    public NbtAny Content { get; set; } = new(0);
    public bool Overlay { get; set; }
		
    public EnumProtocolVersion ClientProtocolVersion { get; set; }
		
    public void ReadFromBuffer(IByteBuffer buffer)
    {
        Content = buffer.ReadNbt();
        Overlay = buffer.ReadBoolean();
    }

    public void WriteToBuffer(IByteBuffer buffer)
    {
        try
        {
            buffer.WriteNbt(Content);
            buffer.WriteBoolean(Overlay);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public bool HandlePacket(GameConnection connection)
    {
        return false;
    }

}