using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameQueue.Core.Entities;

[Table("request_to_maps")]
public sealed record RequestMap
{
    [Required]
    [Key]
    public int Id { get; set; }

    [Required]
    public int SearchRequestId { get; set; }

    public MapSearchRequest SearchRequest { get; set; } = null!;

    [Required]
    public int MapId { get; set; }

    public Map Map { get; set; } = null!;
}
