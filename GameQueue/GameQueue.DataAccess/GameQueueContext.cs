using GameQueue.Core.Entities;
using GameQueue.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace GameQueue.DataAccess;

public class GameQueueContext : DbContext
{
    public DbSet<Map> Maps { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<SearchMapsRequest> SearchMapsRequests { get; set; }

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
            .Entity<SearchMapsRequest>()
            .Property(m => m.Status)
            .HasConversion(
                v => v.ToString(),
                v => (SearchMapsRequestStatus)Enum.Parse(typeof(SearchMapsRequestStatus), v)
            );

        modelBuilder.Entity<SearchMapsRequest>()
            .HasOne(m => m.CreatorUser)
            .WithMany(u => u.SearchMapsRequests)
            .HasForeignKey(m => m.CreatorUserId);

        modelBuilder.Entity<SearchMapsRequest>()
            .HasOne(m => m.HandledByUser)
            .WithMany(u => u.HandledMapsRequests)
            .HasForeignKey(m => m.HandledByUserId);

        modelBuilder
            .Entity<RequestToMap>()
            .HasAlternateKey("SearchMapsRequestId", "MapId");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
            .UseSnakeCaseNamingConvention();
}
