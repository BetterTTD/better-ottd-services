using System.Linq.Expressions;

namespace DataAccess.Abstractions;

public interface IGenericRepository<TEntity, in TKey> where TEntity : Entity<TKey>
{
    Task<IReadOnlyList<TEntity>> GetAllAsync();
    Task<IReadOnlyList<TEntity>> GetAllByAsync(Expression<Func<TEntity, bool>> query);
    Task<TEntity?> FindAsync(TKey key);
    Task<TEntity?> FindByAsync(Expression<Func<TEntity, bool>> query);
    Task<TEntity> CreateAsync(TEntity entity);
    void UpdateAsync(TEntity entity);
    void DeleteAsync(TEntity entity);
    Task SaveAsync();
}