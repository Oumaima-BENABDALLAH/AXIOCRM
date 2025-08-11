using ProductManager.API.Models.AuthentificationJWT;

namespace ProductManager.API.Services.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task SaveResetTokenAsync(Guid userId, string token);
    }
}
