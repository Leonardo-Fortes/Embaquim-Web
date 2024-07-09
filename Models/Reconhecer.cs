using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_Embaquim.Models
{
    [Table("Reconhecimento")]
    public class Reconhecer
    {
        [ForeignKey("Funcionario")]
        [Column("id_Func")]
        public int IdFunc { get; set; }

        [Key]
        [Column("idRec")]
        public int Id { get; set; }
        [Column("mes")]
        public DateTime Mes {  get; set; } 
        [Column("funcRec")]
        public string NomeFunc { get; set; }

        [Column("funcEnvio")]
        public string NomeFuncEnvio { get; set; }

        [Column("pontosEnviado")]
        public int Pontos { get; set; }

        [Column("recAmigavel")]
        public int Amigavel { get; set; }

        [Column("recInovador")]
        public int Inovador { get; set; }

        [Column("recProtagonista")]
        public int Protagonista { get; set; }

        [Column("recProfissionalismo")]
        public int Profissionalismo { get; set; }

        [Column("recMsg")]
        public string MSG { get; set; }

       
  

        public Funcionarios Funcionario { get; set; }
    }
}
