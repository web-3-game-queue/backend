using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GameQueue.Core.Models;

namespace GameQueue.Core.Entities;

[Table("users")]
public sealed record class User
{
    [Required]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Required]
    public string HashedPassword { get; set; } = null!;

    [Required]
    [Range(0, int.MaxValue)]
    public int Level { get; set; }

    [Required]
    public UserRole Role { get; set; } = UserRole.Client;

    public List<SearchMapsRequest> SearchMapsRequests { get; } = new List<SearchMapsRequest>();

    public List<SearchMapsRequest> HandledMapsRequests { get; } = new List<SearchMapsRequest>();
}
