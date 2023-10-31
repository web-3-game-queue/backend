using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GameQueue.Core.Models.MapSearchRequests.Status;
using GameQueue.Core.Models.Users;

namespace GameQueue.Core.Models.MapSearchRequests;

[Table("MapSearchRequests")]
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
