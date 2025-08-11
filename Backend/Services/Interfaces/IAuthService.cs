namespace ProductManager.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<bool> ResetPasswordAsync(string email, string token, string newPassword);
        Task SendResetLinkAsync(string email);
         

    }
}
