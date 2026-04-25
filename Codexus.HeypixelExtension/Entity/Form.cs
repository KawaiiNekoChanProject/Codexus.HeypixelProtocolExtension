using System.Text.Json.Serialization;

namespace Codexus.HeypixelExtension.entity;

public class Form
{
    [JsonPropertyName("title")]
    public required string Title { get; init; }

    [JsonPropertyName("content")]
    public required string Content { get; init; }

    [JsonPropertyName("buttons")]
    public required List<Button> Buttons { get; init; }

    [JsonPropertyName("type")]
    public required string Type { get; init; }

    public class Button
    {
        [JsonPropertyName("text")]
        public required string Text { get; init; }
    }
}