using System.ComponentModel.DataAnnotations;

namespace GameQueue.Core.Commands.Users;

public sealed record AddUserCommand
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

    [Required]
    [Range(0, int.MaxValue)]
    public int Level { get; set; }
}
