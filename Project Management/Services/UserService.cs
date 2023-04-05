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
