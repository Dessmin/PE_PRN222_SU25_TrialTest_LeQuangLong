using BusinessLogic.Interfaces;
using BusinessObject.Models;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Services
{
    public class LionProfileService : ILionProfileService
    {
        private readonly ILogger _logger;
        private readonly IGenericRepository<LionProfile> _lionProfileRepository;
        private readonly IGenericRepository<LionType> _lionTypeRepository;

        public LionProfileService(ILogger<LionProfileService> logger)
        {
            _logger = logger;
            _lionProfileRepository ??= new GenericRepository<LionProfile>();
            _lionTypeRepository ??= new GenericRepository<LionType>();
        }

        public async Task<(List<LionProfile> List, int TotalCount)> GetAllAsync(int pageIndex, int pageSize, string? searchTerm, int? weight)
        {
            try
            {
                _logger.LogInformation($"Fetching with search term: {searchTerm}");
                var item = _lionProfileRepository.GetAllAsQueryable().Include(i => i.LionType).AsQueryable();

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    item = item.Where(e => e.LionType.LionTypeName.ToLower().Contains(searchTerm.ToLower()));
                }
                if (weight.HasValue)
                {
                    item = item.Where(e => e.Weight == weight.Value);
                }
                var items = await item
                    .Include(e => e.LionType)
                    .OrderByDescending(e => e.ModifiedDate)
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var totalCount = await item.CountAsync();

                _logger.LogInformation($"Fetched {items.Count}");
                return (items, totalCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching.");
                throw;
            }
        }

        public async Task<LionProfile?> GetByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching with ID: {id}");
                var item = await _lionProfileRepository.GetByIdAsync(id);
                if (item == null)
                {
                    _logger.LogWarning($"ID: {id} not found.");
                }
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching with ID: {id}");
                throw;
            }
        }

        public async Task<LionProfile?> AddAsync(LionProfile item)
        {
            try
            {
                _logger.LogInformation($"Adding new Id: {item.LionName}");

                await _lionProfileRepository.CreateAsync(item);

                _logger.LogInformation("Added successfully.");
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding new Id.");
                return null;
            }
        }

        public async Task<bool> UpdateAsync(LionProfile item)
        {
            try
            {
                _logger.LogInformation($"Updating with ID: {item.LionProfileId}");
                await _lionProfileRepository.UpdateAsync(item);
                _logger.LogInformation("Updated successfully.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating with ID: {item.LionProfileId}");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int Id)
        {
            try
            {
                _logger.LogInformation($"Deleting with ID: {Id}");
                var item = await _lionProfileRepository.GetByIdAsync(Id);

                await _lionProfileRepository.RemoveAsync(item);
                _logger.LogInformation("Deleted successfully.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting with ID: {Id}");
                return false;
            }
        }

        public async Task<List<LionType>> GetLionTypesAsync()
        {
            try
            {
                _logger.LogInformation("Fetching Lion Types");
                var items = await _lionTypeRepository.GetAllAsQueryable().ToListAsync();
                _logger.LogInformation($"Fetched {items.Count} Lion Types");
                return items;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching Lion Types.");
                throw;
            }
        }
        public async Task<LionType?> GetLionTypeByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching Lion Type with ID: {id}");
                var item = await _lionTypeRepository.GetByIdAsync(id);
                if (item == null)
                {
                    _logger.LogWarning($"Lion Type with ID: {id} not found.");
                }
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching Lion Type with ID: {id}");
                throw;
            }
        }
    }
}
