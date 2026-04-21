using Codexus.Development.SDK.Connection;
using Codexus.Development.SDK.Enums;
using Codexus.Development.SDK.Packet;
using DotNetty.Buffers;

namespace Codexus.HeypixelExtension.packet;

[RegisterPacket(EnumConnectionState.Play, EnumPacketDirection.ClientBound, [104, 108], [EnumProtocolVersion.V1200, EnumProtocolVersion.V1206 ])]
public class SPacketChatMessageSystem : IPacket
{
    public byte[] Content { get; set; } = [];
    public bool Overlay { get; set; }
		
    public EnumProtocolVersion ClientProtocolVersion { get; set; }
		
    public void ReadFromBuffer(IByteBuffer buffer)
    {
        Content = new byte[buffer.ReadableBytes - 1];
        buffer.ReadBytes(Content);
        Overlay = buffer.ReadBoolean();
    }

    public void WriteToBuffer(IByteBuffer buffer)
    {
        buffer.WriteBytes(Content);
        buffer.WriteBoolean(Overlay);
    }

    public bool HandlePacket(GameConnection connection)
    {
        return false;
    }

}