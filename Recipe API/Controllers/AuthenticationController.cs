using Demo_JWT.Dto.Authentication;
using Demo_JWT.Dto.Cmon;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Recipe_API.DTO.Authentification;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Recipe_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        public AuthenticationController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager; 
            _configuration = configuration;
        }


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            var userExists = await _userManager.FindByNameAsync(dto.UserName);
            if (userExists != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto
                {
                    Status = "Error",
                    Message = "User already exists!"
                });
            }

            IdentityUser user = new IdentityUser
            {
                Email = dto.UserName,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = dto.UserName
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto
                {
                    Status = "Error",
                    Message = "User creation failed! Please check user details and try again."
                });
            }


            // voor admin
            if (dto.IsAdmin && !await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (dto.IsAdmin)
            {
                await _userManager.AddToRoleAsync(user, "Admin");
            }

            return Ok(new ResponseDto { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var existingUser = await _userManager.FindByNameAsync(dto.UserName);
            if (existingUser != null && await _userManager.CheckPasswordAsync(existingUser, dto.Password))
            {
                var authClaims = new List<Claim>
                 {
                    new Claim(ClaimTypes.Name, existingUser.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                 };

                //claims opvragen en toevoegen aan lijst
                var existingClaims = await _userManager.GetClaimsAsync(existingUser);
                authClaims.AddRange(existingClaims);
                var token = GetToken(authClaims);
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddHours(3),
            claims: authClaims,
           signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
            return token;
        }

    }

}

