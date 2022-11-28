using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected RepositoryContext RepositoryContext { get; set; } 
        public RepositoryBase(RepositoryContext repositoryContext) 
        {
            RepositoryContext = repositoryContext; 
        }

        //public IQueryable<T> FindAll() => RepositoryContext.Set<T>().AsNoTracking();
        public virtual IQueryable<T> FindAll()
        {
#if (ENABLED_FOR_LAZY_LOADING_USAGE)
            return RepositoryContext.Set<T>();
#else
            return this.RepositoryContext.Set<T>().AsNoTracking();
#endif
        }
        //public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression) => 
        //    RepositoryContext.Set<T>().Where(expression).AsNoTracking();

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
#if (ENABLED_FOR_LAZY_LOADING_USAGE)
            return this.RepositoryContext.Set<T>().Where(expression);
#else
            return this.RepositoryContext.Set<T>().Where(expression).AsNoTracking();
#endif
        }
        public void Create(T entity) => RepositoryContext.Set<T>().Add(entity);

        public void Update(T entity) => RepositoryContext.Set<T>().Update(entity);

        public void Delete(T entity) => RepositoryContext.Set<T>().Remove(entity);

        // LTPE below
        public virtual void EnableLazyLoading()
        {
            this.RepositoryContext.ChangeTracker.LazyLoadingEnabled = true;
        }

        public virtual void DisableLazyLoading()
        {
            this.RepositoryContext.ChangeTracker.LazyLoadingEnabled = false;
        }
    }
}
