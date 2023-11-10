using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GameQueue.Core.Entities;
using GameQueue.Host.Extensions;
using Microsoft.IdentityModel.Tokens;

namespace GameQueue.Host.Services;

public class JwtService : IJwtService
{
    private readonly string issuer;
    private readonly string audience;
    private readonly TimeSpan lifetime;
    private readonly byte[] key;

    public JwtService(IConfiguration configuration)
    {
        var jwtConfig = configuration.GetRequiredSection("Jwt");
        var lifetimeSeconds = double.Parse(
            jwtConfig["LifetimeSeconds"]
            ?? throw new NullReferenceException("jwtConfig[LifetimeSeconds]"));

        issuer = jwtConfig["Issuer"]
            ?? throw new NullReferenceException("jwtConfig[Issuer]");
        audience = jwtConfig["Audience"]
            ?? throw new NullReferenceException("jwConfig[Audience]");
        lifetime = TimeSpan.FromSeconds(lifetimeSeconds);
        key = Encoding.UTF8.GetBytes(jwtConfig["Key"]
            ?? throw new NullReferenceException("jwtConfig[Key]"));
    }

    public string GenerateToken(User user)
    {
        var claims = user.ToClaimsList();
        var now = DateTime.UtcNow;
        var jwt = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            notBefore: now,
            claims: claims,
            expires: now.Add(lifetime),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        );
        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
        return encodedJwt;
    }
}
