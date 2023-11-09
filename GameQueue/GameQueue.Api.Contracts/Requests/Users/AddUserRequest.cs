using System.ComponentModel.DataAnnotations;
using GameQueue.Api.Contracts.Models;

namespace GameQueue.Api.Contracts.Requests.Users;

public sealed record AddUserRequest
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

    [Required]
    [Range(0, int.MaxValue)]
    public int Level { get; set; }

    public UserRoleApi Role { get; set; } = UserRoleApi.Client;
}
