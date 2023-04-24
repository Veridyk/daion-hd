using ServisDeck.Models.Requirement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServisDeck.Services
{
    public interface IRequirementService
    {
        List<Requirement> GetRequirements();
        Requirement GetRequirement(int id);
        void UpdateRequirement(Requirement model);
    }
}
