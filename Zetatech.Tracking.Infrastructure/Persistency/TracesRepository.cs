using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Zetatech.Accelerate.Persistency.Repositories;
using Zetatech.Accelerate.Tracking;
using Zetatech.Tracking.Domain.Entities;
using Zetatech.Tracking.Domain.Repositories;

namespace Zetatech.Tracking.Infrastructure.Persistency;

/// <summary>
/// Represents the repository to manage traces.
/// </summary>
public sealed class TracesRepository : PostgreSqlRepository<TraceEntity, PostgreSqlRepositoryOptions>, ITracesRepository
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
    public TracesRepository(IOptions<PostgreSqlRepositoryOptions> options,
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
        modelBuilder.Entity<TraceEntity>(entity =>
        {
            entity.ToTable("traces", Options.Schema);
            entity.HasKey(x => new { x.AppId, x.Id })
                  .HasName("pk_traces");
            entity.Property(x => x.AppId)
                  .HasColumnName("app_id")
                  .IsRequired();
            entity.Property(x => x.Id)
                  .HasColumnName("id")
                  .IsRequired();
            entity.Property(x => x.Message)
                  .HasColumnName("message")
                  .HasMaxLength(8192)
                  .IsRequired();
            entity.Property(x => x.OperationId)
                  .HasColumnName("operation_id")
                  .IsRequired();
            entity.Property(x => x.Severity)
                  .HasColumnName("severity_level")
                  .IsRequired();
            entity.Property(x => x.SourceTypeName)
                  .HasColumnName("source_type_name")
                  .HasMaxLength(128)
                  .IsRequired();
            entity.Property(x => x.Timestamp)
                  .HasColumnName("timestamp")
                  .IsRequired();
        });
    }
}