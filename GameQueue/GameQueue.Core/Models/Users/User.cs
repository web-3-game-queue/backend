namespace GameQueue.Core.Models.Users;

public sealed record class User
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string HashedPassword { get; set; }

    public int Level { get; set; }
}
