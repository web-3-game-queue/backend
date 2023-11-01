using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameQueue.Core.Entities;

[Table("users")]
public sealed record class User
{
    [Required]
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Required]
    public string HashedPassword { get; set; } = null!;

    [Required]
    [Range(0, int.MaxValue)]
    public int Level { get; set; }
}
