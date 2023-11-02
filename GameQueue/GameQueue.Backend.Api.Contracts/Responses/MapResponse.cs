namespace GameQueue.Backend.Api.Contracts.Responses;

public sealed record MapResponse
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Width { get; set; }

    public int Height { get; set; }

    public int MaxPlayersCount { get; set; }

    public string CoverImageUrl { get; set; } = null!;

    public decimal Price { get; set; }
}
