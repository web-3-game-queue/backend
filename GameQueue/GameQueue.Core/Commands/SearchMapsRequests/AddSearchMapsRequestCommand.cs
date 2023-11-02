using System.ComponentModel.DataAnnotations;

namespace GameQueue.Core.Commands.SearchMapsRequests;

public sealed record AddSearchMapsRequestCommand
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
