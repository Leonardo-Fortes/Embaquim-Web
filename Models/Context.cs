using Microsoft.EntityFrameworkCore;

namespace Web_Embaquim.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {

        }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}
