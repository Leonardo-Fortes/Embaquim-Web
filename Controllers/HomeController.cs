using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Web_Embaquim.Models;

namespace Web_Embaquim.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Context _context;

        public HomeController(ILogger<HomeController> logger, Context context)
        {
            _logger = logger;
            _context = context;
        }

        
        [HttpPost]
        public IActionResult SalvarCurso([FromBody] CursoViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Buscar o registro existente
                var cursoExistente = _context.Cursos.FirstOrDefault();

                // Definir o TimeSpan para Duracao
                TimeSpan duracao;
                if (TimeSpan.TryParseExact(model.DuracaoHr, "hh\\:mm", null, out duracao) ||
                    TimeSpan.TryParseExact(model.DuracaoHr, "mm", null, out duracao))
                {
                    // Parsing successful
                }
                else
                {
                    // Parsing failed, handle the error
                    return Json(new { success = false, message = "Invalid duration format" });
                }

                if (cursoExistente != null)
                {
                    // Atualizar os campos do registro existente
                    cursoExistente.TemaCurso = model.Tema;
                    cursoExistente.DescCurso = model.Descricao;
                    cursoExistente.DuracaoCurso = duracao; // Use the parsed TimeSpan
                    cursoExistente.DataFim = model.DataFim;
                    cursoExistente.PontosCurso = model.Pontos;
                    cursoExistente.LinkCurso = model.LinkCurso;

                    // Salvar as alterações
                    _context.Cursos.Update(cursoExistente);
                }
                else
                {
                    // Criar um novo registro se não existir
                    var novoCurso = new Cursos
                    {
                        TemaCurso = model.Tema,
                        DescCurso = model.Descricao,
                        DuracaoCurso = duracao, // Use the parsed TimeSpan
                        DataFim = model.DataFim,
                        PontosCurso = model.Pontos,
                        LinkCurso = model.LinkCurso
                    };

                    _context.Cursos.Add(novoCurso);
                }

                _context.SaveChanges();

                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

       
        public IActionResult Index()
        {
            var curso = _context.Cursos.FirstOrDefault();
           
                var viewModel = new CursoViewModel
                {
                    Tema = curso.TemaCurso,
                    Descricao = curso.DescCurso,
                    DuracaoHr = curso.DuracaoCurso.ToString(@"hh\:mm"), // Converte TimeSpan para string no formato "hh:mm"
                    DataFim = curso.DataFim,
                    Pontos = curso.PontosCurso,
                    LinkCurso = curso.LinkCurso

                };

                return View(viewModel);
            
            
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Perfil()
        {
            return View();
        }

        public IActionResult Reconhecer()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
