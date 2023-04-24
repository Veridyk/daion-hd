using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServisDeck.Models.School;
using ServisDeck.Services;

namespace ServisDeck.Controllers
{
    [Authorize(Roles = "admin")]
    public class SchoolController : Controller
    {
        private readonly ISchoolService SchoolProvider;

        public SchoolController(ISchoolService schoolService)
        {
            SchoolProvider = schoolService;
        }
        public IActionResult Index()
        {
            SchoolMainViewModel model = new SchoolMainViewModel();
            model.School = new School();
            model.Schools = SchoolProvider.GetSchools();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateSchool(SchoolMainViewModel model)
        {
            if (ModelState.IsValid)
            {
                School school = new School()
                {
                    Name = model.School.Name,
                    ResponsiblePerson = model.School.ResponsiblePerson,
                    City = model.School.City,
                    Street = model.School.Street,
                    StreetNumber = model.School.StreetNumber,
                    PostalCode = model.School.PostalCode,
                    ICO = model.School.ICO,
                    DIC = model.School.DIC,
                    Phone = model.School.Phone,
                    Mail = model.School.Mail,
                    Web = model.School.Web
                };

                SchoolProvider.CreateSchool(school);

                TempData["messageColor"] = "bg-teal-100";
                TempData["message"] = SchoolHelper.GetSchoolCreateMessage(SchoolResult.SUCCESS);
                return RedirectToAction("Index", "School");
            }

            TempData["messageColor"] = "text-red-700";
            TempData["message"] = SchoolHelper.GetSchoolCreateMessage(SchoolResult.FAIL);

            return RedirectToAction("Index", "School");
        }

        public IActionResult Detail(int id)
        {
            School school = SchoolProvider.GetSchool(id);

            if(school == null)
            {
                return NotFound();
            }

            return View(school);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditSchool(School model)
        {
            if (ModelState.IsValid)
            {
                School school = SchoolProvider.GetSchool(model.Id);

                if (school == null)
                {
                    return NotFound();
                }

                school.Name = model.Name;
                school.ResponsiblePerson = model.ResponsiblePerson;
                school.City = model.City;
                school.Street = model.Street;
                school.StreetNumber = model.StreetNumber;
                school.PostalCode = model.PostalCode;
                school.ICO = model.ICO;
                school.DIC = model.DIC;
                school.Phone = model.Phone;
                school.Mail = model.Mail;
                school.Web = model.Web;

                SchoolProvider.UpdateSchool(school);

                TempData["messageColor"] = "bg-teal-100";
                TempData["message"] = SchoolHelper.GetSchoolUpdateMessage(SchoolResult.SUCCESS);
                return RedirectToAction("Index", "School");
            }

            TempData["messageColor"] = "text-red-700";
            TempData["message"] = SchoolHelper.GetSchoolUpdateMessage(SchoolResult.FAIL);

            return RedirectToAction("Index", "School");
        }
    }
}
