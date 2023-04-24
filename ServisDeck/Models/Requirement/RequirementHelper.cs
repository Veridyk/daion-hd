using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServisDeck.Models.Requirement
{
    public class RequirementHelper
    {
        public static string GetState(RequirementStatus status)
        {
            string result = "";
            switch (status)
            {
                case RequirementStatus.NEW: result = "Evidován"; break;
                case RequirementStatus.INPROGRESS: result = "V řešení"; break;
                case RequirementStatus.DONE: result = "Vyřešen"; break;
            }

            return result;
        }
    }
}
