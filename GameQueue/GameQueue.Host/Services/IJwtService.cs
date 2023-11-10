using System.IdentityModel.Tokens.Jwt;
using GameQueue.Core.Entities;

namespace GameQueue.Host.Services;

public interface IJwtService
{
    string GenerateToken(User user);
}
