using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using IMSServer.Models;

namespace IMSServer.Repositories
{
    interface IRepository<TEntity> where TEntity : ModelBase
    {
        TEntity Get(long id);

        Task<TEntity> FindAsync(long id);

        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate);

        //Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate);

        IEnumerable<TEntity> GetAll();

        //Task<IEnumerable<TEntity>> GetAllAsync();

        TEntity GetFirst(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> predicate);

        TEntity Add(TEntity entity);

        Task<TEntity> AddAsync(TEntity entity);

        TEntity Remove(long id);

        Task<TEntity> RemoveAsync(long id);

        TEntity Update(TEntity entity);

        Task<TEntity> UpdateAsync(TEntity entity);
    }
}
