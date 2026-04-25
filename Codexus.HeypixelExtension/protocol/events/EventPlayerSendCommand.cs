using Codexus.Development.SDK.Connection;
using Codexus.Development.SDK.Manager;

namespace Codexus.HeypixelExtension.protocol.events;

public class EventPlayerSendCommand(GameConnection connection) : EventArgsBase(connection)
{
    public string Command = "";

    public void Cancel()
    {
        Command = "";
    }
}