using System.Text.Json.Serialization;
using GameQueue.Backend.Api.Contracts.Models;

namespace GameQueue.Backend.Api.Contracts.Responses;

public sealed record SearchMapsRequestResponse
{
    public int Id { get; set; }

    public int CreatorUserId { get; set; }

    public SearchMapsRequestStatusApi Status { get; set; } = SearchMapsRequestStatusApi.Draft;

    public DateTimeOffset CreationDate { get; set; } = DateTimeOffset.UtcNow;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public UserResponse? CreatorUser { get; set; } = null;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<MapResponse>? Maps { get; set; } = null;
}
