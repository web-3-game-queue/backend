using GameQueue.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GameQueue.S3Access.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddS3Manager(this IServiceCollection services)
        => services.AddScoped<IS3Manager, S3Manager>();
}
