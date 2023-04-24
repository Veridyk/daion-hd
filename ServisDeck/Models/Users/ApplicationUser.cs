using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ServisDeck.Models.Users
{
    public class ApplicationUser : IdentityUser
    {
        [DisplayName("Celé jméno")]
        public string Name { get; set; }
        //public int? SchoolId { get; set; }
        [ForeignKey("SchoolId")]
        public School.School School { get; set; }
    }
}
