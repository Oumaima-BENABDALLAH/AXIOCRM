using System.Net;
using System.Web;
using AXIOCRM.Application.Interfaces;
using AXIOCRM.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;


namespace AXIOCRM.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IEmailService _emailService;
        private readonly UserManager<ApplicationUser> _userManager;
      
        public AuthService(IEmailService emailService, UserManager<ApplicationUser> userManager)
        {
            _emailService = emailService;
            _userManager = userManager;
            
        }

        public async Task SendResetLinkAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return;
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var safeToken = token.Replace("+", "-").Replace("/", "_");
            var resetLink = $"http://localhost:4200/reset-password?email={HttpUtility.UrlEncode(email)}&token={safeToken}";
            await _emailService.SendAsync(
                email,
                "Password reset",
                $"Click this link to reset your password: {resetLink}"
            );
        }

        public async Task<bool> ResetPasswordAsync(string email, string token, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return false;
            var originalToken = token.Replace("-", "+").Replace("_", "/");
            var result = await _userManager.ResetPasswordAsync(user, originalToken, newPassword);
            return result.Succeeded;
        }

    }

}