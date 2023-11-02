using System.Text.Json.Serialization;

namespace GameQueue.Backend.Api.Contracts.Responses;

public sealed record UserResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("level")]
    public int Level { get; set; }
}
