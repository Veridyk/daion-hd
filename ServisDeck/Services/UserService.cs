using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServisDeck.Data;
using ServisDeck.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServisDeck.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public List<ApplicationUser> GetUsers()
        {
            return _userManager.Users.ToList();
        }

        public ApplicationUser GetUser(string id)
        {
            return _context.Users
                .Where(x => x.Id == id)
                .Include(x => x.School)
                .FirstOrDefault();
        }

        public async Task<bool> CreateUser(ApplicationUser user, string password, string role)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                result = await _userManager.AddToRoleAsync(user, role);
            }

            return result.Succeeded;
        }

        public async Task UpdateUser(ApplicationUser user)
        {
            await _userManager.UpdateAsync(user);
        }

        public async Task DeleteUser(string id)
        {
            var user = GetUser(id);
            await _userManager.DeleteAsync(user);
        }

        public async Task<string> GetUserRole(ApplicationUser user)
        {
            var result = await _userManager.GetRolesAsync(user);

            return result.FirstOrDefault();
        }

        public async Task ChangeRole(ApplicationUser user, string role, string newRole)
        {
            var result = await _userManager.RemoveFromRoleAsync(user, role);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, newRole);
            }
        }

        public async Task<bool> ChangePassword(ApplicationUser user, string newPassword)
        {
            string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = _userManager.ResetPasswordAsync(user, resetToken, newPassword);

            return result.Result.Succeeded;
        }
    }
}
