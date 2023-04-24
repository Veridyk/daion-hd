using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServisDeck.Models.Users
{
    public class PasswordViewModel
    {
        public string Id { get; set; }
        //[Required]
        [DisplayName("Heslo:")]
        public string Password { get; set; }
    }
}
