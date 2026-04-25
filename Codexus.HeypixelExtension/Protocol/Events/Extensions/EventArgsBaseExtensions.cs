using Codexus.Development.SDK.Manager;
using Codexus.HeypixelExtension.Protocol.Nbt.Field;
using Codexus.HeypixelExtension.Protocol.Packet;
using Codexus.HeypixelExtension.Protocol.Packet.Message;

namespace Codexus.HeypixelExtension.Protocol.Events.Extensions;

public static class EventArgsBaseExtensions
{
    public static void SendMessageToClient(this EventArgsBase data, NbtAny message)
    {
        data.Connection.ClientChannel.WriteAndFlushAsync(new SPacketChatMessageSystem
        {
            Content = message,
            Overlay = false
        });
    }
    public static void SendMessageToClient(this EventArgsBase data, string message)
    {
        data.SendMessageToClient(MessageBuilder.Builder().Text(message).Build());
    }
}