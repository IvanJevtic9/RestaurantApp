using Microsoft.EntityFrameworkCore;
using RestaurantApp.Core.RepositoryInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace RestaurantApp.Infrastructure.Data.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        internal DbSet<TEntity> dbSet;

        public Repository(ApplicationDbContext db)
        {
            dbSet = db.Set<TEntity>();
        }
        public void Add(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entites)
        {
            dbSet.AddRange(entites);
        }

        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return dbSet.Any(predicate);
        }

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (predicate != null) query = query.Where(predicate);
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }
            if (orderBy != null) return orderBy(query).ToList();
            return query.ToList();
        }

        public TEntity GetById(int id)
        {
            return dbSet.Find(id);
        }

        public TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> predicate = null, string includeProperties = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (predicate != null) query = query.Where(predicate);
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }
            return query.FirstOrDefault();
        }

        public void Remove(int id)
        {
            dbSet.Remove(dbSet.Find(id));
        }

        public void Remove(TEntity entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            dbSet.RemoveRange(entities);
        }
    }
}
