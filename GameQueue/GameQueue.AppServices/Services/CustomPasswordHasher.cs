using System.Security.Cryptography;
using System.Text;
using GameQueue.Core.Entities;
using GameQueue.Core.Services;

namespace GameQueue.AppServices.Services;

internal class CustomPasswordHasher : IPasswordHasher<User>
{
    private const int KeySize = 64;
    private const int Iterations = 350_000;
    private static readonly HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA512;

    public string HashPassword(User user, string password)
    {
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(password),
            Encoding.UTF8.GetBytes(user.Name),
            Iterations,
            HashAlgorithm,
            KeySize
        );
        return Convert.ToHexString(hash);
    }

    public bool IsCorrectPasword(User user, string hashedPassword, string providedPassword)
    {
        var hashedProvidedPassword = HashPassword(user, hashedPassword);
        return hashedPassword == hashedProvidedPassword;
    }
}
