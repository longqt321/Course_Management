using asp_project.Models;
using Microsoft.EntityFrameworkCore;

namespace asp_project.Data
{
    public class MyAppContext : DbContext
    {
        public MyAppContext(DbContextOptions<MyAppContext> options) : base(options)
        {
        }
        public DbSet<Student> Students { get; set; }
    }
}
