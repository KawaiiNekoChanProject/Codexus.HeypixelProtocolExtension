using System.Text;
using Codexus.Base1200.Plugin.Packet.Play.Client;
using Codexus.Development.SDK.Connection;
using Codexus.Development.SDK.Enums;
using Codexus.Development.SDK.Extensions;
using Codexus.Development.SDK.Packet;
using Codexus.HeypixelExtension.utils;
using DotNetty.Buffers;

namespace Codexus.HeypixelExtension.protocol.packet;

[RegisterPacket(EnumConnectionState.Play, EnumPacketDirection.ServerBound, [13, 4], [EnumProtocolVersion.V1200, EnumProtocolVersion.V1206 ])]
public class CPacketChatMessageSystem : IPacket
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
        if (!Command.StartsWith("floodgate:click ")) return false;

        var param = Command.Replace("floodgate:click", "").Trim().Split(" ");
        if (param.Length != 2) return false;
        
        connection.ClientChannel.WriteAndFlushAsync(new SPacketChatMessageSystem
        {
            Content = ComponentHelper.Text("§a执行成功"),
            Overlay = false
        });
        connection.ServerChannel!.WriteAndFlushAsync(new CPacketPluginMessage
        {
            Identifier = "floodgate:form",
            Payload = FloodgateFormId.ToFormId(short.Parse(param[0]))
                .Concat(Encoding.UTF8.GetBytes(param[1]))
                .ToArray()
        });
        return true;
    }

}