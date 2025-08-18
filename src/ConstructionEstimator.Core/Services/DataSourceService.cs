using ConstructionEstimator.Core.Entities;
using ConstructionEstimator.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace ConstructionEstimator.Core.Services;

public class DataSourceService : IDataSourceService
{
    private readonly IDataSourceRepository _dataSourceRepository;
    private readonly ILogger<DataSourceService> _logger;

    public DataSourceService(IDataSourceRepository dataSourceRepository, ILogger<DataSourceService> logger)
    {
        _dataSourceRepository = dataSourceRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<DataSource>> GetDataSourcesByProvinceAsync(int provinceId)
    {
        try
        {
            _logger.LogInformation("Retrieving data sources for province {ProvinceId}", provinceId);
            return await _dataSourceRepository.GetDataSourcesByProvinceAsync(provinceId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving data sources for province {ProvinceId}", provinceId);
            throw;
        }
    }

    public async Task<DataSource?> GetDataSourceAsync(int id)
    {
        try
        {
            _logger.LogInformation("Retrieving data source with ID: {DataSourceId}", id);
            return await _dataSourceRepository.GetByIdAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving data source with ID: {DataSourceId}", id);
            throw;
        }
    }

    public async Task<DataSource> CreateDataSourceAsync(DataSource dataSource)
    {
        try
        {
            _logger.LogInformation("Creating new data source: {DataSourceName} for province {ProvinceId}", 
                dataSource.Name, dataSource.ProvinceId);
            
            dataSource.CreatedDate = DateTime.Now;
            dataSource.LastUpdated = DateTime.Now;
            dataSource.NextScanDate = DateTime.Now.AddDays(dataSource.UpdateFrequencyDays);
            
            var createdDataSource = await _dataSourceRepository.AddAsync(dataSource);
            await _dataSourceRepository.SaveChangesAsync();
            
            _logger.LogInformation("Successfully created data source: {DataSourceName} with ID: {DataSourceId}", 
                dataSource.Name, createdDataSource.Id);
            return createdDataSource;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating data source: {DataSourceName}", dataSource.Name);
            throw;
        }
    }

    public async Task<DataSource> UpdateDataSourceAsync(DataSource dataSource)
    {
        try
        {
            _logger.LogInformation("Updating data source: {DataSourceName} (ID: {DataSourceId})", 
                dataSource.Name, dataSource.Id);
            
            dataSource.LastUpdated = DateTime.Now;
            await _dataSourceRepository.UpdateAsync(dataSource);
            await _dataSourceRepository.SaveChangesAsync();
            
            _logger.LogInformation("Successfully updated data source: {DataSourceName}", dataSource.Name);
            return dataSource;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating data source: {DataSourceName} (ID: {DataSourceId})", 
                dataSource.Name, dataSource.Id);
            throw;
        }
    }

    public async Task DeleteDataSourceAsync(int id)
    {
        try
        {
            _logger.LogInformation("Deleting data source with ID: {DataSourceId}", id);
            await _dataSourceRepository.DeleteAsync(id);
            await _dataSourceRepository.SaveChangesAsync();
            _logger.LogInformation("Successfully deleted data source with ID: {DataSourceId}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting data source with ID: {DataSourceId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<DataSource>> GetActiveDataSourcesAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving all active data sources");
            return await _dataSourceRepository.GetActiveDataSourcesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving active data sources");
            throw;
        }
    }

    public async Task<IEnumerable<DataSource>> GetDataSourcesDueForUpdateAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving data sources due for update");
            return await _dataSourceRepository.GetDataSourcesDueForUpdateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving data sources due for update");
            throw;
        }
    }
}