using ServisDeck.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServisDeck.Services
{
    public interface IUserService
    {
        List<ApplicationUser> GetUsers();
        ApplicationUser GetUser(string id);
        Task<string> GetUserRole(ApplicationUser user);
        Task<bool> CreateUser(ApplicationUser user, string password, string role);
        Task UpdateUser(ApplicationUser user);
        Task DeleteUser(string id);
        Task ChangeRole(ApplicationUser user, string role, string newRole);
        Task<bool> ChangePassword(ApplicationUser user, string newPassword);
    }
}
