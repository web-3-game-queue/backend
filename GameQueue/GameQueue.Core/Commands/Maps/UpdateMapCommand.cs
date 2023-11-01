using System.ComponentModel.DataAnnotations;
using GameQueue.Core.Entities;

namespace GameQueue.Core.Commands.Maps;

public sealed record UpdateMapCommand
{
    [Required]
    public int Id { get; set; }

    [StringLength(100)]
    public string? Name { get; set; } = null!;

    [Range(0, int.MaxValue)]
    public int? Width { get; set; }

    [Range(0, int.MaxValue)]
    public int? Height { get; set; }

    [Range(0, int.MaxValue)]
    public int? MaxPlayersCount { get; set; }

    public string? CoverImageUrl { get; set; } = null!;

    [Range(0, double.MaxValue)]
    public decimal? Price { get; set; }

    public bool FieldsAreEmpty()
        => Name == null
            && Width == null
            && Height == null
            && MaxPlayersCount == null
            && CoverImageUrl == null
            && Price == null;

    public void Update(Map map)
        => map = map with {
            Name = Name ?? map.Name,
            Width = Width ?? map.Width,
            Height = Height ?? map.Height,
            MaxPlayersCount = MaxPlayersCount ?? map.MaxPlayersCount,
            CoverImageUrl = CoverImageUrl ?? map.CoverImageUrl,
            Price = Price ?? map.Price,
        };
}
