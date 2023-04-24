using ServisDeck.Models.Requirement;
using ServisDeck.Models.School;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServisDeck.Services
{
    public interface ISchoolService
    {
        void CreateSchool(School school);
        void UpdateSchool(School school);
        School GetSchool(string id);
        School AddRequirementToSchool(string id, RequirementViewModel model);
        List<School> GetSchools();
        School GetSchool(int id);
    }
}
