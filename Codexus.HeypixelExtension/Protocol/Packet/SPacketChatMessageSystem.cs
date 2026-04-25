using Codexus.Development.SDK.Connection;
using Codexus.Development.SDK.Enums;
using Codexus.Development.SDK.Packet;
using Codexus.HeypixelExtension.Protocol.Nbt;
using Codexus.HeypixelExtension.Protocol.Nbt.Field;
using DotNetty.Buffers;

namespace Codexus.HeypixelExtension.Protocol.Packet;

[RegisterPacket(EnumConnectionState.Play, EnumPacketDirection.ClientBound, 108, [EnumProtocolVersion.V1200, EnumProtocolVersion.V1206 ])]
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