using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using appBD.Models;
using appBD.Controllers;

namespace appBD.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> users { get; set; } = null!;
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {   
        }
    }
}
