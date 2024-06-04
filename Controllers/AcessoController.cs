using Microsoft.AspNetCore.Mvc;

namespace Web_Embaquim.Controllers
{
    public class AcessoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult PrimeiroAcesso()
        {
            return View();
        }
    }
}
