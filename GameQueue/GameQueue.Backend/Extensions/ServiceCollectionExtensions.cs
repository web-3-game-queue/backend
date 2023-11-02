using GameQueue.Backend.DataAccess;
using GameQueue.Backend.Services;
using GameQueue.Backend.Services.Managers;
using GameQueue.Backend.Services.Repositories;
using GameQueue.Core.Contracts.Services.Managers;
using GameQueue.Core.Contracts.Services.Repositories;
using GameQueue.Core.Models;
using GameQueue.Core.Services.Managers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GameQueue.Backend.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDb(this IServiceCollection services, IConfiguration configuration)
        => services.AddDbContext<GameQueueContext>(
            opt => opt.UseNpgsql(configuration.GetConnectionString("DB_URL")));

    public static IServiceCollection AddRepositories(this IServiceCollection services)
        => services
            .AddScoped<IMapRepository, MapRepository>()
            .AddScoped<ISearchMapsRequestRepository, SearchMapsRequestRepository>()
            .AddScoped<IUserRepository, UserRepository>();

    public static IServiceCollection AddManagers(this IServiceCollection services)
        => services
            .AddScoped<IPasswordHasher<User>, CustomPasswordHasher>()
            .AddScoped<IUserManager, UserManager>()
            .AddScoped<IMapManager, MapManager>()
            .AddScoped<ISearchMapsRequestManager, SearchMapsRequestManager>();
}
