using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ServisDeck.Models.Requirement
{
    public class RequirementViewModel
    {
        public int Id { get; set; }
        [DisplayName("Zadavatel: ")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Pole předmět musí být vyplněno!")]
        [DisplayName("Předmět")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "Pole učebna musí být vyplněno!")]
        [DisplayName("Učebna")]
        public string Room { get; set; }
        [Required(ErrorMessage = "Pole popis musí být vyplněno!")]
        [AllowHtml]
        [DisplayName("Popis")]
        public string Message { get; set; }
        [DisplayName("Poznámka")]
        public string Note { get; set; }
        public RequirementStatus Status { get; set; }
    }
}
