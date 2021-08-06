using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TimeCardServices.Repository
{
    public interface IRepository<TEntity>
    {

        Task<TEntity> GetByIdAsync(object id);

        IQueryable<TEntity> SearchFor(Expression<Func<TEntity, bool>> predicate);

        IQueryable<TEntity> GetAll();

        TEntity GetById(object id);
        int  Edit(TEntity entity);

        int  Edit(TEntity entity, Expression<Func<TEntity, string[]>> predicate);

        int Insert(TEntity entity);

        int Delete(TEntity entity);
        int Delete(Expression<Func<TEntity, bool>> predicate);

        Task EditAsync(TEntity entity);

        Task EditAsync(TEntity entity, Expression<Func<TEntity, string[]>> predicate);

        Task InsertAsync(TEntity entity);

        Task DeleteAsync(TEntity entity);
        Task DeleteAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
