using AXIOCRM.Domain.Entities;

namespace AXIOCRM.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task SaveResetTokenAsync(Guid userId, string token);
    }
}
