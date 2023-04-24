using Microsoft.EntityFrameworkCore;
using ServisDeck.Data;
using ServisDeck.Models.Requirement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServisDeck.Services
{
    public class RequirementService : IRequirementService
    {
        private readonly ApplicationDbContext _context;
        public RequirementService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Requirement> GetRequirements()
        {
            return _context.Requirements
                .Include(x => x.Creator)
                .Include(x=> x.School)
                .ToList();
        }

        public Requirement GetRequirement(int id)
        {
            var req = _context.Requirements
                .Where(x => x.Id == id)
                .Include(c => c.Creator)
                .Include(s => s.School)
                .ThenInclude(s => s.ApplicationUsers)
                .FirstOrDefault();

            return req;
        }

        public void UpdateRequirement(Requirement model)
        {
            _context.Requirements.Attach(model);
            _context.Entry(model).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
