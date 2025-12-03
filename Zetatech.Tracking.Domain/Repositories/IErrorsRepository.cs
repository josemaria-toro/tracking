using Zetatech.Accelerate.Domain;
using Zetatech.Tracking.Domain.Entities;

namespace Zetatech.Tracking.Domain.Repositories;

/// <summary>
/// Provides the interface for implementing custom domain repositories for managing errors.
/// </summary>
public interface IErrorsRepository : IRepository<ErrorEntity>
{
}
