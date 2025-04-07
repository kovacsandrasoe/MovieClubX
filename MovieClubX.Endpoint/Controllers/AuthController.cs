using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Validations;
using MovieClubX.Endpoint.Helpers;
using MovieClubX.Entities.Dto.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MovieClubX.Endpoint.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private UserManager<AppUser> userManager;
        private RoleManager<IdentityRole> roleManager;
        private IConfiguration configuration;

        public AuthController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
        }

        [HttpPost("register")]
        public async Task Register(UserCreateDto dto)
        {
            var user = new AppUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                EmailConfirmed = true,
                FamilyName = dto.FamilyName,
                GivenName = dto.GivenName,
                RefreshToken = ""
            };
            await userManager.CreateAsync(user, dto.Password);

            if (userManager.Users.Count() == 1)
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
                await userManager.AddToRoleAsync(user, "Admin");
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(TokenApiDto tokenApiDto)
        {
            if (tokenApiDto is null)
                return BadRequest("Invalid client request");
            string accessToken = tokenApiDto.AccessToken;
            string refreshToken = tokenApiDto.RefreshToken;
            var principal = GetPrincipalFromExpiredToken(accessToken);
            var username = principal?.Identity?.Name;

            var user = await userManager.GetUserAsync(principal!);
            if (user is null || user.RefreshToken != refreshToken)
                return BadRequest("Invalid client request");

            int accessTokenExpiryInMinutes = 24 * 60;
            var newAccessToken = GenerateAccessToken(principal?.Claims, accessTokenExpiryInMinutes);
            var newRefreshToken = await GenerateRefreshToken(user);
            user.RefreshToken = newRefreshToken;

            await userManager.UpdateAsync(user);

            return Ok(new
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                RefreshToken = newRefreshToken,
                AccessTokenExpiration = newAccessToken.ValidTo,
                RefreshTokenExpiration = DateTime.Now.AddMinutes(accessTokenExpiryInMinutes * 7)
            });
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                  Encoding.UTF8.GetBytes(configuration["jwt:key"] ?? "")),
                ValidateLifetime = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto dto)
        {
            var user = await userManager.FindByEmailAsync(dto.Email);
            if (user != null)
            {
                var result = await userManager.CheckPasswordAsync(user, dto.Password);
                if (result)
                {
                    //van ilyen user és jó a jelszava
                    //todo: generate token
                    var claim = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName!),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                    };

                    foreach (var role in await userManager.GetRolesAsync(user))
                    {
                        claim.Add(new Claim(ClaimTypes.Role, role));
                    }

                    int accessTokenExpiryInMinutes = 24 * 60;
                    var accessToken = GenerateAccessToken(claim, accessTokenExpiryInMinutes);
                    int refreshTokenExpiryInMinutes = 24 * 60 * 7;
                    var refreshToken = await GenerateRefreshToken(user);

                    return Ok(new LoginResultDto()
                    {
                        AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                        AccessTokenExpiration = DateTime.Now.AddMinutes(accessTokenExpiryInMinutes),
                        RefreshToken = refreshToken,
                        RefreshTokenExpiration = DateTime.Now.AddMinutes(refreshTokenExpiryInMinutes)
                    });
                }
                else
                {
                    //throw new ArgumentException("Nem jó a jelszó");
                    return BadRequest("Nem jó a jelszó");
                }
            }
            else
            {
                //throw new ArgumentException("Nincs ilyen user");
                return BadRequest("Nincs ilyen user");
            }
            
        }

        private JwtSecurityToken GenerateAccessToken(IEnumerable<Claim>? claims, int expiryInMinutes)
        {
            var signinKey = new SymmetricSecurityKey(
                  Encoding.UTF8.GetBytes(configuration["jwt:key"] ?? throw new Exception("jwt:key not found in appsettings.json")));

            return new JwtSecurityToken(
                  issuer: "movieclub.com",
                  audience: "movieclub.com",
                  claims: claims?.ToArray(),
                  expires: DateTime.Now.AddMinutes(expiryInMinutes),
                  signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256)
                );
        }

        private async Task<string> GenerateRefreshToken(AppUser user)
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                string result = Convert.ToBase64String(randomNumber);
                user.RefreshToken = result;
                await userManager.UpdateAsync(user);
                return result;
            }
        }
    }
}
