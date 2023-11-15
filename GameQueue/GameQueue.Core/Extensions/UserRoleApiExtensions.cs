using GameQueue.Api.Contracts.Models;
using GameQueue.Core.Models;

namespace GameQueue.Core.Extensions;

public static class UserRoleApiExtensions
{
    public static UserRole ToUserRole(this UserRoleApi role)
        => role switch {
            UserRoleApi.Client => UserRole.Client,
            UserRoleApi.Moderator => UserRole.Moderator,
            UserRoleApi.Administrator => UserRole.Administrator,
            _ => throw new NotImplementedException(),
        };
}
