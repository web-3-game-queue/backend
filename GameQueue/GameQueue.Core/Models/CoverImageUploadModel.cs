namespace GameQueue.Core.Models;

public sealed record CoverImageUploadModel
{
    public string? Url { get; set; }

    public Stream FileData { get; set; } = null!;

    public string ContentType { get; set; } = null!;
}
