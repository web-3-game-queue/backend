using System.Text.Json.Serialization;
using GameQueue.Api.Contracts.Models;

namespace GameQueue.Api.Contracts.Responses;

public sealed record SearchMapsRequestResponseVerbose
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("creatorUser")]
    public UserResponse CreatorUser { get; set; } = null!;

    [JsonPropertyName("status")]
    public SearchMapsRequestStatusApi Status { get; set; } = SearchMapsRequestStatusApi.Draft;

    [JsonPropertyName("creationDate")]
    public DateTimeOffset CreationDate { get; set; } = DateTimeOffset.UtcNow;

    [JsonPropertyName("composeDate")]
    public DateTimeOffset? ComposeDate { get; set; } = null;

    [JsonPropertyName("doneDate")]
    public DateTimeOffset? DoneDate { get; set; } = null;

    [JsonPropertyName("handledByUserId")]
    public int? HandeldByUserId { get; set; }

    [JsonPropertyName("maps")]
    public List<MapResponse> Maps { get; set; } = null!;
}
