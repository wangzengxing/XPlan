using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using XPlan.Model;

namespace XPlan.Repository.EntityFrameworkCore
{
    public class XPlanDbContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Plan>().ToTable("plan");
            modelBuilder.Entity<User>().ToTable("user");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("FileName=XPlan.db", options =>
             {
                 options.MigrationsAssembly("XPlan.WebApi");
             });
        }
    }
}
