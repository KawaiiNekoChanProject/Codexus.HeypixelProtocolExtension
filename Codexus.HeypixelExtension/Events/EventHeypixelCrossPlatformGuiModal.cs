using System.Text;
using Codexus.Base1200.Plugin.Packet.Play.Client;
using Codexus.Development.SDK.Connection;
using Codexus.Development.SDK.Manager;
using Codexus.HeypixelExtension.Entity;
using Codexus.HeypixelExtension.Protocol.Events.Extensions;
using Codexus.HeypixelExtension.Protocol.Packet.Message;
using Codexus.HeypixelExtension.Utils;

namespace Codexus.HeypixelExtension.Events;

public class EventHeypixelCrossPlatformGuiModal(GameConnection connection, short windowId, Modal modal) : EventArgsBase(connection)
{
    public readonly short WindowId = windowId;
    public readonly Modal Modal = modal;
    
    public bool Clicked;

    public void Click(bool state)
    {
        if (Clicked) throw new InvalidOperationException("Already Clicked");
        
        Clicked = true;
        Connection.ServerChannel!.WriteAndFlushAsync(new CPacketPluginMessage
        {
            Identifier = "floodgate:form",
            Payload = FloodgateFormId.ToFormId(WindowId)
                .Concat(Encoding.UTF8.GetBytes(state ? "true" : "false"))
                .ToArray()
        });
    }

    public void LetPlayerChoice()
    {
        this.SendMessageToClient(
            MessageBuilder.Builder()
                .Text("§7>§r " + Modal.Title)
                .OnHover(OnHover.ShowText("§7容器序号: §b" + WindowId))
                .Build()
        );
        this.SendMessageToClient(Modal.Content);
        this.SendMessageToClient(
            MessageBuilder.Builder()
                .Text("§7[" + Modal.Button1.Replace("§l", "").Replace("\n", " ") + "§7]")
                .OnHover(OnHover.ShowText("§7点击执行选项: §a是"))
                .OnClick(OnClick.RunCommand("/floodgate:click " + WindowId + " true"))
                .Build()
        );
        this.SendMessageToClient(
            MessageBuilder.Builder()
                .Text("§7[" + Modal.Button2.Replace("§l", "").Replace("\n", " ") + "§7]")
                .OnHover(OnHover.ShowText("§7点击执行选项: §a否"))
                .OnClick(OnClick.RunCommand("/floodgate:click " + WindowId + " false"))
                .Build()
        );
    }
}