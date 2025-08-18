using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ConstructionEstimator.Core.Interfaces;
using ConstructionEstimator.Core.Entities;
using ConstructionEstimator.Data.Context;

namespace ConstructionEstimator.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ConstructionEstimatorDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(ConstructionEstimatorDbContext context)
    {
        _context = context;
        Projects = new Repository<Project>(_context);
        Materials = new Repository<Material>(_context);
        MaterialPrices = new Repository<MaterialPrice>(_context);
        Provinces = new Repository<Province>(_context);
        ProvinceConfigs = new Repository<ProvinceConfig>(_context);
        PriceHistories = new Repository<PriceHistory>(_context);
    }

    public IRepository<Project> Projects { get; }
    public IRepository<Material> Materials { get; }
    public IRepository<MaterialPrice> MaterialPrices { get; }
    public IRepository<Province> Provinces { get; }
    public IRepository<ProvinceConfig> ProvinceConfigs { get; }
    public IRepository<PriceHistory> PriceHistories { get; }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}