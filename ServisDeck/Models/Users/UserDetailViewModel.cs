using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServisDeck.Models.Users
{
    public class UserDetailViewModel
    {
        public UserViewModel UserViewModel { get; set; }
        public PasswordViewModel PasswordViewModel { get; set; }
    }
}
