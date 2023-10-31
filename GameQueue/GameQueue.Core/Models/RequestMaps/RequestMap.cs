using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GameQueue.Core.Models.Maps;
using GameQueue.Core.Models.MapSearchRequests;

namespace GameQueue.Core.Models.RequestMaps;

[Table("RequestMaps")]
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
