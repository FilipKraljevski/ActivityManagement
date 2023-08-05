using ActivityManagement.Domain.Identity;
using ActivityManagement.Domain.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityManagement.Repository
{
    public class ApplicationDbContext : IdentityDbContext<ActivityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Activity> Activities { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Activity>()
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();
        }
    }
}
