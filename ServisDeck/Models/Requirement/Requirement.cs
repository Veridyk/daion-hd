using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ServisDeck.Models.Requirement
{
    public class Requirement
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Předmět")]
        public string Subject { get; set; }
        [Required]
        [DisplayName("Učebna")]
        public string Room { get; set; }
        [Required]
        [DisplayName("Popis")]
        public string Message { get; set; }
        [DisplayName("Poznámka")]
        public string Note { get; set; }
        [Required]
        [DisplayName("Stav")]
        public RequirementStatus Status { get; set; }
        [Required]
        [DisplayName("Evidováno")]
        public DateTime Created { get; set; }
        [ForeignKey("SchoolId")]
        public School.School School { get; set; }
        [ForeignKey("CreatorId")]
        public Users.ApplicationUser Creator { get; set; }

        public string GetState()
        {
            string result = "";
            switch (Status)
            {
                case RequirementStatus.NEW: result = "Evidován"; break;
                case RequirementStatus.INPROGRESS: result = "V řešení"; break;
                case RequirementStatus.DONE: result = "Vyřešen"; break;
            }

            return result;
        }
    }
}
