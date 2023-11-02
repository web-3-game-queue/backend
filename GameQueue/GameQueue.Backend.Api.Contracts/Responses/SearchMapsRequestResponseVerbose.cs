using System.Text.Json.Serialization;
using GameQueue.Backend.Api.Contracts.Models;

namespace GameQueue.Backend.Api.Contracts.Responses;

public sealed record SearchMapsRequestResponseVerbose
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("creatorUserId")]
    public int CreatorUserId { get; set; }

    [JsonPropertyName("status")]
    public SearchMapsRequestStatusApi Status { get; set; } = SearchMapsRequestStatusApi.Draft;

    [JsonPropertyName("creationDate")]
    public DateTimeOffset CreationDate { get; set; } = DateTimeOffset.UtcNow;

    [JsonPropertyName("creatorUser")]
    public UserResponse CreatorUser { get; set; } = null!;

    [JsonPropertyName("maps")]
    public List<MapResponse> Maps { get; set; } = null!;
}
