using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Web_Embaquim.Models
{
    [Table("Cursos")]
    public class Cursos
    {
        [Column("idCurso")]
        [Key]
        public int Id { get; set; }
        
        [Column("temaCurso")]
        public string TemaCurso { get; set; }

        [Column("desCurso")]
        public string DescCurso { get; set; }

        [Column("duracaoCurso")]
        public TimeSpan DuracaoCurso { get; set; }

        [Column("dataFim")]
        public DateTime DataFim { get; set; }

        [Column("pontosCurso")]
        public int PontosCurso { get; set; }

        [Column("linkCurso")]
        public string LinkCurso { get; set; }

    }
}
