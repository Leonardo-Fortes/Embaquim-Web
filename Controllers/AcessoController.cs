using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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

        [HttpPost]
        public IActionResult ChecarLogin(string usuario, string senha)
        {
            var login = new VerificaUsuario(_context)
            {
                Usuario = usuario,
                Senha = senha,
            };

            if (login.VerificaLogin())
            {
                // Configurar o cookie de autenticação
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["MensagemErro"] = "Usuário ou senha inválidos";
                return RedirectToAction("Index");
            }
        }

        public IActionResult Negado()
        {
            return View(); // Uma view que informa ao usuário que o acesso foi negado
        }
    }

}
