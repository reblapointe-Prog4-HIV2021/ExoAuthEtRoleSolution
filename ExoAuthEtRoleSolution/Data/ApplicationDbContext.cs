using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using ExoAuthEtRoleSolution.Models.ExoAuthEtRoleSolution.Models;

namespace ExoAuthEtRoleSolution.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ExoAuthEtRoleSolution.Models.ExoAuthEtRoleSolution.Models.Vetement> Vetement { get; set; }
    }
}
