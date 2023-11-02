using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameQueue.Core.Models;

[Table("search_maps_requests")]
public sealed record SearchMapsRequest
{
    [Required]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int CreatorUserId { get; set; }

    [Required]
    public SearchMapsRequestStatus Status { get; set; } = SearchMapsRequestStatus.Draft;

    [Required]
    public DateTimeOffset CreationDate { get; set; } = DateTimeOffset.UtcNow;

    public User CreatorUser { get; set; } = null!;

    public List<RequestToMap> RequestsToMap { get; } = new List<RequestToMap>();
}
