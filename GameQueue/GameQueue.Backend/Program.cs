using GameQueue.Backend.ExceptionFilters;
using GameQueue.Backend.Extensions;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services
    .AddDb(builder.Configuration)
    .AddRepositories()
    .AddManagers();

builder.Services
    .AddControllers(
        options => options.Filters.Add(new ExceptionFilter()));

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
