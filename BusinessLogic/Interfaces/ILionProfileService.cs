using BusinessObject.Models;

namespace BusinessLogic.Interfaces
{
    public interface ILionProfileService
    {
        Task<(List<LionProfile> List, int TotalCount)> GetAllAsync(int pageIndex, int pageSize, string? searchTerm, int? weight);
        Task<LionProfile?> GetByIdAsync(int id);
        Task<LionProfile?> AddAsync(LionProfile item);
        Task<bool> UpdateAsync(LionProfile item);
        Task<bool> DeleteAsync(int Id);
        Task<List<LionType>> GetLionTypesAsync();
        Task<LionType?> GetLionTypeByIdAsync(int id);
    }
}
