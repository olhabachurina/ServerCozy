using Microsoft.EntityFrameworkCore;
using ServerCozy.Models;
using System.Collections.Generic;

namespace ServerCozy.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
