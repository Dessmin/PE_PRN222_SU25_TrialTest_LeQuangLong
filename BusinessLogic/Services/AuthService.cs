using BusinessLogic.Interfaces;
using BusinessObject.Models;
using DataAccess;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Services
{
    public class AuthService : IAuthService
    {
        private readonly ILogger _logger;
        private readonly IGenericRepository<LionAccount> _accountRepository;

        public AuthService(ILogger<AuthService> logger)
        {
            _logger = logger;
            _accountRepository ??= new GenericRepository<LionAccount>();
        }

        public async Task<LionAccount?> LoginAsync(string email, string password)
        {
            try
            {
                _logger.LogInformation($"Attempting login for user {email}.");

                var user = await _accountRepository.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
                if (user != null)
                {
                    _logger.LogInformation($"User {email} logged in successfully.");
                    return user;
                }
                else
                {
                    _logger.LogWarning($"Login failed for user {email}. Invalid credentials or inactive account.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred during login for user {email}.");
                throw;
            }
        }
    }
}
