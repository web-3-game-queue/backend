using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GameQueue.Core.Entities.MapSearchRequests.Status;

namespace GameQueue.Core.Entities;

[Table("map_search_requests")]
public sealed record MapSearchRequest
{
    [Required]
    [Key]
    public int Id { get; set; }

    [Required]
    public int CreatorUserId { get; set; }

    [Required]
    public User CreatorUser { get; set; } = null!;

    [Required]
    public MapSearchRequestStatus Status { get; set; }
}
