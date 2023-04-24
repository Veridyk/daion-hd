using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServisDeck.Models.School
{
    public class SchoolHelper
    {
        public static string GetSchoolCreateMessage(SchoolResult result)
        {
            switch (result)
            {
                case SchoolResult.SUCCESS: { return "Škola byla úspěšně vytvořena."; }
                case SchoolResult.FAIL: { return "Internal error. Zkontaktujte správce ICT."; }
                default: { return null; }
            }
        }

        public static string GetSchoolUpdateMessage(SchoolResult result)
        {
            switch (result)
            {
                case SchoolResult.SUCCESS: { return "Škola byla úspěšně upravena."; }
                case SchoolResult.FAIL: { return "Internal error. Zkontaktujte správce ICT."; }
                default: { return null; }
            }
        }
    }
}
