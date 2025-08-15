using backend.Models;
using Microsoft.AspNetCore.Identity;

namespace backend.Services
{
    public class UserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly LdapService _ldapService;

        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, LdapService ldapService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _ldapService = ldapService;
        }

        public async Task<bool> LoginAsync(LoginRequest request)
        {
            var valid = await _ldapService.ValidateCredentials(request.Username, request.Password);
            if (!valid) return false;

            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null)
            {
                user = new AppUser { UserName = request.Username };
                await _userManager.CreateAsync(user);
            }

            await _signInManager.SignInAsync(user, false);
            return true;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<bool> ChangePasswordAsync(AppUser user, ChangePasswordRequest request)
        {
            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            return result.Succeeded;
        }
    }
}
