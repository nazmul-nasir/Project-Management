using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Project_Management.Models;
using Project_Management.Repository;

namespace Project_Management.Services
{
    public class UserService : Service<User>, IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly AppSettings _appSettings;
        private readonly IUserRepository _userRepository;

        public UserService(UserManager<User> userManager,
            SignInManager<User> signInManager,
            IOptions<AppSettings> appSettings,
            IUserRepository userRepository) : base(userRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appSettings.Value;
            _userRepository = userRepository;
        }


        /*  public async Task<IActionResult> AuthenticateAsync(string email, string password)
          {
              *//*var user = await _userManager.FindByEmailAsync(email);

              if (user == null)
              {
                  return null;
              }

              var result = await _userManager.CheckPasswordAsync(user, password);

              if (!result)
              {
                  return null;
              }

              var tokenHandler = new JwtSecurityTokenHandler();
              var key = Encoding.ASCII.GetBytes(_appSettings.Jwt.Secret);
              var tokenDescriptor = new SecurityTokenDescriptor
              {
                  Subject = new ClaimsIdentity(new[]
                  {
                  new Claim(ClaimTypes.Name, user.Id.ToString()),
                  new Claim(ClaimTypes.Email, user.Email),
                  new Claim(ClaimTypes.Role, user.Role.ToString())
              }),
                  Expires = DateTime.UtcNow.AddDays(7),
                  SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
              };
              var token = tokenHandler.CreateToken(tokenDescriptor);
              user.Token = tokenHandler.WriteToken(token);

              return user;*//*
              var user = await _userManager.FindByEmailAsync(email);
              if (user != null && await _userManager.CheckPasswordAsync(user, password))
              {
                  var userRoles = await _userManager.GetRolesAsync(user);

                  var authClaims = new List<Claim>
                             {
                                 new Claim(ClaimTypes.Name, user.UserName),
                                 new Claim(ClaimTypes.Role, user.Role),
                                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                             };

                  foreach (var userRole in userRoles)
                  {
                      authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                  }

                  var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ByYM000OLlMQG6VVVp1OH7Xzyr7gHuw1qvUC5dcGt3SNM"));

                  var token = new JwtSecurityToken(
                      issuer: "http://localhost:61955",
                      audience: "http://localhost:4200",
                      expires: DateTime.Now.AddHours(12),
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
          }*/

        public IEnumerable<User> GetAll()
        {
            return _userManager.Users.ToList();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _userManager.FindByIdAsync(id.ToString());
        }

        public async Task<User> CreateAsync(User user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                throw new ApplicationException(string.Join(",", result.Errors.Select(e => e.Description)));
            }

            return user;
        }

        public async Task UpdateAsync(User user, string password = null)
        {
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                throw new ApplicationException(string.Join(",", result.Errors.Select(e => e.Description)));
            }

            if (!string.IsNullOrEmpty(password))
            {
                await _userManager.RemovePasswordAsync(user);
                await _userManager.AddPasswordAsync(user, password);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);

                if (!result.Succeeded)
                {
                    throw new ApplicationException(string.Join(",", result.Errors.Select(e => e.Description)));
                }
            }
        }
    }

}
