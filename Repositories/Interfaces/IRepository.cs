using System.Linq.Expressions;

namespace Library_Management_System.Repositories.Interfaces
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        #region Query

        IQueryable<TEntity> Query();

        Task<TEntity?> GetByIdAsync(params object[] keyValues);

        #endregion

        #region Commands

        Task AddAsync(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);

        Task<int> SaveChangesAsync();

        #endregion
    }
}
