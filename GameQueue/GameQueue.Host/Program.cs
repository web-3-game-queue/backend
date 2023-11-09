using System.Text.Json.Serialization;
using GameQueue.AppServices.Extensions;
using GameQueue.DataAccess.Extensions;
using GameQueue.Host.ExceptionFilters;
using GameQueue.S3Access.Extensions;
using Minio;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

var minioClient = new MinioClient()
    .WithEndpoint(configuration["MINIO_ENDPOINT"])
    .WithCredentials(
        configuration["MINIO_ACCESS_KEY"],
        configuration["MINIO_SECRET_KEY"])
    .WithSSL(false)
    .Build();

builder.Services.AddCors();

builder.Services.AddSingleton(minioClient);

builder.Services
    .AddHttpClient();

builder.Services
    .AddS3Manager()
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

//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseCors(builder => 
    builder
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod());

app.UseAuthorization();

app.MapControllers();

app.Run();
