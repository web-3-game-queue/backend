using System.ComponentModel.DataAnnotations;
using GameQueue.Core.Entities;

namespace GameQueue.Core.Commands.Maps;

public sealed record AddMapCommand
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

    public Map ToMap()
        => new Map {
            Name = Name,
            Width = Width,
            Height = Height,
            CoverImageUrl = CoverImageUrl,
            Price = Price
        };
}
