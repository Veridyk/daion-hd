using Microsoft.EntityFrameworkCore;
using ServisDeck.Data;
using ServisDeck.Models.Requirement;
using ServisDeck.Models.School;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServisDeck.Services
{
    public class SchoolService : ISchoolService
    {
        private readonly ApplicationDbContext _context;
        public SchoolService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void CreateSchool(School school)
        {
            _context.Add(school);
            _context.SaveChanges();
        }

        public void UpdateSchool(School school)
        {
            _context.Schools.Attach(school);
            _context.Entry(school).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public School GetSchool(string id)
        {
            var user = _context.Users
                .Where(u => u.Id == id)
                .Include(s => s.School)
                .ThenInclude(s => s.Requirements)
                .ThenInclude(r => r.Creator).FirstOrDefault();

            return user.School;
        }

        public School GetSchool(int id)
        {
            var school = _context.Schools
                .Where(x => x.Id == id)
                .Include(x => x.ApplicationUsers)
                .Include(x => x.Requirements)
                .ThenInclude(x => x.Creator)
                .FirstOrDefault();

            return school;
        }

        public List<School> GetSchools()
        {
            var schools = _context.Schools
                .Include(x => x.ApplicationUsers)
                .Include(x => x.Requirements)
                .ThenInclude(x => x.Creator).ToList();

            return schools;
        }

        public School AddRequirementToSchool(string id, RequirementViewModel model)
        {
            var user = _context.Users.Where(x => x.Id == id).Include(x => x.School).FirstOrDefault();

            if (user != null)
            {
                Requirement req = new Requirement()
                {
                    Subject = model.Subject,
                    Room = model.Room,
                    Message = model.Message,
                    Note = model.Note,
                    Status = RequirementStatus.NEW,
                    School = user.School,
                    Creator = user,
                    Created = DateTime.Now
                };

                _context.Requirements.Add(req);
                _context.SaveChanges();

                return user.School;
            }

            return null;
        }
    }
}
