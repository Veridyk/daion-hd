using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServisDeck.Data;
using ServisDeck.Models.Requirement;
using ServisDeck.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServisDeck.Helpers
{
    public class Helpers
    {
        public static async Task CreateAdmin(UserManager<ApplicationUser> manager)
        {
            var user = new ApplicationUser
            {
                UserName = "klimes",
                Email = "klimes@daion.cz",
                Name = "klimes@daion.cz",
                EmailConfirmed = true
            };

            await manager.CreateAsync(user, "Kokos123*");

        }

        public static void CreateRequirement(string id, ApplicationDbContext context)
        {
            var user = context.Users.Where(x => x.Id == id).Include(x => x.School).FirstOrDefault();

            Requirement req = new Requirement()
            {
                Subject = "Rozbitej hajzl",
                Room = "Hajzl na třetím patře",
                Message = "Ondro, rozbil se nám hajzl. Skoč ho pls opravit beruško.",
                Note = "Poznámka: všude na zemi je voda.",
                Status = RequirementStatus.NEW,
                School = user.School,
                Creator = user,
                Created = DateTime.Now
            };

            context.Requirements.Add(req);
            context.SaveChanges();
        }

        public static string GetRequirementResultMessage(RequirementResult result)
        {
            switch (result)
            {
                case RequirementResult.SUCCESS: { return "Požadavek byl úspěšně založen."; }
                case RequirementResult.FAIL: { return "Zpráva v požadavku musí být vyplněna!"; }
                default: { return null; }
            }
        }

        public static string UpdateRequirementResultMessage(RequirementResult result)
        {
            switch (result)
            {
                case RequirementResult.SUCCESS: { return "Požadavek byl úspěšně aktualizován."; }
                case RequirementResult.FAIL: { return "Internal error. Kontaktujte podporu"; }
                default: { return null; }
            }
        }


    }
}
