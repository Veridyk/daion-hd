using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServisDeck.Models.Requirement;
using ServisDeck.Models.Users;
using ServisDeck.Services;

namespace ServisDeck.Controllers
{
    [Authorize(Roles = "admin")]
    public class DeckAdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRequirementService RequirementProvider;
        private readonly IMailService MailProvider;
        public DeckAdminController(UserManager<ApplicationUser> userManager, IRequirementService requirementProvider, IMailService mailProvider)
        {
            RequirementProvider = requirementProvider;
            _userManager = userManager;
            MailProvider = mailProvider;
        }

        public IActionResult Index()
        {
            RequirementViewIndexModel model = new RequirementViewIndexModel();
            model.Requirements = RequirementProvider.GetRequirements();
            model.RequirementViewModel = new RequirementViewModel();
            return View(model);
        }

        public IActionResult Detail(int id)
        {
            var requirement = RequirementProvider.GetRequirement(id);

            if(requirement == null)
            {
                return NotFound();
            }

            var stateList = new List<SelectListItem>()
            {
                new SelectListItem(){ Text = RequirementHelper.GetState(RequirementStatus.NEW), Value = RequirementStatus.NEW.ToString(), Selected = requirement.Status == RequirementStatus.NEW },
                new SelectListItem(){ Text = RequirementHelper.GetState(RequirementStatus.INPROGRESS), Value = RequirementStatus.INPROGRESS.ToString(), Selected = requirement.Status == RequirementStatus.INPROGRESS },
                new SelectListItem(){ Text = RequirementHelper.GetState(RequirementStatus.DONE), Value = RequirementStatus.DONE.ToString(), Selected = requirement.Status == RequirementStatus.DONE }
            };

            ViewBag.StateList = stateList;

            return View(requirement);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRequirement(RequirementViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var requirement = RequirementProvider.GetRequirement(model.Id);

                if (requirement != null)
                {
                    //set the values
                    requirement.Subject = model.Subject;
                    requirement.Room = model.Room;
                    requirement.Message = model.Message;
                    requirement.Note = model.Note;
                    requirement.Status = model.Status;

                    //call the service
                    RequirementProvider.UpdateRequirement(requirement);

                    //MailProvider.SendMail(new List<string>() { user.Email, requirement.School.ResponsiblePerson, requirement.Creator.Email }, MailService.UPDATE_TITLE, model.Subject, model.Message);

                    TempData["messageColor"] = "bg-teal-100";
                    TempData["message"] = Helpers.Helpers.UpdateRequirementResultMessage(RequirementResult.SUCCESS);
                    return Redirect("Detail/" + model.Id);
                }

                return NotFound();
            }

            TempData["messageColor"] = "text-red-700";
            TempData["message"] = Helpers.Helpers.UpdateRequirementResultMessage(RequirementResult.FAIL);

            return Redirect("Detail/" + model.Id);
        }
    }
}
