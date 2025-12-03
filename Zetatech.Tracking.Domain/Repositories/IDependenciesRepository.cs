using Zetatech.Accelerate.Domain;
using Zetatech.Tracking.Domain.Entities;

namespace Zetatech.Tracking.Domain.Repositories;

/// <summary>
/// Provides the interface for implementing custom domain repositories for managing dependencies.
/// </summary>
public interface IDependenciesRepository : IRepository<DependencyEntity>
{
}
