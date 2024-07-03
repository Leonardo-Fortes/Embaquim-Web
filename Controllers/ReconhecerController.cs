using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Embaquim.Models;

namespace Web_Embaquim.Controllers
{
    [Authorize]
    public class ReconhecerController : Controller
    {
        private readonly Context _context;
        public ReconhecerController (Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var verificaUsuario = new VerificaUsuario(_context);
            var combinedViewModel = new CombinedViewModel()
            {
                VerificaUsuario = verificaUsuario
            };
            return View(combinedViewModel);
        }
        [HttpGet]
        public IActionResult BuscarUsuarios(string prefixo)
        {
            var usuarios = _context.Usuarios
          .Where(u => EF.Functions.Like(u.Name, prefixo + "%"))
          .Select(u => new { u.Id, u.Name })
          .ToList();

            return Json(usuarios);
        }

    }
}
