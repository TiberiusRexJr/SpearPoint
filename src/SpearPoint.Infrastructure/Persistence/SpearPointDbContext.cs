using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SpearPoint.Domain.Entities;
using System.Text.Json;

namespace SpearPoint.Infrastructure.Persistence;

public class SpearPointDbContext : DbContext
{
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<TestSession> TestSessions => Set<TestSession>();

    public SpearPointDbContext(DbContextOptions<SpearPointDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var jsonListConverter = new ValueConverter<List<string>, string>(
            list => JsonSerializer.Serialize(list, (JsonSerializerOptions?)null),
            json => JsonSerializer.Deserialize<List<string>>(json, (JsonSerializerOptions?)null) ?? new()
        );

        var listValueComparer = new ValueComparer<List<string>>(
            (currentList, otherList) => currentList!.SequenceEqual(otherList!),
            list => list.Aggregate(0, (combinedHash, item) => HashCode.Combine(combinedHash, item.GetHashCode())),
            list => list.ToList()
        );

        modelBuilder.Entity<Question>(question =>
        {
            question.Property(q => q.Choices)
                .HasConversion(jsonListConverter)
                .Metadata.SetValueComparer(listValueComparer);

            question.Property(q => q.Tags)
                .HasConversion(jsonListConverter)
                .Metadata.SetValueComparer(listValueComparer);
        });

        modelBuilder.Entity<TestSession>(testSession =>
        {
            testSession.OwnsMany(ts => ts.Items);
            testSession.OwnsOne(ts => ts.Summary);
        });
    }
}
