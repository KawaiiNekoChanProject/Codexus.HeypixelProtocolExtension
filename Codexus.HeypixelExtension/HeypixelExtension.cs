using System.Text;
using System.Text.Json;
using Codexus.Base1200.Plugin.Event;
using Codexus.Base1200.Plugin.Packet.Play.Client.Configuration;
using Codexus.Development.SDK.Enums;
using Codexus.Development.SDK.Manager;
using Codexus.Development.SDK.Plugin;
using Codexus.HeypixelExtension.entity;
using Codexus.HeypixelExtension.Entity;
using Codexus.HeypixelExtension.Protocol.Events;
using Codexus.HeypixelExtension.Protocol.Events.Extensions;
using Codexus.HeypixelExtension.Protocol.Packet.Message;
using Codexus.HeypixelExtension.Utils;
using Serilog;

namespace Codexus.HeypixelExtension;

[Development.SDK.Attributes.Plugin(
    "00AB0E97-71E1-4F59-873B-4B9B6EF9729C",
    "Heypixel Protocol Extension",
    "Unofficial Heypixel Protocol Extension!",
    "NekoCurit",
    "0.0.2",
    [
        "36D701B3-6E98-3E92-AF53-C4EC327B3A71",
    ])
]
public class HeypixelExtension : IPlugin
{
    public void OnInitialize()
    {
        EventManager.Instance.RegisterHandler<EventPluginMessage>(Base1200.Plugin.Base1200.PluginChannel, OnPluginMessage);
        EventManager.Instance.RegisterHandler<EventPlayerSendCommand>("base_1200_extra", OnPlaySendCommand);
    }

    public static void OnPluginMessage(EventPluginMessage e)
    {
        if (e.Connection.State != EnumConnectionState.Play) return;
        if (e.Identifier != "floodgate:form" || e.Payload.Length <= 3) return;
        
        var windowId = FloodgateFormId.GetFormIdFromPayloadRaw(e.Payload);
        var raw = Encoding.UTF8.GetString(e.Payload, 3, e.Payload.Length - 3);
            
        Log.Debug("Id: {0} Detail: {1}", windowId, raw);
        switch (e.Payload[0])
        {
            case 0x00:
                var form = JsonSerializer.Deserialize<Form>(raw)!;
            
                e.SendMessageToClient(
                    MessageBuilder.Builder()
                        .Text("§7>§r " + form.Title)
                        .OnHover(OnHover.ShowText("§7容器序号: §b" + windowId))
                        .Build()
                );
                e.SendMessageToClient(form.Content);
                    
                for (var i = 0; i < form.Buttons.Count; i++)
                {
                    var button = form.Buttons[i];

                    e.SendMessageToClient(
                        MessageBuilder.Builder()
                            .Text("§7[" + button.Text.Replace("§l", "").Replace("\n", " ") + "§7]")
                            .OnHover(OnHover.ShowText("§7点击执行选项: §b" + i))
                            .OnClick(OnClick.RunCommand("/floodgate:click " + windowId + " " + i))
                            .Build()
                    );
                }
                break;
            case 0x01:
                var modal = JsonSerializer.Deserialize<Modal>(raw)!;
            
                e.SendMessageToClient(
                    MessageBuilder.Builder()
                        .Text("§7>§r " + modal.Title)
                        .OnHover(OnHover.ShowText("§7容器序号: §b" + windowId))
                        .Build()
                );
                e.SendMessageToClient(modal.Content);
                e.SendMessageToClient(
                    MessageBuilder.Builder()
                        .Text("§7[" + modal.Button1.Replace("§l", "").Replace("\n", " ") + "§7]")
                        .OnHover(OnHover.ShowText("§7点击执行选项: §a是"))
                        .OnClick(OnClick.RunCommand("/floodgate:click " + windowId + " true"))
                        .Build()
                );
                e.SendMessageToClient(
                    MessageBuilder.Builder()
                        .Text("§7[" + modal.Button2.Replace("§l", "").Replace("\n", " ") + "§7]")
                        .OnHover(OnHover.ShowText("§7点击执行选项: §a否"))
                        .OnClick(OnClick.RunCommand("/floodgate:click " + windowId + " false"))
                        .Build()
                );
                break;
        }
    }

    public static void OnPlaySendCommand(EventPlayerSendCommand e)
    {
        if (!e.Command.StartsWith("floodgate:click ")) return;
        e.Cancel();
        
        var param = e.Command.Replace("floodgate:click", "").Trim().Split(" ");
        if (param.Length != 2)
        {
            e.SendMessageToClient("§c无效的命令格式");
            return;
        }

        e.Connection.ServerChannel!.WriteAndFlushAsync(new CPacketPluginMessage
        {
            Identifier = "floodgate:form",
            Payload = FloodgateFormId.ToFormId(short.Parse(param[0]))
                .Concat(Encoding.UTF8.GetBytes(param[1]))
                .ToArray()
        });
        e.SendMessageToClient("§a执行成功");
    }

}