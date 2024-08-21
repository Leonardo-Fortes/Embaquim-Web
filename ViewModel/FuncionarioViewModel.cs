using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Web_Embaquim.Models;

namespace Web_Embaquim.ViewModel
{
    public class FuncionarioViewModel
    {
        public int Id { get; set; }

        public string? NomeFunc { get; set; }

        public string? NomeFuncPerfil { get; set; }




        public string? EmailFunc { get; set; }

        public DateTime DataNascimento { get; set; }


        public string? Funcao { get; set; }

        public int PontosRec { get; set; }

        public int PontosRecPerfil { get; set; }

        public int PontosDis { get; set; }


        public int RecAmigavel { get; set; }


        public int RecInovador { get; set; }


        public int RecProtagonista { get; set; }


        public int RecProfissionalismo { get; set; }


        public int IdUsu { get; set; }

        public Usuario? Usuario { get; set; }

        public string FotoPerfil { get; set; }


    }
}
