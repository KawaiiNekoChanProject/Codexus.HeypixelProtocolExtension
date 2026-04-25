using Codexus.Development.SDK.Manager;
using Codexus.HeypixelExtension.Protocol.Nbt.Field;
using Codexus.HeypixelExtension.Protocol.Packet;

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
}