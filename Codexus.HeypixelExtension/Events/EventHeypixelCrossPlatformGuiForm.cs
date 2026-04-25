using System.Text;
using Codexus.Base1200.Plugin.Packet.Play.Client.Configuration;
using Codexus.Development.SDK.Connection;
using Codexus.Development.SDK.Manager;
using Codexus.HeypixelExtension.Entity;
using Codexus.HeypixelExtension.Protocol.Events.Extensions;
using Codexus.HeypixelExtension.Protocol.Packet.Message;
using Codexus.HeypixelExtension.Utils;

namespace Codexus.HeypixelExtension.Events;

public class EventHeypixelCrossPlatformGuiForm(GameConnection connection, short windowId, Form form) : EventArgsBase(connection)
{
    public readonly short WindowId = windowId;
    public readonly Form Form = form;
    
    public bool Clicked;

    public void Click(int id)
    {
        if (Clicked) throw new InvalidOperationException("Already Clicked");
        
        Clicked = true;
        Connection.ServerChannel!.WriteAndFlushAsync(new CPacketPluginMessage
        {
            Identifier = "floodgate:form",
            Payload = FloodgateFormId.ToFormId(WindowId)
                .Concat(Encoding.UTF8.GetBytes(id.ToString()))
                .ToArray()
        });
    }

    public void LetPlayerChoice()
    {
        this.SendMessageToClient(
            MessageBuilder.Builder()
                .Text("§7>§r " + Form.Title)
                .OnHover(OnHover.ShowText("§7容器序号: §b" + WindowId))
                .Build()
        );
        this.SendMessageToClient(Form.Content);
                    
        for (var i = 0; i < Form.Buttons.Count; i++)
        {
            var button = Form.Buttons[i];

            this.SendMessageToClient(
                MessageBuilder.Builder()
                    .Text("§7[" + button.Text.Replace("§l", "").Replace("\n", " ") + "§7]")
                    .OnHover(OnHover.ShowText("§7点击执行选项: §b" + i))
                    .OnClick(OnClick.RunCommand("/floodgate:click " + WindowId + " " + i))
                    .Build()
            );
        }
    }
}