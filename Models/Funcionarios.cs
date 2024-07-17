using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_Embaquim.Models
{
    [Table("Funcionarios")]
    public class Funcionarios
    {
        [Key]
        [Column("idFunc")]
        public int Id { get; set; }

        [Column("nomeFunc")]
        public string NomeFunc { get; set; }

        [Column("emailFunc")]
        public string EmailFunc { get; set; }

        [Column("dataNasc")]
        public DateTime DataNascimento { get; set; }

        [Column("funcao")]
        public string Funcao { get; set; }

        [Column("pontosRec")]
        public int PontosRec { get; set; }

        [Column("pontosDis")]
        public int PontosDis { get; set; }

        [Column("recAmigavel")]
        public int RecAmigavel { get; set; }

        [Column("recInovador")]
        public int RecInovador { get; set; }

        [Column("recProtagonista")]
        public int RecProtagonista { get; set; }

        [Column("recProfissionalismo")]
        public int RecProfissionalismo { get; set; }
        [Column("fotoUrl")]
        public string FotoUrl { get; set; }

        [ForeignKey("Usuario")]
        [Column("id_Usu")]
        public int IdUsuario { get; set; }

        public Usuario? Usuario { get; set; }

        
        public ICollection<Reconhecer> Reconhecimentos { get; set; }
    }
}
