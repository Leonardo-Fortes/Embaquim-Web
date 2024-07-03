namespace Web_Embaquim.Models
{
    public class VerificaUsuario
    {
        private readonly Context _context;

        public VerificaUsuario(Context context)
        {
            _context = context;
        }

        public string Usuario { get; set; }

        public string Senha { get; set; }

        public static int IdFunc { get; set; }

        public int PontosDis { get; set; }

        public int PontosRec { get; set; }

        public bool VerificaLogin()
        {
            var usuarioFunc = _context.Usuarios.Where(u => u.Name == this.Usuario && u.Senha == this.Senha)
                .Select(u => new
                {
                    u.Id
                })
                .FirstOrDefault();

            if (usuarioFunc != null)
            {
                IdFunc = usuarioFunc.Id;
                var pontosFunc = _context.Funcionarios
                    .Where(u => u.IdUsu == IdFunc)
                    .Select(u => new
                    {
                        u.PontosDis,
                        u.PontosRec
                    })
                    .FirstOrDefault();

                // Verifica se pontosFunc é null antes de acessar PontosDis
                if (pontosFunc == null || pontosFunc.PontosDis == null)
                {
                    PontosDis = 0;
                }
                else
                {
                    PontosDis = pontosFunc.PontosDis;
                }

                if (pontosFunc == null || pontosFunc.PontosRec == null)
                {
                    PontosRec = 0;
                }
                else
                {
                    PontosRec = pontosFunc.PontosRec;
                }

                return true;
            }

            IdFunc = 0;
            return false;

        }
    }
}
