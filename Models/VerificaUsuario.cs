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
                return true;
            }

            IdFunc = 0;
            return false;

        }
    }
}
