using GameQueue.Core.Services.Repositories;
using GameQueue.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GameQueue.DataAccess.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDb(this IServiceCollection services, IConfiguration configuration)
        => services.AddDbContext<GameQueueContext>(
            opt => opt
                    .UseNpgsql(configuration.GetConnectionString("DB_URL"), b => b.MigrationsAssembly("GameQueue.Host")));

    public static IServiceCollection AddRepositories(this IServiceCollection services)
        => services
            .AddScoped<IMapRepository, MapRepository>()
            .AddScoped<ISearchMapsRequestRepository, SearchMapsRequestRepository>()
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IRequestToMapRepository, RequestToMapRepository>();
}
