using GameQueue.Backend.Api.Contracts.Responses;
using GameQueue.Core.Models;

namespace GameQueue.Core.Extensions;

public static class UserExtensions
{
    public static UserResponse ToUserResponse(this User user)
        => new UserResponse {
            Id = user.Id,
            Name = user.Name,
            Level = user.Level,
        };
}
