using GameQueue.Core.Commands.Users;
using GameQueue.Core.Contracts.Services.Managers;
using GameQueue.Core.Contracts.Services.Repositories;
using GameQueue.Core.Models;
using GameQueue.Core.Services;

namespace GameQueue.AppServices.Services.Managers;

internal class UserManager : IUserManager
{
    private IUserRepository userRepository;

    private IPasswordHasher<User> passwordHasher;

    public UserManager(
        IUserRepository userRepository,
        IPasswordHasher<User> passwordHasher)
    {
        this.userRepository = userRepository;
        this.passwordHasher = passwordHasher;
    }

    public async Task<ICollection<User>> GetAllAsync(CancellationToken token = default)
        => await userRepository.GetAllAsync(token);

    public async Task<User> GetByIdAsync(int id, CancellationToken token = default)
        => await userRepository.GetByIdAsync(id, token);

    public async Task AddAsync(AddUserCommand addUserCommand, CancellationToken token = default)
    {
        var user = convertAddCommandToUser(addUserCommand);
        await userRepository.AddAsync(user, token);
    }

    private User convertAddCommandToUser(AddUserCommand addUserCommand)
    {
        var user = new User {
            Name = addUserCommand.Name,
            Level = addUserCommand.Level,
        };
        user.HashedPassword = passwordHasher.HashPassword(user, addUserCommand.Password);
        return user;
    }
}
