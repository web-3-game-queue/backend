namespace GameQueue.Core.Commands.Maps;

public sealed record UpdateMapCommand
{
    public int Id { get; set; }

    public string? Name { get; set; } = null!;

    public int? Width { get; set; }

    public int? Height { get; set; }

    public int? MaxPlayersCount { get; set; }

    public string? CoverImageUrl { get; set; } = null!;

    public decimal? Price { get; set; }

    public bool FieldsAreEmpty()
        => Name == null
            && Width == null
            && Height == null
            && MaxPlayersCount == null
            && CoverImageUrl == null
            && Price == null;
}
