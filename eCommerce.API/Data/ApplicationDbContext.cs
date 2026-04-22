using Microsoft.EntityFrameworkCore;

namespace eCommerce.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Models.Usuario> Usuarios { get; set; }
    }
}
