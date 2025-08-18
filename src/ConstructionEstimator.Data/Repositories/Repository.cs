using Microsoft.EntityFrameworkCore;
using ConstructionEstimator.Core.Interfaces;
using ConstructionEstimator.Data.Context;

namespace ConstructionEstimator.Data.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly ConstructionEstimatorDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(ConstructionEstimatorDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        return entity;
    }

    public virtual Task<bool> UpdateAsync(T entity)
    {
        try
        {
            _dbSet.Update(entity);
            return Task.FromResult(true);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }

    public virtual Task<bool> DeleteAsync(int id)
    {
        try
        {
            var entity = _dbSet.Find(id);
            if (entity == null) return Task.FromResult(false);
            
            _dbSet.Remove(entity);
            return Task.FromResult(true);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }

    public virtual Task<bool> ExistsAsync(int id)
    {
        var entity = _dbSet.Find(id);
        return Task.FromResult(entity != null);
    }
}