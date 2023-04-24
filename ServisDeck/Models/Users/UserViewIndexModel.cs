using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServisDeck.Models.Users
{
    public class UserViewIndexModel
    {
        public List<ApplicationUser> Users { get; set; }
        public UserViewModel UserViewModel { get; set; }
    }
}
