using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TimeCardServices.Model;

namespace TimeCardServices.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected DbSet<TEntity> DbSet;

        private readonly TimeCardDBContext _dbContext;

        public Repository(TimeCardDBContext dbContext)
        {
            _dbContext = dbContext;
            DbSet = _dbContext.Set<TEntity>();
        }

        public IQueryable<TEntity> GetAll()
        {
            return DbSet;
        }

        public TEntity GetById(object id)
        {
            return  DbSet.Find(id);
        }

        public async Task<TEntity> GetByIdAsync(object id)
        {
            return await DbSet.FindAsync(id);
        }

        public IQueryable<TEntity> SearchFor(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }

        public async Task EditAsync(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task EditAsync(TEntity entity, Expression<Func<TEntity, string[]>> path)
        {

            EntityEntry<TEntity> entry = _dbContext.Entry<TEntity>(entity);
            DbSet.Attach(entity);
            //**如果使用 Entry 附加 实体对象到数据容器中，则需要手动 设置 实体包装类的对象 的 状态为 Unchanged**  
            //**如果使用 Attach 就不需要这句  
            //entry.State = EntityState.Detached;
            entry.State = EntityState.Unchanged;
            //0.2标识 实体对象 某些属性 已经被修改了 

            System.Linq.Expressions.NewArrayExpression Allpro = path.Body as NewArrayExpression;

            foreach (Expression pro in Allpro.Expressions)
            {
                string FieldName = "";
                MemberExpression proInner = pro as MemberExpression;
                if (proInner != null)
                {
                    FieldName = proInner.Member.Name;
                    entry.Property(FieldName).IsModified = true;
                }
                else
                {
                    MethodCallExpression proInnerItem = pro as MethodCallExpression;
                    if (proInnerItem != null)
                    {
                        FieldName = (proInnerItem.Object as MemberExpression).Member.Name;
                        entry.Property(FieldName).IsModified = true;
                    }
                }

            }
            //entry.Property("ATitle").IsModified = true;  
            //entry.Property("AContent").IsModified = true;

            await _dbContext.SaveChangesAsync();
        }

        public async Task InsertAsync(TEntity entity)
        {

            DbSet.Add(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            if (typeof(TEntity) == typeof(User))
            {
                //Delete all timecard of the deleting user 
                var ModelsDeleting = _dbContext.TimeCards.Where(f => f.UserName == (entity as User).UserName);
                foreach (var item in ModelsDeleting)
                {
                    _dbContext.TimeCards.Remove(item);
                }
                DbSet.Remove(entity);
            }
            else
            {
                DbSet.Remove(entity);
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {

            var ModelsDeleting = DbSet.Where(predicate);
            foreach (var item in ModelsDeleting)
            {
                DbSet.Remove(item);
            }
            await _dbContext.SaveChangesAsync();
        }


        public int  Edit(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
           return  _dbContext.SaveChanges();
        }

        public int Edit(TEntity entity, Expression<Func<TEntity, string[]>> path)
        {

            EntityEntry<TEntity> entry = _dbContext.Entry<TEntity>(entity);
            DbSet.Attach(entity);
            //**如果使用 Entry 附加 实体对象到数据容器中，则需要手动 设置 实体包装类的对象 的 状态为 Unchanged**  
            //**如果使用 Attach 就不需要这句  
            //entry.State = EntityState.Detached;
            entry.State = EntityState.Unchanged;
            //0.2标识 实体对象 某些属性 已经被修改了 

            System.Linq.Expressions.NewArrayExpression Allpro = path.Body as NewArrayExpression;

            foreach (Expression pro in Allpro.Expressions)
            {
                string FieldName = "";
                MemberExpression proInner = pro as MemberExpression;
                if (proInner != null)
                {
                    FieldName = proInner.Member.Name;
                    entry.Property(FieldName).IsModified = true;
                }
                else
                {
                    MethodCallExpression proInnerItem = pro as MethodCallExpression;
                    if (proInnerItem != null)
                    {
                        FieldName = (proInnerItem.Object as MemberExpression).Member.Name;
                        entry.Property(FieldName).IsModified = true;
                    }
                }

            }
            //entry.Property("ATitle").IsModified = true;  
            //entry.Property("AContent").IsModified = true;

            return _dbContext.SaveChanges();
        }

        public int Insert(TEntity entity)
        {

            DbSet.Add(entity);
            return  _dbContext.SaveChanges();
        }

        public int Delete(TEntity entity)
        {
            if (typeof(TEntity) == typeof(User))
            {
                //Delete all timecard of the deleting user 
                var ModelsDeleting = _dbContext.TimeCards.Where(f => f.UserName == (entity as User).UserName);
                foreach (var item in ModelsDeleting)
                {
                    _dbContext.TimeCards.Remove(item);
                }
                DbSet.Remove(entity);
            }
            else
            {
                DbSet.Remove(entity);
            }
            return _dbContext.SaveChanges();
        }

        public int Delete(Expression<Func<TEntity, bool>> predicate)
        {

            var ModelsDeleting = DbSet.Where(predicate);
            foreach (var item in ModelsDeleting)
            {
                DbSet.Remove(item);
            }
            return _dbContext.SaveChanges();
        }
    }
}
