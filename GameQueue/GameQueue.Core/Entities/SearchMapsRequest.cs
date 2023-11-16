using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GameQueue.Core.Models;

namespace GameQueue.Core.Entities;

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

    public DateTimeOffset? ComposeDate { get; set; } = null;

    public DateTimeOffset? DoneDate { get; set; } = null;

    public int? HandledByUserId { get; set; } = null;

    public User CreatorUser { get; set; } = null!;

    public User? HandledByUser { get; set; } = null;

    public List<RequestToMap> RequestsToMap { get; } = new List<RequestToMap>();
}
