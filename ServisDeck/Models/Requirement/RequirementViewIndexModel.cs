using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServisDeck.Models.Requirement
{
    public class RequirementViewIndexModel
    {
        public List<Requirement> Requirements { get; set; }
        public RequirementViewModel RequirementViewModel { get; set; }
    }
}
