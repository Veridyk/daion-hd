using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServisDeck.Models.Users
{
    public class UserHelper
    {
        public static string GetUserCreateMessage(UserResult result)
        {
            switch (result)
            {
                case UserResult.SUCCESS: { return "Uživatel byl úspěšně založen."; }
                case UserResult.FAIL: { return "Internal error. Kontaktujte podporu"; }
                default: { return null; }
            }
        }

        public static string GetUserUpdateMessage(UserResult result)
        {
            switch (result)
            {
                case UserResult.SUCCESS: { return "Uživatel byl úspěšně aktualizován."; }
                case UserResult.FAIL: { return "Internal error. Kontaktujte podporu"; }
                default: { return null; }
            }
        }

        public static string GetUserChangeMessage(UserResult result)
        {
            switch (result)
            {
                case UserResult.SUCCESS: { return "Heslo bylo úspěšně změněno"; }
                case UserResult.FAIL: { return "Heslo musí obsahovat alespoň 8 znaků, velké písmeno, číslici a jeden speciální znak."; }
                default: { return null; }
            }
        }
    }
}
