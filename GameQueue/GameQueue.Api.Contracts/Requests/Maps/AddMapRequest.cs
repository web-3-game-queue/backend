using System.ComponentModel.DataAnnotations;

namespace GameQueue.Api.Contracts.Requests.Maps;

public sealed record AddMapRequest
{
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    [Range(0, int.MaxValue)]
    public int Width { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int Height { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int MaxPlayersCount { get; set; }

    [Required]
    public string CoverImageUrl { get; set; } = null!;

    [Required]
    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }
}
