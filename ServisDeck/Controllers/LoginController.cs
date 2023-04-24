using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServisDeck.Models.Login;
using ServisDeck.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServisDeck.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LoginController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: true, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Deck");
                }

                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "The account has been locked.");
                }
                else if (result.IsNotAllowed)
                {
                    ModelState.AddModelError("", "The account has not been confirmed yet.");
                }
                else if (result.RequiresTwoFactor)
                {
                    ModelState.AddModelError("", "The two factor authentication is required.");
                }
                else
                {
                    ModelState.AddModelError("", "Entered credentionals are incorrect.");
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Login");
        }
    }
}
