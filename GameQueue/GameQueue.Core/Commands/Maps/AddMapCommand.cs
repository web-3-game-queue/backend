using GameQueue.Core.Models;

namespace GameQueue.Core.Commands.Maps;

public sealed record AddMapCommand
{
    public string Name { get; set; } = null!;

    public int Width { get; set; }

    public int Height { get; set; }

    public int MaxPlayersCount { get; set; }

    public decimal Price { get; set; }

    public CoverImageUploadModel? CoverImageFile { get; set; }
}
