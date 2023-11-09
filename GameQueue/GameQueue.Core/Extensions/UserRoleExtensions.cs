using GameQueue.Api.Contracts.Models;
using GameQueue.Core.Models;

namespace GameQueue.Core.Extensions;

public static class UserRoleExtensions
{
    public static UserRoleApi ToUserRoleApi(this UserRole role)
        => role switch {
            UserRole.Client => UserRoleApi.Client,
            UserRole.Moderator => UserRoleApi.Moderator,
            UserRole.Administrator => UserRoleApi.Administrator,
            _ => throw new NotImplementedException(),
        };
}
