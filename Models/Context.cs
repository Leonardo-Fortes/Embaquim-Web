using Microsoft.EntityFrameworkCore;

namespace Web_Embaquim.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cursos> Cursos { get; set; }
        public DbSet<Funcionarios> Funcionarios { get; set; }
        public DbSet<Reconhecer> Reconhecer { get; set; }

    }

}
