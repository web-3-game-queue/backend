﻿using GameQueue.AppServices.Services;
using GameQueue.AppServices.Services.Managers;
using GameQueue.Core.Contracts.Services.Managers;
using GameQueue.Core.Models;
using GameQueue.Core.Services;
using GameQueue.Core.Services.Managers;
using Microsoft.Extensions.DependencyInjection;

namespace GameQueue.Backend.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddManagers(this IServiceCollection services)
        => services
            .AddScoped<IPasswordHasher<User>, CustomPasswordHasher>()
            .AddScoped<IUserManager, UserManager>()
            .AddScoped<IMapManager, MapManager>()
            .AddScoped<ISearchMapsRequestManager, SearchMapsRequestManager>();
}
