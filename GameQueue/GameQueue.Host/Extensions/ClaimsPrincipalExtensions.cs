using System.Security.Claims;

namespace GameQueue.Host.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int Id(this ClaimsPrincipal principal)
        => int.Parse(
            principal.FindFirstValue(ClaimTypes.Sid)
            ?? throw new NullReferenceException(nameof(ClaimTypes.Sid)));
}
