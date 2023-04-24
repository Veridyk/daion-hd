using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServisDeck.Models.School;
using ServisDeck.Models.Users;
using ServisDeck.Services;

namespace ServisDeck.Controllers
{
    [Authorize(Roles = "admin")]
    public class UsersController : Controller
    {
        private readonly IUserService UserProvider;
        private readonly ISchoolService SchoolProvider;
        public UsersController(IUserService userProvider, ISchoolService schoolProvider)
        {
            UserProvider = userProvider;
            SchoolProvider = schoolProvider;
        }

        public IActionResult Index()
        {
            var RoleList = new List<SelectListItem>()
            {
                new SelectListItem(){ Text = "user", Value = "user", Selected = true },
                new SelectListItem(){ Text = "admin", Value = "admin" }
            };

            var SchoolList = SchoolProvider
                .GetSchools()
                .Select(x => new SelectListItem() { 
                    Text = x.Name,
                    Value = x.Id.ToString()
            });

            UserViewIndexModel model = new UserViewIndexModel();
            model.Users = UserProvider.GetUsers();
            model.UserViewModel = new UserViewModel();
            ViewBag.RoleList = RoleList;
            ViewBag.SchoolList = SchoolList;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(UserViewIndexModel model)
        {
            if (ModelState.IsValid)
            {
                int SchoolId = Convert.ToInt32(model.UserViewModel.School);
                var school = SchoolProvider.GetSchool(SchoolId);
                ApplicationUser user = new ApplicationUser()
                {
                    Email = model.UserViewModel.Email,
                    UserName = model.UserViewModel.Email,
                    Name = model.UserViewModel.Name,
                    EmailConfirmed = true,
                    School = school
                };

                bool isPasswordOk = await UserProvider.CreateUser(user, model.UserViewModel.Password, model.UserViewModel.Role);

                if (!isPasswordOk)
                {
                    TempData["messageColor"] = "text-red-700";
                    TempData["message"] = UserHelper.GetUserChangeMessage(UserResult.FAIL);
                    return RedirectToAction("Index", "Users");
                }

                TempData["messageColor"] = "bg-teal-100";
                TempData["message"] = UserHelper.GetUserCreateMessage(UserResult.SUCCESS);
                return RedirectToAction("Index", "Users");
            }

            TempData["messageColor"] = "text-red-700";
            TempData["message"] = UserHelper.GetUserCreateMessage(UserResult.FAIL);

            return RedirectToAction("Index", "Users");
        }

        //[Authorize(Roles = "admin, user")]
        public IActionResult Detail(string id)
        {
            
            var user = UserProvider.GetUser(id);

            /*if (!User.IsInRole("admin"))
            {
                if (user.Id != id)
                {
                    return NotFound();
                }
            }*/

            if (user == null)
            {
                return NotFound();
            }

            var role = UserProvider.GetUserRole(user).Result;
            var RoleList = new List<SelectListItem>()
            {
                new SelectListItem(){ Text = "user", Value = "user", Selected = role == "user"},
                new SelectListItem(){ Text = "admin", Value = "admin", Selected = role == "admin" }
            };

            var SchoolList = SchoolProvider
                .GetSchools()
                .Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                    Selected = x.Id == user.School.Id
            });

            UserDetailViewModel model = new UserDetailViewModel();
            model.PasswordViewModel = new PasswordViewModel();
            model.PasswordViewModel.Id = user.Id;
            model.UserViewModel = new UserViewModel();
            model.UserViewModel.Id = user.Id;
            model.UserViewModel.Email = user.Email;
            model.UserViewModel.UserName = user.Email;
            model.UserViewModel.Name = user.Name;

            ViewBag.RoleList = RoleList;
            ViewBag.SchoolList = SchoolList;
            ViewBag.RoleName = role;
            ViewBag.SchoolName = user.School.Name;

            return View(model);
        }

        //[Authorize(Roles = "admin, user")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(UserDetailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = UserProvider.GetUser(model.UserViewModel.Id);

                /*if (!User.IsInRole("admin"))
                {
                    if (user.Id != model.UserViewModel.Id)
                    {
                        return NotFound();
                    }
                }*/

                if (user == null)
                {
                    return NotFound();
                }

                user.Email = model.UserViewModel.Email;
                user.UserName = model.UserViewModel.Email;
                user.Name = model.UserViewModel.Name;

                var role = UserProvider.GetUserRole(user).Result;
                if (model.UserViewModel.Role != role)
                {
                    await UserProvider.ChangeRole(user, role, model.UserViewModel.Role);
                }

                int SchoolId = Convert.ToInt32(model.UserViewModel.School);
                if (SchoolId != user.School.Id)
                {
                    var school = SchoolProvider.GetSchool(SchoolId);
                    user.School = school;
                }

                await UserProvider.UpdateUser(user);

                TempData["messageColor"] = "bg-teal-100";
                TempData["message"] = UserHelper.GetUserUpdateMessage(UserResult.SUCCESS);

                return RedirectToAction("Index", "Users");
            }

            TempData["messageColor"] = "text-red-700";
            TempData["message"] = UserHelper.GetUserUpdateMessage(UserResult.FAIL);

            return RedirectToAction("Index", "Users");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(UserDetailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = UserProvider.GetUser(model.PasswordViewModel.Id);

                if (user == null)
                {
                    return NotFound();
                }

                var isOk = await UserProvider.ChangePassword(user, model.PasswordViewModel.Password);

                if (isOk)
                {
                    TempData["messageColor"] = "bg-teal-100";
                    TempData["message"] = UserHelper.GetUserChangeMessage(UserResult.SUCCESS);

                    return RedirectToAction("Index", "Users");
                }
            }

            TempData["messageColor"] = "text-red-700";
            TempData["message"] = UserHelper.GetUserChangeMessage(UserResult.FAIL);

            return RedirectToAction("Index", "Users");
        }
    }
}
