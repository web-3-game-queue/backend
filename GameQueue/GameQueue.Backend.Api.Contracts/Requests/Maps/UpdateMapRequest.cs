using System.ComponentModel.DataAnnotations;

namespace GameQueue.Api.Contracts.Requests.Maps;

public sealed record UpdateMapRequest
{
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
}
