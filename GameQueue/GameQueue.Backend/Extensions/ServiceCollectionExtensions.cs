using GameQueue.Backend.DataAccess;
using GameQueue.Backend.Services.Repositories;
using GameQueue.Core.Contracts.Services.Repositories;
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
}
