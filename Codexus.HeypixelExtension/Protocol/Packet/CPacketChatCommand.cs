using Codexus.Development.SDK.Connection;
using Codexus.Development.SDK.Enums;
using Codexus.Development.SDK.Extensions;
using Codexus.Development.SDK.Manager;
using Codexus.Development.SDK.Packet;
using Codexus.HeypixelExtension.Protocol.Events;
using DotNetty.Buffers;

namespace Codexus.HeypixelExtension.Protocol.Packet;

[RegisterPacket(EnumConnectionState.Play, EnumPacketDirection.ServerBound, 4, [EnumProtocolVersion.V1200, EnumProtocolVersion.V1206 ])]
public class CPacketChatCommand : IPacket
{
    private string Command { get; set; } = "";
		
    public EnumProtocolVersion ClientProtocolVersion { get; set; }
		
    public void ReadFromBuffer(IByteBuffer buffer)
    {
        Command = buffer.ReadStringFromBuffer(32767);
    }

    public void WriteToBuffer(IByteBuffer buffer)
    {
        buffer.WriteStringToBuffer(Command);
    }

    public bool HandlePacket(GameConnection connection)
    {
        var e = new EventPlayerSendCommand(connection)
        {
            Command = Command
        };
        EventManager.Instance.TriggerEvent("base_1200_extra", e);
        Command = e.Command;
        
        return e.IsCancelled || Command.Length == 0;
    }

}