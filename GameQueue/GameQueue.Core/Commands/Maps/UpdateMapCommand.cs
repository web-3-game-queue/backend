using GameQueue.Core.Models;

namespace GameQueue.Core.Commands.Maps;

public sealed record UpdateMapCommand
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? Width { get; set; }

    public int? Height { get; set; }

    public int? MaxPlayersCount { get; set; }

    public decimal? Price { get; set; }

    public CoverImageUploadModel? CoverImageFile { get; set; }

    public bool FieldsAreEmpty()
        => Name == null
            && Width == null
            && Height == null
            && MaxPlayersCount == null
            && Price == null
            && CoverImageFile == null;
}
