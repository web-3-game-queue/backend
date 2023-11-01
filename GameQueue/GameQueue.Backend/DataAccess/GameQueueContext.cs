using GameQueue.Core.Entities;
using GameQueue.Core.Entities.MapSearchRequests.Status;
using GameQueue.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace GameQueue.Backend.DataAccess;

public class GameQueueContext : DbContext
{
    public DbSet<Map> Maps { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<MapSearchRequest> MapSearchRequests { get; set; }

    public DbSet<RequestToMap> RequestsToMap { get; set; }

    public GameQueueContext(DbContextOptions<GameQueueContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Map>()
            .Property(m => m.Status)
            .HasConversion(
                v => v.ToString(),
                v => (MapStatus)Enum.Parse(typeof(MapStatus), v))
            .HasDefaultValue(MapStatus.Pending);

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
