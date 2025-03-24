using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Validations;
using MovieClubX.Endpoint.Helpers;
using MovieClubX.Entities.Dto.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
                GivenName = dto.GivenName
            };
            await userManager.CreateAsync(user, dto.Password);

            if (userManager.Users.Count() == 1)
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
                await userManager.AddToRoleAsync(user, "Admin");
            }
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

                    int expiryInMinutes = 24 * 60;
                    var token = GenerateAccessToken(claim, expiryInMinutes);

                    return Ok(new LoginResultDto()
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        Expiration = DateTime.Now.AddMinutes(expiryInMinutes)
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
    }
}
