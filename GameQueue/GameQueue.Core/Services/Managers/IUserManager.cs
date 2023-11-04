﻿using GameQueue.Core.Commands.Users;
using GameQueue.Core.Entities;

namespace GameQueue.Core.Services.Managers;

public interface IUserManager
{
    Task<ICollection<User>> GetAllAsync(CancellationToken token = default);

    Task<User> GetByIdAsync(int id, CancellationToken token = default);

    Task AddAsync(AddUserCommand addUserCommand, CancellationToken token = default);
}
