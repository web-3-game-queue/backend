using System.Text.Json.Serialization;
using GameQueue.AppServices.Extensions;
using GameQueue.AuthTokensCache.Authorization;
using GameQueue.AuthTokensCache.Extensions;
using GameQueue.DataAccess.Extensions;
using GameQueue.Host.ExceptionFilters;
using GameQueue.Host.Extensions;
using GameQueue.S3Access.Extensions;
using Microsoft.OpenApi.Models;
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

//builder.Services
//    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie();
builder.Services.AddJwtAuth(configuration);
builder.Services.AddJwtAuthWithTokenCache(configuration);
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy(
//        CacheTokenRequirement.Name,
//        policy => policy.Requirements.Add(new CacheTokenRequirement()));
//});

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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo {
        Title = "game-queue API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
        In = ParameterLocation.Header,
        Description = "JWT",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

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
