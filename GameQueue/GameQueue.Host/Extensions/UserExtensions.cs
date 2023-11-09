using System.Security.Claims;
using GameQueue.Core.Entities;

namespace GameQueue.Host.Extensions;

public static class UserExtensions
{
    public static List<Claim> ToClaimsList(this User user)
        => new List<Claim> {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim(ClaimTypes.Sid, user.Id.ToString())
        };
}
