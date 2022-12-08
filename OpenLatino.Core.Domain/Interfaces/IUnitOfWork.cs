using System;

namespace OpenLatino.Core.Domain.Interfaces
{
    /// <inheritdoc />
    /// <summary>
    ///     Contract for UnitOfWork pattern.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        ///     Commit all changes made in a container.
        /// </summary>
        /// <returns></returns>
        int SaveChanges();

        /// <summary>
        ///     Gets the TEntity repository.
        /// </summary>
        /// <typeparam name="TEntity">Represents a database's entity.</typeparam>
        /// <returns>A <see cref="IRepository{TEntity}" /> object.</returns>
        IRepository<TEntity> Set<TEntity>() where TEntity : class;
    }
}