using System.Text;
using System.Text.Json;
using Codexus.Base1200.Plugin.Event;
using Codexus.Development.SDK.Connection;
using Codexus.Development.SDK.Enums;
using Codexus.Development.SDK.Manager;
using Codexus.Development.SDK.Plugin;
using Codexus.HeypixelExtension.entity;
using Codexus.HeypixelExtension.protocol.packet;
using Codexus.HeypixelExtension.utils;
using Serilog;

namespace Codexus.HeypixelExtension;

[Development.SDK.Attributes.Plugin(
    "00AB0E97-71E1-4F59-873B-4B9B6EF9729C",
    "Heypixel Protocol Extension",
    "Unofficial Heypixel Protocol Extension!",
    "NekoCurit",
    "0.0.1",
    [
        "36D701B3-6E98-3E92-AF53-C4EC327B3A71",
    ])
]
public class HeypixelExtension : IPlugin
{
    public void OnInitialize()
    {
        EventManager.Instance.RegisterHandler<EventPluginMessage>(
            Base1200.Plugin.Base1200.PluginChannel,
            context => HandlePluginMessageGeneric(context.Identifier, context.Payload, context.Connection));
    }

    private static void HandlePluginMessageGeneric(string identifier, byte[] payload, GameConnection connection)
    {
        if (connection.State != EnumConnectionState.Play) return;
        
        if (identifier == "floodgate:form" && payload.Length > 3)
        {
            var raw = Encoding.UTF8.GetString(payload, 3, payload.Length - 3);
            
            var windowRaw = new byte[2];
            windowRaw[0] = payload[1];
            windowRaw[1] = payload[2];
            var windowId = FloodgateFormId.GetFormId(windowRaw);
            
            Log.Debug("Id: {0} Detail: {1}", windowId, raw);
            if (payload[0] == 0x00)
            {
                var form = JsonSerializer.Deserialize<Form>(raw)!;
            
                connection.ClientChannel.WriteAndFlushAsync(new SPacketChatMessageSystem
                {
                    Content = ComponentHelper.Text("§7>§r " + form.Title),
                    Overlay = false
                });
                connection.ClientChannel.WriteAndFlushAsync(new SPacketChatMessageSystem
                {
                    Content = ComponentHelper.Text(form.Content),
                    Overlay = false
                });
            

                for (var i = 0; i < form.Buttons.Count; i++)
                {
                    var button = form.Buttons[i];

                    connection.ClientChannel.WriteAndFlushAsync(new SPacketChatMessageSystem
                    {
                        Content = ComponentHelper.TextClick(
                            "§7[" + button.Text.Replace("§l", "").Replace("\n", " ") + "§7]", 
                            "/floodgate:click " + windowId + " " + i
                        ),
                        Overlay = false
                    });
                }
            }
            if (payload[0] == 0x01)
            {
                var modal = JsonSerializer.Deserialize<Modal>(raw)!;
            
                connection.ClientChannel.WriteAndFlushAsync(new SPacketChatMessageSystem
                {
                    Content = ComponentHelper.Text("§7>§r " + modal.Title),
                    Overlay = false
                });
                connection.ClientChannel.WriteAndFlushAsync(new SPacketChatMessageSystem
                {
                    Content = ComponentHelper.Text(modal.Content),
                    Overlay = false
                });

                connection.ClientChannel.WriteAndFlushAsync(new SPacketChatMessageSystem
                {
                    Content = ComponentHelper.TextClick(
                        "§7[" + modal.Button1.Replace("§l", "").Replace("\n", " ") + "§7]", 
                        "/floodgate:click " + windowId + " true"
                    ),
                    Overlay = false
                });
                
                connection.ClientChannel.WriteAndFlushAsync(new SPacketChatMessageSystem
                {
                    Content = ComponentHelper.TextClick(
                        "§7[" + modal.Button2.Replace("§l", "").Replace("\n", " ") + "§7]", 
                        "/floodgate:click " + windowId + " false"
                    ),
                    Overlay = false
                });
            }

        }
    }

}