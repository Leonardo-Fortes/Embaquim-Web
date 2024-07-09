using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_Embaquim.Models
{

    public class ReconhecerViewModel
    {
        public int IdFuncRec { get; set; }

        public int Medalha { get; set; }

        public string? Nome { get; set; }

        public int Pontos { get; set; }

        public string? Msg { get; set; }
    }
}
