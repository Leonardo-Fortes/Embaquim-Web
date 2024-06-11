using Microsoft.AspNetCore.Mvc;
using Web_Embaquim.Models;

namespace Web_Embaquim.Controllers
{
    public class AcessoController : Controller
    {
        private readonly Context _context;
        public AcessoController(Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult PrimeiroAcesso()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChecarLogin(string usuario, string senha)
        {
            // Crie uma instância da classe Login e defina os parâmetros
            var login = new VerificaUsuario(_context)
            {
                Usuario = usuario,
                Senha = senha,
            };

            // Realize a autenticação
            if (login.VerificaLogin())
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Autenticação falhou, retorne para a página de login com uma mensagem de erro
                TempData["MensagemErro"] = "Usuário ou senha inválidos";
                return RedirectToAction("Index", "Acesso");
            }

        }
    }
}
