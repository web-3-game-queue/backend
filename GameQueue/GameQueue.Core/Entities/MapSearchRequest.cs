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
    public MapSearchRequestStatus Status { get; set; }

    public User CreatorUser { get; set; } = null!;

    public List<RequestToMap> RequestsToMap { get; } = new List<RequestToMap>();
}
