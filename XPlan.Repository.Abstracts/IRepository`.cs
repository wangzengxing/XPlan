using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace XPlan.Repository.Abstracts
{
    public interface IRepository<TEntity>
        where TEntity : class, new()
    {
        List<TEntity> GetList();
        List<TEntity> GetList(Expression<Func<TEntity, bool>> whereExpression);
        Page<TEntity> GetList(Expression<Func<TEntity, bool>> whereExpression, int page, int size);
        Page<TEntity> GetList(Expression<Func<TEntity, bool>> whereExpression, int page, int size, Expression<Func<TEntity, object>> orderByExpression, OrderByType orderByType);
        TEntity Get(object id);
        TEntity Get(Expression<Func<TEntity, bool>> whereExpression);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(object id);
        int Count();
        int Count(Expression<Func<TEntity, bool>> whereExpression);
        List<TResult> Join<TEntity1, TKey, TResult>(Expression<Func<TEntity, TKey>> entitySelector, Expression<Func<TEntity1, TKey>> entity1Selector, Expression<Func<TEntity, TEntity1, TResult>> resultSelector)
            where TEntity1 : class, new()
            where TResult : class, new();
        List<TResult> Join<TEntity1, TKey, TResult>(Expression<Func<TEntity, TKey>> entitySelector, Expression<Func<TEntity1, TKey>> entity1Selector, Expression<Func<TResult, bool>> whereExpression, Expression<Func<TEntity, TEntity1, TResult>> resultSelector)
            where TEntity1 : class, new()
            where TResult : class, new();
        Page<TResult> Join<TEntity1, TKey, TResult>(Expression<Func<TEntity, TKey>> entitySelector, Expression<Func<TEntity1, TKey>> entity1Selector, Expression<Func<TResult, bool>> whereExpression, int page, int size, Expression<Func<TEntity, TEntity1, TResult>> resultSelector)
            where TEntity1 : class, new()
            where TResult : class, new();
        List<TResult> FromSql<TResult>(string sql, params object[] parameters)
            where TResult : class, new();
        void UseTransaction(Action<ITransactionContext> action);
        Task<List<TEntity>> GetListAsync();
        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> whereExpression);
        Task<Page<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> whereExpression, int page, int size);
        Task<Page<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> whereExpression, int page, int size, Expression<Func<TEntity, object>> orderByExpression, OrderByType orderByType);
        Task<TEntity> GetAsync(object id);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> whereExpression);
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(object id);
        Task DeleteAsync(TEntity entity);
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<TEntity, bool>> whereExpression);
        Task UseTransactionAsync(Action<ITransactionContext> action);
        Task<List<TResult>> JoinAsync<TEntity1, TKey, TResult>(Expression<Func<TEntity, TKey>> entitySelector, Expression<Func<TEntity1, TKey>> entity1Selector, Expression<Func<TEntity, TEntity1, TResult>> resultSelector)
            where TEntity1 : class, new()
            where TResult : class, new();
        Task<List<TResult>> JoinAsync<TEntity1, TKey, TResult>(Expression<Func<TEntity, TKey>> entitySelector, Expression<Func<TEntity1, TKey>> entity1Selector, Expression<Func<TResult, bool>> whereExpression, Expression<Func<TEntity, TEntity1, TResult>> resultSelector)
             where TEntity1 : class, new()
             where TResult : class, new();
        Task<Page<TResult>> JoinAsync<TEntity1, TKey, TResult>(Expression<Func<TEntity, TKey>> entitySelector, Expression<Func<TEntity1, TKey>> entity1Selector, Expression<Func<TResult, bool>> whereExpression, int page, int size, Expression<Func<TEntity, TEntity1, TResult>> resultSelector)
            where TEntity1 : class, new()
            where TResult : class, new();
        Task<List<TResult>> FromSqlAsync<TResult>(string sql, params object[] parameters)
            where TResult : class, new();
    }
}
