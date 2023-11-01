using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GameQueue.Core.Entities.MapSearchRequests.Status;

namespace GameQueue.Core.Entities;

[Table("map_search_requests")]
public sealed record MapSearchRequest
{
    [Required]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int CreatorUserId { get; set; }

    [Required]
    public MapSearchRequestStatus Status { get; set; } = MapSearchRequestStatus.Draft;

    [Required]
    public DateTime CreationDate { get; set; } = DateTime.Now;

    public User CreatorUser { get; set; } = null!;

    public List<RequestToMap> RequestsToMap { get; } = new List<RequestToMap>();
}
