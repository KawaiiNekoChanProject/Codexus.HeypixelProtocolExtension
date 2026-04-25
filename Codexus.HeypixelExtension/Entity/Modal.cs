using System.Text.Json.Serialization;

namespace Codexus.HeypixelExtension.Entity;

public class Modal
{
    [JsonPropertyName("title")]
    public required string Title { get; init; }

    [JsonPropertyName("content")]
    public required string Content { get; init; }
    
    [JsonPropertyName("button1")]
    public required string Button1 { get; init; }

    [JsonPropertyName("button2")]
    public required string Button2 { get; init; }
   
    [JsonPropertyName("type")]
    public required string Type { get; init; }
}