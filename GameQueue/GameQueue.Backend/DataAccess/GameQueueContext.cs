using GameQueue.Core.Models.Maps;
using GameQueue.Core.Models.Maps.Status;
using GameQueue.Core.Models.MapSearchRequests;
using GameQueue.Core.Models.MapSearchRequests.Status;
using GameQueue.Core.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace GameQueue.Backend.DataAccess;

public class GameQueueContext : DbContext
{
    private IConfiguration configuration;

    public DbSet<Map> Maps { get; set; }

    public DbSet<User> Users { get; set; }

    public GameQueueContext(
        DbContextOptions<GameQueueContext> options,
        IConfiguration configuration) : base(options)
    {
        this.configuration = configuration;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Map>()
            .Property(m => m.Status)
            .HasConversion(
                v => v.ToString(),
                v => (MapStatus)Enum.Parse(typeof(MapStatus), v))
            .HasDefaultValue(MapStatus.Unknown);

        modelBuilder
            .Entity<MapSearchRequest>()
            .Property(m => m.Status)
            .HasConversion(
                v => v.ToString(),
                v => (MapSearchRequestStatus)Enum.Parse(typeof(MapSearchRequestStatus), v)
            );
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
            .UseSnakeCaseNamingConvention();
}
