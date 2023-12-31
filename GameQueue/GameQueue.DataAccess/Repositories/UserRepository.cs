﻿using GameQueue.Core.Entities;
using GameQueue.Core.Exceptions;
using GameQueue.Core.Services.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GameQueue.DataAccess.Repositories;

internal class UserRepository : IUserRepository
{
    private readonly GameQueueContext db;

    public UserRepository(GameQueueContext db) => this.db = db;

    public async Task<ICollection<User>> GetAllAsync(CancellationToken token = default)
        => await db.Users.ToListAsync(token);

    public async Task<User> GetByIdAsync(int id, CancellationToken token = default)
        => await db.Users.FindAsync(id, token) ?? throw new EntityNotFoundException(typeof(User), id);

    public async Task AddAsync(User user, CancellationToken token = default)
    {
        await db.Users.AddAsync(user);
        await db.SaveChangesAsync(token);
    }

    public async Task<User> GetByUsername(string username, CancellationToken token = default)
        => await db.Users.Where(x => x.Name == username).SingleAsync();
}
