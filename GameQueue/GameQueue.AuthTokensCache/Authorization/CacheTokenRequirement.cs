using Microsoft.AspNetCore.Authorization;

namespace GameQueue.AuthTokensCache.Authorization;

public class CacheTokenRequirement: IAuthorizationRequirement
{
    public const string Name = "CacheTokenRequirement";
}
