namespace GameQueue.Core.Services;

public interface IPasswordHasher<T>
{
    public string HashPassword(T value, string password);

    public bool IsCorrectPasword(T value, string hashedPassword, string providedPassword);
}
