namespace ProductManager.API.Models.AuthentificationJWT
{
    public class ResetPassword
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}
