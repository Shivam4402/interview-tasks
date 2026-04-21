using EF_WEBAPI_CRUD.Models;
using Microsoft.EntityFrameworkCore;

namespace EF_WEBAPI_CRUD.Data
{
    public class AppDbContext : DbContext
    {
       public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Student> Students { get; set; }
    }
}
