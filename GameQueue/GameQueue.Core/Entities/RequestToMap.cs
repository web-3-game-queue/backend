using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameQueue.Core.Models;

[Table("request_to_maps")]
public sealed record RequestToMap
{
    [Required]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int SearchMapsRequestId { get; set; }

    [Required]
    public int MapId { get; set; }

    public SearchMapsRequest SearchMapsRequest { get; set; } = null!;

    public Map Map { get; set; } = null!;
}
