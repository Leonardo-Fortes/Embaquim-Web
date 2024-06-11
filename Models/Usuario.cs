using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_Embaquim.Models
{
    [Table("Usuario")]
    public class Usuario
    {
        [Key]
        [Column("idUsu")]
        public int Id { get; set; }

        [Column("usuario")]
        public string? Name { get; set; }

        [Column("senha")]
        public string? Senha { get; set; }

    }
}
