using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using OpenLatino.Core.Domain.Interfaces;

namespace Openlatino.Admin.Infrastucture.Services
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        public Repository(IUnitOfWork unitOfWork)
        {
            // check preconditions
            // set internal values
            UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            // create object set from unit of work
            Set = ((DbContext)unitOfWork).Set<TEntity>();
        }

        private DbSet<TEntity> Set { get; }

        #region IRepository<TEntity> Members

        /// <inheritdoc />
        /// <summary>
        ///     Gets the unit of work in this repository.
        /// </summary>
        public IUnitOfWork UnitOfWork { get; protected set; }

        /// <inheritdoc />
        /// <summary>
        ///     Add item into repository.
        /// </summary>
        /// <param name="item">Item to add to repository.</param>
        public virtual TEntity Add(TEntity item)
        {
            // check item
            if (item == null)
                // error
                throw new ArgumentNullException(nameof(item));

            // attach object to unit of work
            return Set.Add(item).Entity;
        }

        /// <inheritdoc />
        /// <summary>
        ///     Gets an element by it's entity key.
        /// </summary>
        /// <param name="entityKeyValues">Entity key values, the order the are same of order in mapping.</param>
        /// <returns>The element found, otherwise null.</returns>
        public virtual TEntity Find(params object[] entityKeyValues)
        {
            // local instance of entity
            TEntity entity = null;

            if (entityKeyValues != null && entityKeyValues.Length > 0)
                // gets entity
                entity = Set.Find(entityKeyValues);

            // return located entity or null
            return entity;
        }

        public virtual async Task<TEntity> FindAsync(params object[] entityKeyValues)
        {
            return await Set.FindAsync(entityKeyValues);
        }

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
        public virtual IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null,
            bool noTracking = false,
            params Expression<Func<TEntity, object>>[] joinedEntities)
        {
            // create IDbSet 
            IQueryable<TEntity> query = Set;

            if (filter != null)
                query = query.Where(filter);

            if (joinedEntities != null)
                query = joinedEntities.Aggregate(query, (current, joinedEntity) => current.Include(joinedEntity));

            if (noTracking)
                query = query.AsNoTracking();

            // return not materialized query
            return query;
        }


        /// <inheritdoc />
        /// <summary>
        ///     Delete item.
        /// </summary>
        /// <param name="item">Item to delete.</param>
        public virtual TEntity Remove(TEntity item)
        {
            // check item
            if (item == null)
                // error
                throw new ArgumentNullException(nameof(item));
            
            // delete object from DbSet
            return Set.Remove(item).Entity;
        }

        /// <inheritdoc />
        /// <summary>
        ///     Sets modified entity into the repository. When calling Commit() method in UnitOfWork these changes will be saved
        ///     into the storage.
        ///     <remarks>
        ///         Internally this method always calls Repository.Attach().
        ///     </remarks>
        /// </summary>
        /// <param name="item">Item with changes.</param>
        public virtual TEntity Modify(TEntity item)
        {
            // check arguments
            if (item == null)
                // error
                throw new ArgumentNullException(nameof(item));

            // set object as modified
            try
            {
                Set.Attach(item);
            }
            catch { }
            ((DbContext)UnitOfWork).Entry(item).State = EntityState.Modified;
            return item;
        }

        #endregion
    }
}