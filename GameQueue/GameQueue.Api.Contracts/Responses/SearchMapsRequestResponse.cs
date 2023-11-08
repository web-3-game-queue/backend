using System.Text.Json.Serialization;
using GameQueue.Api.Contracts.Models;

namespace GameQueue.Api.Contracts.Responses;

public sealed record SearchMapsRequestResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("creatorUserId")]
    public int CreatorUserId { get; set; }

    [JsonPropertyName("status")]
    public SearchMapsRequestStatusApi Status { get; set; } = SearchMapsRequestStatusApi.Draft;

    [JsonPropertyName("creationDate")]
    public DateTimeOffset CreationDate { get; set; } = DateTimeOffset.UtcNow;

    [JsonPropertyName("composeDate")]
    public DateTimeOffset? ComposeDate { get; set; } = null;

    [JsonPropertyName("doneDate")]
    public DateTimeOffset? DoneDate { get; set; } = null;

    [JsonPropertyName("mapsCount")]
    public int MapsCount { get; set; }
}
