using Library_Management_System.Common.Exceptions;
using Library_Management_System.Data;
using Library_Management_System.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Library_Management_System.Repositories
{
    public class Repository<TEntity>(LibraryDbContext context) : IRepository<TEntity>
        where TEntity : class
    {
        private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

        #region Query

        public IQueryable<TEntity> Query()
        {
            return _dbSet.AsQueryable();
        }

        public async Task<TEntity?> GetByIdAsync(params object[] keyValues)
        {
            return await _dbSet.FindAsync(keyValues);
        }

        #endregion

        #region Commands

        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlException)
            {
                throw MapSqlException(sqlException);
            }
        }

        #endregion

        #region Helpers

        private static Exception MapSqlException(SqlException sqlException)
        {
            return sqlException.Number switch
            {
                2601 or 2627 => new DuplicateKeyException("A record with the same unique value already exists.", sqlException.Number),
                547 => new ForeignKeyViolationException("The record cannot be saved because related data is missing or in use.", sqlException.Number),
                -2 => new DatabaseTimeoutException("The database request timed out.", sqlException.Number),
                _ => new DatabaseServerException("A database error occurred.")
            };
        }

        #endregion
    }
}
