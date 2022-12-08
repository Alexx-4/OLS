using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OpenLatino.Core.Domain.Interfaces
{
    /// <summary>
    ///     Base interface for implement a "Repository Pattern".
    /// </summary>
    /// <remarks>
    ///     Indeed, one might think that IObjectSet is already a generic repository and therefore  would not need this item.
    ///     Using this interface allows us to ensure PI principle within our domain model.
    /// </remarks>
    /// <typeparam name="TEntity">Type of entity for this repository.</typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        ///     Gets the unit of work in this repository.
        /// </summary>
        IUnitOfWork UnitOfWork { get; }

        /// <summary>
        ///     Add item into repository.
        /// </summary>
        /// <param name="item">Item to add to repository.</param>
        TEntity Add(TEntity item);

        /// <summary>
        ///     Delete item.
        /// </summary>
        /// <param name="item">Item to delete.</param>
        TEntity Remove(TEntity item);

        /// <summary>
        ///     Sets modified entity into the repository. When calling Commit() method in UnitOfWork these changes will be saved
        ///     into the storage.
        ///     <remarks>
        ///         Internally this method always calls Repository.Attach().
        ///     </remarks>
        /// </summary>
        /// <param name="item">Item with changes.</param>
        TEntity Modify(TEntity item);

        /// <summary>
        ///     Get all elements of type <typeparamref name="TEntity" /> in repository.
        /// </summary>
        /// <param name="filter">Filters the elements in database BEFORE materialize the query.</param>
        /// <param name="noTracking">Indicates that the resulting objects are not tracked by EF.</param>
        /// <param name="joinedEntities">
        ///     Include these entities in the query result, otherwise these navigation fields will be
        ///     null.
        /// </param>
        /// <returns>List of selected elements.</returns>
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null, bool noTracking = false,
            params Expression<Func<TEntity, object>>[] joinedEntities);

       
        /// <summary>
        ///     Gets an element by it's entity key.
        /// </summary>
        /// <param name="entityKeyValues">Entity key values, the order the are same of order in mapping.</param>
        /// <returns>The element found, otherwise null.</returns>
        TEntity Find(params object[] entityKeyValues);

        Task<TEntity> FindAsync(params object[] entityKeyValues);
    }
}
