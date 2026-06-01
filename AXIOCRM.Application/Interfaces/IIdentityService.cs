using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AXIOCRM.Application.Interfaces
{
    public interface IIdentityService
    {
        Task<string?> GetCurrentUserIdAsync();

        Task<bool> IsInRoleAsync(string userId, string role);

        //Task<List<UserDto>> GetUsersInRoleAsync(string role);
    }
}
