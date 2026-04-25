using Codexus.Development.SDK.Manager;
using Codexus.HeypixelExtension.protocol.nbt.field;
using Codexus.HeypixelExtension.protocol.packet;

namespace Codexus.HeypixelExtension.protocol.events.extensions;

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