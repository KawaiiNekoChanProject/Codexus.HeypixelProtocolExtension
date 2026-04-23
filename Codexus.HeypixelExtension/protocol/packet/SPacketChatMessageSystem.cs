using Codexus.Development.SDK.Connection;
using Codexus.Development.SDK.Enums;
using Codexus.Development.SDK.Packet;
using Codexus.HeypixelExtension.protocol.nbt;
using DotNetty.Buffers;

namespace Codexus.HeypixelExtension.protocol.packet;

[RegisterPacket(EnumConnectionState.Play, EnumPacketDirection.ClientBound, [104, 108], [EnumProtocolVersion.V1200, EnumProtocolVersion.V1206 ], true)]
public class SPacketChatMessageSystem : IPacket
{
    public Nbt Content { get; set; } = new();
    public bool Overlay { get; set; }
		
    public EnumProtocolVersion ClientProtocolVersion { get; set; }
		
    public void ReadFromBuffer(IByteBuffer buffer)
    {
        // TODO: 修复 NBT 无法解析问题
        Content = buffer.ReadNbt();
        Overlay = buffer.ReadBoolean();
    }

    public void WriteToBuffer(IByteBuffer buffer)
    {
        buffer.WriteNbt(Content);
        buffer.WriteBoolean(Overlay);
    }

    public bool HandlePacket(GameConnection connection)
    {
        return false;
    }

}