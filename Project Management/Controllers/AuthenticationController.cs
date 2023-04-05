using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Project_Management.Models;
using Project_Management.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Response = Project_Management.Models.Response;

namespace Project_Management.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IDeveloperService _developerService;
        private readonly IUserService _userService;
        private readonly IAdminService _adminService;
        private readonly IAuthService _authService;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly AppSettings _appSettings;
        public AuthenticationController(IDeveloperService developerService, IUserService userService, IAdminService adminService, IAuthService authService, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IOptions<AppSettings> appSettings)
        {
            _developerService = developerService;
            _userService = userService;
            _adminService = adminService;
            _authService = authService;
            this._userManager = userManager;
            this.roleManager = roleManager;
            _appSettings = appSettings.Value;
        }

        [HttpPost("register/admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegistrationModel registrationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isUniqueEmail = (await _userManager.FindByEmailAsync(registrationDto.Email) == null);
            if (!isUniqueEmail)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Email Address already exist" });
            }

            var admin = new User
            {
                Email = registrationDto.Email,
                UserName = registrationDto.UserName,
                Role = Role.Admin,
            };

            try
            {
                var result = await _userManager.CreateAsync(admin, registrationDto.Password);
                if (!result.Succeeded)
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = result.Errors.ToString() });

                if (!await roleManager.RoleExistsAsync(Role.Admin))
                    await roleManager.CreateAsync(new IdentityRole(Role.Admin));

                if (await roleManager.RoleExistsAsync(Role.Admin))
                    await _userManager.AddToRoleAsync(admin, Role.Admin);

                return Ok(new Response { Status = "Success", Message = "Admin created successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("register/developer")]
        public async Task<IActionResult> RegisterDeveloper([FromBody] RegistrationModel registrationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isUniqueEmail = (await _userManager.FindByEmailAsync(registrationDto.Email) == null);
            if (!isUniqueEmail)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Email Address already exist" });
            }

            var developer = new User
            {
                Email = registrationDto.Email,
                Role = Role.Developer,
                UserName = registrationDto.UserName,
            };

            try
            {
                var result = await _userManager.CreateAsync(developer, registrationDto.Password);
                if (!result.Succeeded)
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = result.Errors.ToString() });

                if (!await roleManager.RoleExistsAsync(Role.Developer))
                    await roleManager.CreateAsync(new IdentityRole(Role.Developer));

                if (await roleManager.RoleExistsAsync(Role.Developer))
                    await _userManager.AddToRoleAsync(developer, Role.Developer);

                return Ok(new Response { Status = "Success", Message = "Developer created successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, request.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                           {
                               new Claim(ClaimTypes.Name, user.UserName),
                               new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                           };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Jwt.Secret));

                var token = new JwtSecurityToken(
                    issuer: _appSettings.Jwt.ValidIssuer,
                    audience: _appSettings.Jwt.ValidAudience,
                    expires: DateTime.Now.AddDays(7),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();

        }
    }
}
