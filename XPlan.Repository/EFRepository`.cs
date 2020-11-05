using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using XPlan.Repository.Abstracts;
using XPlan.Repository.EntityFrameworkCore.Extensions;

namespace XPlan.Repository.EntityFrameworkCore
{
    public class EFRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, new()
    {
        public EFRepository(XPlanDbContext dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public DbContext DbContext { get; }

        public List<TEntity> GetList()
        {
            return DbContext.Set<TEntity>().ToList();
        }

        public List<TEntity> GetList(Expression<Func<TEntity, bool>> whereExpression)
        {
            if (whereExpression == null)
            {
                throw new ArgumentNullException(nameof(whereExpression));
            }

            var entities = DbContext.Set<TEntity>()
                .AsNoTracking()
                .Where(whereExpression)
                .ToList();
            return entities;
        }

        public PageResult<TEntity> GetList(Expression<Func<TEntity, bool>> whereExpression, int page, int size)
        {
            if (whereExpression == null)
            {
                throw new ArgumentNullException(nameof(whereExpression));
            }

            Debug.Assert(page > 0);
            Debug.Assert(size > 0);

            var offset = (page - 1) * size;
            var limit = size;

            var entities = DbContext.Set<TEntity>()
                .AsNoTracking()
                .Where(whereExpression)
                .Skip(offset)
                .Take(limit)
                .ToList();

            var totalCount = DbContext.Set<TEntity>()
                 .AsNoTracking()
                 .Count(whereExpression);

            return new PageResult<TEntity>(entities, totalCount);
        }

        public PageResult<TEntity> GetList(Expression<Func<TEntity, bool>> whereExpression, int page, int size, Expression<Func<TEntity, object>> orderByExpression, OrderByType orderByType)
        {
            if (whereExpression == null)
            {
                throw new ArgumentNullException(nameof(whereExpression));
            }

            Debug.Assert(page > 0);
            Debug.Assert(size > 0);

            var offset = (page - 1) * size;
            var limit = size;

            var entities = DbContext.Set<TEntity>()
                .AsNoTracking()
                .Where(whereExpression)
                .Skip(offset)
                .Take(limit);

            switch (orderByType)
            {
                case OrderByType.Asc:
                    entities = entities.OrderBy(orderByExpression);
                    break;
                case OrderByType.Desc:
                    entities = entities.OrderByDescending(orderByExpression);
                    break;
                default:
                    break;
            }

            var totalCount = DbContext.Set<TEntity>()
                 .AsNoTracking()
                 .Count(whereExpression);

            return new PageResult<TEntity>(entities.ToList(), totalCount);
        }

        public TEntity Get(object id)
        {
            var entity = DbContext.Find<TEntity>(id);
            return entity;
        }

        public TEntity Get(Expression<Func<TEntity, bool>> whereExpression)
        {
            if (whereExpression == null)
            {
                throw new ArgumentNullException(nameof(whereExpression));
            }

            var entity = DbContext.Set<TEntity>()
                .SingleOrDefault(whereExpression);
            return entity;
        }

        public void Add(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            DbContext.Add(entity);
            DbContext.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            DbContext.Update(entity);
            DbContext.SaveChanges();
        }

        public void Delete(object id)
        {
            var entity = DbContext.Find<TEntity>(id);
            if (entity == null)
            {
                return;
            }

            DbContext.Remove(entity);
            DbContext.SaveChanges();
        }

        public int Count()
        {
            var count = DbContext.Set<TEntity>()
                .AsNoTracking()
                .Count();
            return count;
        }

        public int Count(Expression<Func<TEntity, bool>> whereExpression)
        {
            if (whereExpression == null)
            {
                throw new ArgumentNullException(nameof(whereExpression));
            }

            var count = DbContext.Set<TEntity>()
                .AsNoTracking()
                .Count(whereExpression);
            return count;
        }

        public void UseTransaction(Action<ITransactionContext> action)
        {
            using (var transaction = DbContext.Database.BeginTransaction())
            {
                var transactionContext = new EFTransactionContext(transaction);
                action(transactionContext);
            }
        }

        public List<TResult> Join<TEntity1, TKey, TResult>(Expression<Func<TEntity, TKey>> entitySelector, Expression<Func<TEntity1, TKey>> entity1Selector, Expression<Func<TEntity, TEntity1, TResult>> resultSelector)
            where TEntity1 : class, new()
            where TResult : class, new()
        {
            if (entitySelector == null)
            {
                throw new ArgumentNullException(nameof(entitySelector));
            }

            if (entity1Selector == null)
            {
                throw new ArgumentNullException(nameof(entity1Selector));
            }

            if (resultSelector == null)
            {
                throw new ArgumentNullException(nameof(resultSelector));
            }

            var joinEntities = DbContext.Set<TEntity1>();
            var data = DbContext.Set<TEntity>()
                .Join(joinEntities, entitySelector, entity1Selector, resultSelector)
                .ToList();
            return data;
        }

        public List<TResult> Join<TEntity1, TKey, TResult>(Expression<Func<TEntity, TKey>> entitySelector, Expression<Func<TEntity1, TKey>> entity1Selector, Expression<Func<TResult, bool>> whereExpression, Expression<Func<TEntity, TEntity1, TResult>> resultSelector)
            where TEntity1 : class, new()
            where TResult : class, new()
        {
            if (entitySelector == null)
            {
                throw new ArgumentNullException(nameof(entitySelector));
            }

            if (entity1Selector == null)
            {
                throw new ArgumentNullException(nameof(entity1Selector));
            }

            if (whereExpression == null)
            {
                throw new ArgumentNullException(nameof(whereExpression));
            }

            if (resultSelector == null)
            {
                throw new ArgumentNullException(nameof(resultSelector));
            }

            var joinEntities = DbContext.Set<TEntity1>();
            var data = DbContext.Set<TEntity>()
                .Join(joinEntities, entitySelector, entity1Selector, resultSelector)
                .Where(whereExpression)
                .ToList();
            return data;
        }

        public PageResult<TResult> Join<TEntity1, TKey, TResult>(Expression<Func<TEntity, TKey>> entitySelector, Expression<Func<TEntity1, TKey>> entity1Selector, Expression<Func<TResult, bool>> whereExpression, int page, int size, Expression<Func<TEntity, TEntity1, TResult>> resultSelector)
            where TEntity1 : class, new()
            where TResult : class, new()
        {
            if (entitySelector == null)
            {
                throw new ArgumentNullException(nameof(entitySelector));
            }

            if (entity1Selector == null)
            {
                throw new ArgumentNullException(nameof(entity1Selector));
            }

            if (whereExpression == null)
            {
                throw new ArgumentNullException(nameof(whereExpression));
            }

            if (resultSelector == null)
            {
                throw new ArgumentNullException(nameof(resultSelector));
            }

            var joinEntities = DbContext.Set<TEntity1>();
            var totalCount = DbContext.Set<TEntity>()
                .Join(joinEntities, entitySelector, entity1Selector, resultSelector)
                .Count(whereExpression);

            Debug.Assert(page > 0);
            Debug.Assert(size > 0);

            var offset = (page - 1) * size;
            var limit = size;

            var data = DbContext.Set<TEntity>()
                .Join(joinEntities, entitySelector, entity1Selector, resultSelector)
                .Where(whereExpression)
                .Skip(offset)
                .Take(limit)
                .ToList();
            return new PageResult<TResult>(data, totalCount);
        }

        public List<TResult> FromSql<TResult>(string sql, params object[] parameters)
            where TResult : class, new()
        {
            return DbContext.Database.FromSql<TResult>(sql, parameters);
        }

        public async Task<List<TEntity>> GetListAsync()
        {
            var entities = await DbContext.Set<TEntity>().ToListAsync();
            return entities;
        }

        public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> whereExpression)
        {
            if (whereExpression == null)
            {
                throw new ArgumentNullException(nameof(whereExpression));
            }

            var entities = await DbContext.Set<TEntity>()
                .AsNoTracking()
                .Where(whereExpression)
                .ToListAsync();
            return entities;
        }

        public async Task<PageResult<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> whereExpression, int page, int size)
        {
            if (whereExpression == null)
            {
                throw new ArgumentNullException(nameof(whereExpression));
            }

            Debug.Assert(page > 0);
            Debug.Assert(size > 0);

            var offset = (page - 1) * size;
            var limit = size;

            var entities = await DbContext.Set<TEntity>()
                .AsNoTracking()
                .Where(whereExpression)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();

            var totalCount = DbContext.Set<TEntity>()
                 .AsNoTracking()
                 .Count(whereExpression);

            return new PageResult<TEntity>(entities, totalCount);
        }

        public async Task<PageResult<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> whereExpression, int page, int size, Expression<Func<TEntity, object>> orderByExpression, OrderByType orderByType)
        {
            if (whereExpression == null)
            {
                throw new ArgumentNullException(nameof(whereExpression));
            }

            if (orderByExpression == null)
            {
                throw new ArgumentNullException(nameof(orderByExpression));
            }

            Debug.Assert(page > 0);
            Debug.Assert(size > 0);

            var offset = (page - 1) * size;
            var limit = size;

            var entities = DbContext.Set<TEntity>()
                .AsNoTracking()
                .Where(whereExpression)
                .Skip(offset)
                .Take(limit);

            switch (orderByType)
            {
                case OrderByType.Asc:
                    entities = entities.OrderBy(orderByExpression);
                    break;
                case OrderByType.Desc:
                    entities = entities.OrderByDescending(orderByExpression);
                    break;
                default:
                    break;
            }

            var totalCount = await DbContext.Set<TEntity>()
                 .AsNoTracking()
                 .CountAsync(whereExpression);

            return new PageResult<TEntity>(await entities.ToListAsync(), totalCount);
        }

        public async Task<TEntity> GetAsync(object id)
        {
            var entity = await DbContext.FindAsync<TEntity>(id);
            return entity;
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> whereExpression)
        {
            if (whereExpression == null)
            {
                throw new ArgumentNullException(nameof(whereExpression));
            }

            var entity = await DbContext.Set<TEntity>()
                .AsNoTracking()
                .SingleOrDefaultAsync(whereExpression);
            return entity;
        }

        public async Task AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            DbContext.Add(entity);
            await DbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            DbContext.Update(entity);
            await DbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(object id)
        {
            var entity = DbContext.Find<TEntity>(id);
            if (entity == null)
            {
                return;
            }

            DbContext.Remove(entity);
            await DbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            DbContext.Remove(entity);
            await DbContext.SaveChangesAsync();
        }

        public async Task UseTransactionAsync(Action<ITransactionContext> action)
        {
            using (var transaction = await DbContext.Database.BeginTransactionAsync())
            {
                var transactionContext = new EFTransactionContext(transaction);
                action(transactionContext);
            }
        }

        public async Task<int> CountAsync()
        {
            var count = await DbContext.Set<TEntity>()
                .AsNoTracking()
                .CountAsync();
            return count;
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> whereExpression)
        {
            if (whereExpression == null)
            {
                throw new ArgumentNullException(nameof(whereExpression));
            }

            var count = await DbContext.Set<TEntity>()
                .AsNoTracking()
                .CountAsync(whereExpression);
            return count;
        }

        public async Task<List<TResult>> JoinAsync<TEntity1, TKey, TResult>(Expression<Func<TEntity, TKey>> entitySelector, Expression<Func<TEntity1, TKey>> entity1Selector, Expression<Func<TEntity, TEntity1, TResult>> resultSelector)
            where TEntity1 : class, new()
            where TResult : class, new()
        {
            if (entitySelector == null)
            {
                throw new ArgumentNullException(nameof(entitySelector));
            }

            if (entity1Selector == null)
            {
                throw new ArgumentNullException(nameof(entity1Selector));
            }

            if (resultSelector == null)
            {
                throw new ArgumentNullException(nameof(resultSelector));
            }

            var joinEntities = DbContext.Set<TEntity1>();
            var data = await DbContext.Set<TEntity>()
                .Join(joinEntities, entitySelector, entity1Selector, resultSelector)
                .ToListAsync();
            return data;
        }

        public async Task<List<TResult>> JoinAsync<TEntity1, TKey, TResult>(Expression<Func<TEntity, TKey>> entitySelector, Expression<Func<TEntity1, TKey>> entity1Selector, Expression<Func<TResult, bool>> whereExpression, Expression<Func<TEntity, TEntity1, TResult>> resultSelector)
            where TEntity1 : class, new()
            where TResult : class, new()
        {
            if (entitySelector == null)
            {
                throw new ArgumentNullException(nameof(entitySelector));
            }

            if (entity1Selector == null)
            {
                throw new ArgumentNullException(nameof(entity1Selector));
            }

            if (whereExpression == null)
            {
                throw new ArgumentNullException(nameof(whereExpression));
            }

            if (resultSelector == null)
            {
                throw new ArgumentNullException(nameof(resultSelector));
            }

            var joinEntities = DbContext.Set<TEntity1>();
            var data = await DbContext.Set<TEntity>()
                .Join(joinEntities, entitySelector, entity1Selector, resultSelector)
                .Where(whereExpression)
                .ToListAsync();
            return data;
        }

        public async Task<PageResult<TResult>> JoinAsync<TEntity1, TKey, TResult>(Expression<Func<TEntity, TKey>> entitySelector, Expression<Func<TEntity1, TKey>> entity1Selector, Expression<Func<TResult, bool>> whereExpression, int page, int size, Expression<Func<TEntity, TEntity1, TResult>> resultSelector)
            where TEntity1 : class, new()
            where TResult : class, new()
        {
            if (entitySelector == null)
            {
                throw new ArgumentNullException(nameof(entitySelector));
            }

            if (entity1Selector == null)
            {
                throw new ArgumentNullException(nameof(entity1Selector));
            }

            if (whereExpression == null)
            {
                throw new ArgumentNullException(nameof(whereExpression));
            }

            if (resultSelector == null)
            {
                throw new ArgumentNullException(nameof(resultSelector));
            }

            var joinEntities = DbContext.Set<TEntity1>();
            var totalCount = await DbContext.Set<TEntity>()
                .Join(joinEntities, entitySelector, entity1Selector, resultSelector)
                .CountAsync(whereExpression);

            Debug.Assert(page > 0);
            Debug.Assert(size > 0);

            var offset = (page - 1) * size;
            var limit = size;

            var data = await DbContext.Set<TEntity>()
                .Join(joinEntities, entitySelector, entity1Selector, resultSelector)
                .Where(whereExpression)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();
            return new PageResult<TResult>(data, totalCount);
        }

        public Task<List<TResult>> FromSqlAsync<TResult>(string sql, params object[] parameters) where TResult : class, new()
        {
            return DbContext.Database.FromSqlAsync<TResult>(sql, parameters);
        }
    }
}
