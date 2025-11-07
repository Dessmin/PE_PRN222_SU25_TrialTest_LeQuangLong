using BusinessObject.Models;

namespace BusinessLogic.Interfaces
{
    public interface IAuthService
    {
        Task<LionAccount?> LoginAsync(string email, string password);
    }
}
