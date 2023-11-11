using System.Text.Json.Serialization;
using GameQueue.Api.Contracts.Models;

namespace GameQueue.Api.Contracts.Responses;

public sealed record UserResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("level")]
    public int Level { get; set; }

    [JsonPropertyName("role")]
    public UserRoleApi Role { get; set; }

    [JsonPropertyName("claims")]
    public ICollection<string>? Claims { get; set; } = null;
}
