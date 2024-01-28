using System.Linq.Expressions;
using DataAccess.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public sealed class GenericGuidRepository<TEntity, TKey>(DbContext db) : IGenericRepository<TEntity, TKey>
    where TEntity : Entity<TKey>
{
    private readonly DbSet<TEntity> _set = db.Set<TEntity>();

    public async Task<IReadOnlyList<TEntity>> GetAllAsync()
    {
        var items = await _set.ToListAsync();
        return items.AsReadOnly();
    }

    public async Task<IReadOnlyList<TEntity>> GetAllByAsync(Expression<Func<TEntity, bool>> query)
    {
        var items = await _set.Where(query).ToListAsync();
        return items.AsReadOnly();
    }

    public async Task<TEntity?> FindAsync(TKey key)
    {
        return await _set.FindAsync(key);
    }

    public async Task<TEntity?> FindByAsync(Expression<Func<TEntity, bool>> query)
    {
        return await _set.SingleOrDefaultAsync(query);
    }

    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        await _set.AddAsync(entity);
        return entity;
    }

    public void Update(TEntity entity)
    {
        _set.Attach(entity);
        db.Entry(entity).State = EntityState.Modified;
    }

    public void Delete(TEntity entity)
    {
        _set.Attach(entity);
        _set.Remove(entity);
    }

    public async Task SaveAsync()
    {
        await db.SaveChangesAsync();
    }
}