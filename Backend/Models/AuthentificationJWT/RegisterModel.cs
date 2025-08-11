namespace ProductManager.API.Models.AuthentificationJWT
{
    public class RegisterModel
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; }
    }
}
