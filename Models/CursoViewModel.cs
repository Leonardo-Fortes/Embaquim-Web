namespace Web_Embaquim.Models
{
    public class CursoViewModel
    {
        public string Tema { get; set; }
        public string Descricao { get; set; }
        public string DuracaoHr { get; set; } // Representando a duração como string "HH:mm"
        public DateTime DataFim { get; set; }
        public int Pontos { get; set; }

        public string LinkCurso { get; set; }
    }
}
