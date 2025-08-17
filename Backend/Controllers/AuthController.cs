using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProductManager.API.Models.AuthentificationJWT;
using ProductManager.API.Services.Interfaces;

namespace ProductManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        // GET: AuthController

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;

        public AuthController (UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, IEmailService emailService, IAuthService authService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _emailService = emailService;
            _authService = authService;
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email , Role = model.Role };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, user.Role);
                return Ok(new { Message = "Role  registered successfully." });
            }
            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var user = await _userManager.FindByEmailAsync(model.Email);

            if(user == null)
                return Unauthorized("Invalid User !!!");
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            if (!result.Succeeded)   return Unauthorized("Incorrect ¨Password");
            var token = await GenerateJwtToken(user);
            return Ok(new { Token = token });

        }

        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
            var claims = new List<Claim>
                  {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("fullName", user.FullName ?? ""),
            new Claim("profilePictureUrl", user.ProfilePictureUrl ?? "")
    };
            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["ExpiresInMinutes"])),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPassword model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest("User not found");

            await _authService.SendResetLinkAsync(model.Email);
            return Ok("Reset link sent");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPassword model)
        {
            // Logging pour le débogage (optionnel)
            Console.WriteLine("Token reçu dans le contrôleur : " + model.Token);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            //var decodedToken = System.Net.WebUtility.UrlDecode(model.Token);

            var result = await _authService.ResetPasswordAsync(model.Email, model.Token, model.NewPassword);

            if (!result)
                return BadRequest("Le lien est invalide ou a expiré.");

            return Ok("Mot de passe réinitialisé avec succès.");
        }
        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest request)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { "1018179724908-d7l8b3kcqgubkg6vnco23jq0g9igrh23.apps.googleusercontent.com" }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, settings);
                var user = await _userManager.FindByEmailAsync(payload.Email);
                if( user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = payload.Email,
                        Email = payload.Email,
                        FullName = payload.Name, 
                        ProfilePictureUrl = payload.Picture, 
                        Role = "User"

                    };
                    var createResult = await _userManager.CreateAsync(user);
                    if (!createResult.Succeeded)
                    {
                        return BadRequest(createResult.Errors);
                    }


                    await _userManager.AddToRoleAsync(user, user.Role);
                }

                else
                {
                    // Mettre à jour la photo si elle a changé
                    if (user.ProfilePictureUrl != payload.Picture)
                    {
                        user.ProfilePictureUrl = payload.Picture;
                        await _userManager.UpdateAsync(user);
                    }
                }

                    var token = await GenerateJwtToken(user);

                return Ok(new { 
                    token,
                    fullName = user.FullName,
                    profilePictureUrl = user.ProfilePictureUrl

                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Invalid Google token", error = ex.Message });
            }
        }
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                         User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound();

            return Ok(new
            {
                fullName = user.FullName,
                profilePictureUrl = user.ProfilePictureUrl
            });
        }

    }
}
