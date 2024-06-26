namespace Web_Embaquim.Models
{
    public class CadastroViewModel
    {
        public List<Solicitacao> solicitacao {  get; set; }
        public string Usuario { get; set; }
        public string Senha { get; set; }
    
    }
    public class Solicitacao
    {
        public string NomeCad { get; set; }
        public string SobrenomeCad { get; set; }
        public string CpfCad { get; set; }
        public DateTime DataNasciCad { get; set; }
        public string FuncaoCad { get; set; }
        public string EmailCad { get; set; }
    }
}
