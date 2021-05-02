using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace RestaurantApp.Core.RepositoryInterface
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity GetById(int id);
        IEnumerable<TEntity> GetAll(
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null
        );
        TEntity GetFirstOrDefault(
            Expression<Func<TEntity, bool>> predicate = null,
            string includeProperties = null
        );
        bool Any(Expression<Func<TEntity, bool>> predicate = null);
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        void Remove(int id);
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
    }
}
