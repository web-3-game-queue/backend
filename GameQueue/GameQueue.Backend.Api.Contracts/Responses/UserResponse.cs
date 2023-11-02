namespace GameQueue.Backend.Api.Contracts.Responses;

public sealed record UserResponse
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Level { get; set; }
}
