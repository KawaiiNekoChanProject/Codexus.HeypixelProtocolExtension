using Codexus.Development.SDK.Connection;
using Codexus.Development.SDK.Manager;

namespace Codexus.HeypixelExtension.Protocol.Events;

public class EventPlayerSendCommand(GameConnection connection) : EventArgsBase(connection)
{
    public string Command = "";
}