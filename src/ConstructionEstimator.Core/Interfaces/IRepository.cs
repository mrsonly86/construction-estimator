namespace ConstructionEstimator.Core.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate);
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<int> CountAsync();
}

public interface IUnitOfWork : IDisposable
{
    IProjectRepository Projects { get; }
    IMaterialRepository Materials { get; }
    ILaborRepository Labor { get; }
    IEquipmentRepository Equipment { get; }
    IStandardRepository Standards { get; }
    IPriceListRepository PriceLists { get; }
    IEstimateItemRepository EstimateItems { get; }
    ICategoryRepository Categories { get; }
    
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}