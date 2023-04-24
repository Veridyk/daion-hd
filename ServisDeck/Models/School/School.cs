using ServisDeck.Models.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServisDeck.Models.School
{
    public class School
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Název školy")]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        [DisplayName("Zodpovědná osoba")]
        public string ResponsiblePerson { get; set; }
        public string City { get; set; }
        [DisplayName("Ulice")]
        public string Street { get; set; }
        [DisplayName("Číslo popisné")]
        public string StreetNumber { get; set; }
        [DisplayName("PSČ")]
        public string PostalCode { get; set; }
        [DisplayName("IČO")]
        public string ICO { get; set; }
        [DisplayName("DIČ")]
        public string DIC { get; set; }
        [Phone]
        [DisplayName("Telefon")]
        public string Phone { get; set; }
        [EmailAddress]
        [DisplayName("Email")]
        public string Mail { get; set; }
        public string Web { get; set; }
        public List<ApplicationUser> ApplicationUsers { get; set; }
        public List<Requirement.Requirement> Requirements { get; set; }

        public string GetFullAdress()
        {
            return $"{Street} {StreetNumber}, {City} {PostalCode}";
        }
    }
}
