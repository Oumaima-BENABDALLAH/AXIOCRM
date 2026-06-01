using Microsoft.AspNetCore.Identity;

namespace AXIOCRM.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? Role { get; set; }
        public string? FullName { get; set; }
        public string? ProfilePictureUrl { get; set; }
    }

}
