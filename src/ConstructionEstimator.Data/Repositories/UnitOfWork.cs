using Microsoft.EntityFrameworkCore.Storage;
using ConstructionEstimator.Core.Interfaces;
using ConstructionEstimator.Data.Context;

namespace ConstructionEstimator.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ConstructionEstimatorDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(ConstructionEstimatorDbContext context)
    {
        _context = context;
        Projects = new ProjectRepository(_context);
        Materials = new MaterialRepository(_context);
        Labor = new LaborRepository(_context);
        Equipment = new EquipmentRepository(_context);
        Standards = new StandardRepository(_context);
        PriceLists = new PriceListRepository(_context);
        EstimateItems = new EstimateItemRepository(_context);
        Categories = new CategoryRepository(_context);
    }

    public IProjectRepository Projects { get; }
    public IMaterialRepository Materials { get; }
    public ILaborRepository Labor { get; }
    public IEquipmentRepository Equipment { get; }
    public IStandardRepository Standards { get; }
    public IPriceListRepository PriceLists { get; }
    public IEstimateItemRepository EstimateItems { get; }
    public ICategoryRepository Categories { get; }

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
        _context?.Dispose();
    }
}