using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Openlatino.Admin.Infrastucture.DataContexts;
using OpenLatino.Core.Domain.Interfaces;

namespace Openlatino.Admin.Infrastucture.Services
{
    /// <inheritdoc>
    ///     <cref></cref>
    /// </inheritdoc>
    /// <summary>
    ///     Contract for UnitOfWork pattern.
    /// </summary>
    public abstract class UnitOfWork : IdentityDbContext , IUnitOfWork
    {
        protected UnitOfWork(DbContextOptions options): base(options)
        {
            //Configuration.ProxyCreationEnabled = Configuration.LazyLoadingEnabled = true;
            ///Hacerlo de esta forma desde el proyecto web***********
            // .AddDbContext<BloggingContext>(
            //b => b.UseLazyLoadingProxies()
            //.UseSqlServer(myConnectionString));
           
            //var test = this.Set<OpenLatino.Core.Domain.Entities.Layer>();
        }


        //Probar llamando a este, que lo genere yo.-------------------------------------------------------
        protected UnitOfWork(string connectionString) : base(GetOptions(connectionString))
        {}

        private static DbContextOptions GetOptions(string connectionString)
        {
            return SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), connectionString).Options;
        }

        /// <inheritdoc />
        /// <summary>
        ///     Gets the TEntity repository.
        /// </summary>
        /// <typeparam name = "TEntity" > Represents a database's entity.</typeparam>
        /// <returns>A<see cref="T:OpenLatino.Core.Domain.Interfaces.IRepository`1" /> object.</returns>
        IRepository<TEntity> IUnitOfWork.Set<TEntity>()
        {
            return new Repository<TEntity>(this);
        }

        public IRepository<TEntity> getRopo<TEntity>() where TEntity : class
        {
            return new Repository<TEntity>(this);
        }
    }
}