using System.Text.Json.Serialization;
using GameQueue.Backend;
using GameQueue.Backend.ExceptionFilters;
using GameQueue.Backend.Extensions;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services
    .AddSingleton(new ModeratorUser {
        Id = int.Parse(configuration["ModeratorId"])
    });

builder.Services
    .AddDb(configuration)
    .AddRepositories()
    .AddManagers();

builder.Services
    .AddControllers(options
        => options.Filters.Add(new ExceptionFilter()))
    .AddJsonOptions(options
        => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app
    .UseHttpsRedirection()
    .UseAuthorization();

app.MapControllers();

app.Run();
