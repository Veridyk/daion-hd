using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServisDeck.Data;
using ServisDeck.Models.Requirement;
using ServisDeck.Models.School;
using ServisDeck.Models.Users;
using ServisDeck.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServisDeck.Controllers
{
    [Authorize]
    public class DeckController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private readonly ISchoolService SchoolProvider;
        private readonly IRequirementService RequirementService;
        private readonly IMailService MailProvider;

        public DeckController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ISchoolService schoolProvider, IRequirementService requirementService, IMailService mailProvider)
        {
            _userManager = userManager;
            SchoolProvider = schoolProvider;
            RequirementService = requirementService;
            MailProvider = mailProvider;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index(int? status)
        {
            var user = await _userManager.GetUserAsync(User);
            

            //create first add admin
            
            /*if (!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole() { Name = "admin" });
                await _userManager.AddToRoleAsync(user, "admin");
            }
            var res = await _userManager.AddToRoleAsync(user, "ADMIN");*/

            var school = SchoolProvider.GetSchool(user.Id);

            SchoolIndexViewModel model = new SchoolIndexViewModel();
            model.School = school;
            model.RequirementViewModel = new RequirementViewModel();
            model.RequirementViewModel.Name = user.Name;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRequirement(SchoolIndexViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                var result = SchoolProvider.AddRequirementToSchool(user.Id, model.RequirementViewModel);
                if (result != null)
                {
                    MailProvider.SendMail(new List<string>() { user.Email, result.ResponsiblePerson, "klimes@daion.cz" }, MailService.CREATE_TITLE, model.RequirementViewModel.Subject, model.RequirementViewModel.Message);
                    TempData["messageColor"] = "bg-teal-100";
                    TempData["message"] = Helpers.Helpers.GetRequirementResultMessage(RequirementResult.SUCCESS);
                    return RedirectToAction("Index", "deck");
                }
            }

            TempData["messageColor"] = "text-red-700";
            TempData["message"] = Helpers.Helpers.GetRequirementResultMessage(RequirementResult.FAIL);

            return RedirectToAction("Index", "Deck");
        }

        public async Task<IActionResult> Detail(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var requirement = RequirementService.GetRequirement(id);

            var isSchoolMember = requirement.School.ApplicationUsers.Exists(x => x.Id == user.Id);
            //if(!requirement.School.ApplicationUsers.Contains(user))
            if(!isSchoolMember)
            {
                return NotFound();
            }

            if (user.Id == requirement.Creator.Id)
            {
                ViewBag.showEdit = true;
            }

            return View(requirement);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRequirement(RequirementViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var requirement = RequirementService.GetRequirement(model.Id);

                if (requirement != null)
                {
                    var isSchoolMember = requirement.School.ApplicationUsers.Exists(x => x.Id == user.Id);
                    if (user.Id == requirement.Creator.Id ||
                        isSchoolMember)
                    {
                        //set the values
                        requirement.Subject = model.Subject;
                        requirement.Room = model.Room;
                        requirement.Message = model.Message;

                        //call the service
                        RequirementService.UpdateRequirement(requirement);

                        //MailProvider.SendMail(new List<string>() { user.Email, requirement.School.ResponsiblePerson, "klimes@daion.cz" }, MailService.UPDATE_TITLE,  model.Subject, model.Message);

                        TempData["messageColor"] = "bg-teal-100";
                        TempData["message"] = Helpers.Helpers.UpdateRequirementResultMessage(RequirementResult.SUCCESS);
                        return Redirect("Detail/" + model.Id);
                    }
                }

                return NotFound();
            }

            TempData["messageColor"] = "text-red-700";
            TempData["message"] = Helpers.Helpers.UpdateRequirementResultMessage(RequirementResult.FAIL);

            return Redirect("Detail/" + model.Id);
        }
    }
}
