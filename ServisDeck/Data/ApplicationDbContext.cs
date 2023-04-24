using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ServisDeck.Models.Requirement;
using ServisDeck.Models.School;
using ServisDeck.Models.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServisDeck.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*var schools = modelBuilder.Entity<School>();
            schools.HasMany(s => s.ApplicationUsers).WithOne( x => x.School).HasForeignKey( );

            var req = modelBuilder.Entity<Requirement>();
            req.HasOne(x => x.Creator).WithOne();
            req.HasOne(x => x.School).WithMany();*/

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<School> Schools { get; set; }
        public DbSet<Requirement> Requirements { get; set; }
    }
}
