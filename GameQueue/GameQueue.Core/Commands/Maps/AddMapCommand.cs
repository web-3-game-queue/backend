namespace GameQueue.Core.Commands.Maps;

public sealed record AddMapCommand
{
    public string Name { get; set; } = null!;

    public int Width { get; set; }

    public int Height { get; set; }

    public int MaxPlayersCount { get; set; }

    public string CoverImageUrl { get; set; } = null!;

    public decimal Price { get; set; }

    public Stream CoverImageData { get; set; } = null!;

    public string ContentType { get; set; } = null!;
}
