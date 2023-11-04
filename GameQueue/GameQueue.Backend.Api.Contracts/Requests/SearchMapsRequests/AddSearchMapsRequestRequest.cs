using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GameQueue.Api.Contracts.Requests.SearchMapsRequests;

public sealed record AddSearchMapsRequestRequest
{
    [Required]
    public int CreatorUserId { get; set; }

    [Required]
    public int MapId { get; set; }

    [JsonIgnore]
    public DateTimeOffset CreationDate { get; set; } = DateTimeOffset.UtcNow;
}
