using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Zetatech.Accelerate.Persistency.Repositories;
using Zetatech.Accelerate.Tracking;
using Zetatech.Tracking.Domain.Entities;
using Zetatech.Tracking.Domain.Repositories;

namespace Zetatech.Tracking.Infrastructure.Persistency;

/// <summary>
/// Represents the repository to manage dependencies.
/// </summary>
public sealed class DependenciesRepository : PostgreSqlRepository<DependencyEntity, PostgreSqlRepositoryOptions>, IDependenciesRepository
{
    /// <summary>
    /// Initializes a new instance of the class with the specified options.
    /// </summary>
    /// <param name="options">
    /// The repository options to be used.
    /// </param>
    /// <param name="trackingService">
    /// Service for tracking application data.
    /// </param>
    public DependenciesRepository(IOptions<PostgreSqlRepositoryOptions> options,
                                  ITrackingService trackingService = null) : base(options, trackingService)
    {
    }

    /// <summary>
    /// Configures the model that was discovered by convention from the entity types exposed in <see cref="DbSet{TEntity}"/> properties on your derived context.
    /// </summary>
    /// <param name="modelBuilder">
    /// The builder being used to construct the model for this context.
    /// </param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DependencyEntity>(entity =>
        {
            entity.ToTable("dependencies", Options.Schema);
            entity.HasKey(x => new { x.AppId, x.Id })
                  .HasName("pk_dependencies");
            entity.Property(x => x.AppId)
                  .HasColumnName("app_id")
                  .IsRequired();
            entity.Property(x => x.CreatedAt)
                  .HasColumnName("created_at")
                  .IsRequired();
            entity.Property(x => x.Duration)
                  .HasColumnName("duration")
                  .IsRequired();
            entity.Property(x => x.Id)
                  .HasColumnName("id")
                  .IsRequired();
            entity.Property(x => x.InputData)
                  .HasColumnName("input_data")
                  .HasMaxLength(8192)
                  .IsRequired();
            entity.Property(x => x.Name)
                  .HasColumnName("name")
                  .HasMaxLength(128)
                  .IsRequired();
            entity.Property(x => x.OperationId)
                  .HasColumnName("operation_id")
                  .IsRequired();
            entity.Property(x => x.OutputData)
                  .HasColumnName("output_data")
                  .HasMaxLength(8192)
                  .IsRequired();
            entity.Property(x => x.Success)
                  .HasColumnName("success")
                  .IsRequired();
            entity.Property(x => x.TargetName)
                  .HasColumnName("target_name")
                  .HasMaxLength(4096)
                  .IsRequired();
            entity.Property(x => x.Type)
                  .HasColumnName("type")
                  .HasMaxLength(128)
                  .IsRequired();
            entity.Property(x => x.UpdatedAt)
                  .HasColumnName("updated_at")
                  .IsRequired();
        });
    }
}