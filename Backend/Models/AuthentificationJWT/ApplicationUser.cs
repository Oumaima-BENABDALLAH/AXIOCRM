using Microsoft.AspNetCore.Identity;

namespace ProductManager.API.Models.AuthentificationJWT
{
    public class ApplicationUser : IdentityUser
    {
        public string Role { get; set; }
    }

}
