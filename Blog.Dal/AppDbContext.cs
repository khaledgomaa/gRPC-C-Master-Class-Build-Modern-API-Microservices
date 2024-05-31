using Blogg.Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blogg.Dal
{
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost\\MSSQLSERVER01;database=BlogDb;trusted_connection=true;Encrypt=false;TrustServerCertificate=true;");
        }

        public DbSet<Blog> Blogs { get; set; }
    }
}
