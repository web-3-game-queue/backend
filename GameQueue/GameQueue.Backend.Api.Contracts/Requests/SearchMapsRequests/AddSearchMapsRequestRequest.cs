using System.ComponentModel.DataAnnotations;

namespace GameQueue.Core.Backend.Api.Contracts.Requests.Maps;

public sealed record AddSearchMapsRequestRequest
{
    [Required]
    public int SearchMapsRequestId { get; set; }

    [Required]
    public int CreatorUserId { get; set; }

    [Required]
    public int MapId { get; set; }

    [Required]
    public DateTime CreationDate { get; set; } = DateTime.Now;
}
