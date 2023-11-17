using GameQueue.Core.Commands.Users;
using GameQueue.Core.Entities;
using GameQueue.Core.Exceptions;
using GameQueue.Core.Services;
using GameQueue.Core.Services.Managers;
using GameQueue.Core.Services.Repositories;

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

    public async Task<User?> TryLogin(string username, string password, CancellationToken token = default)
    {
        var user = await userRepository.GetByUsername(username, token);
        var hashedPassword = passwordHasher.HashPassword(user, password);
        if (hashedPassword != user.HashedPassword)
        {
            return null;
        }
        return user;
    }

    public async Task Register(string username, string password, CancellationToken token = default)
    {
        User? user = null;
        try
        {
            user = await userRepository.GetByUsername(username, token);
        }
        catch (EntityNotFoundException)
        { }
        catch (InvalidOperationException)
        { }
        if (user != null)
        {
            throw new EntityAlreadyExistsException(typeof(User), user.Id);
        }
        var addUserCommand = new AddUserCommand {
            Name = username,
            Level = 1,
            Password = password,
            Role = Core.Models.UserRole.Client
        };
        await AddAsync(addUserCommand, token);
    }

    public async Task SetLevel(int userId, int level, CancellationToken token = default)
        => await userRepository.SetLevel(userId, level, token);

    private User convertAddCommandToUser(AddUserCommand addUserCommand)
    {
        var user = new User {
            Name = addUserCommand.Name,
            Level = addUserCommand.Level,
            Role = addUserCommand.Role
        };
        user.HashedPassword = passwordHasher.HashPassword(user, addUserCommand.Password);
        return user;
    }
}
