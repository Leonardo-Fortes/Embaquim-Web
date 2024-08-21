using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using Web_Embaquim.Models;
using Web_Embaquim.ViewModel;

namespace Web_Embaquim.Controllers
{

    public class PerfilController : Controller
    {
        private readonly ILogger<PerfilController> _logger;
        private readonly Context _context;

        // Construtor para injetar as dependências do logger e do contexto
        public PerfilController(ILogger<PerfilController> logger, Context context)
        {
            _logger = logger;
            _context = context;
        }
        [Authorize]
        public async Task<IActionResult> Index(int? id)
        {


            if (id.HasValue)
            {
               
                var idPerfil = VerificaUsuario.IdFunc;



                var funcName = await _context.Funcionarios
                .Where(u => u.IdUsuario == idPerfil)
                .Select(u => new
                {
                    u.NomeFunc,
                    u.PontosRec
                }).FirstOrDefaultAsync();


                var func = await _context.Funcionarios
                 .Where(u => u.Id == id)
                 .Select(u => new
                 {
                     u.PontosDis,
                     u.PontosRec,
                     u.EmailFunc,
                     u.NomeFunc,
                     u.FotoUrl,
                     u.DataNascimento,
                     u.Funcao,
                     u.RecAmigavel,
                     u.RecInovador,
                     u.RecProfissionalismo,
                     u.RecProtagonista
                 })
                 .FirstOrDefaultAsync();


                var viewModell = new FuncionarioViewModel
                {
                    PontosDis = func.PontosDis,
                    PontosRec = func.PontosRec,
                    PontosRecPerfil = funcName.PontosRec,
                    EmailFunc = func.EmailFunc,
                    NomeFunc = func.NomeFunc,
                    NomeFuncPerfil = funcName?.NomeFunc,
                    DataNascimento = func.DataNascimento,
                    Funcao = func.Funcao,
                    RecProtagonista = func.RecProtagonista,
                    RecProfissionalismo = func.RecProfissionalismo,
                    RecInovador = func.RecInovador,
                    RecAmigavel = func.RecAmigavel,
                    FotoPerfil = func.FotoUrl

                };



                var combinedViewModel = new CombinedViewModel
                {
                    FuncionarioViewModel = viewModell,
                };

                HttpContext.Session.SetInt32("IdFunc", (int)id);
               

                return View(combinedViewModel);

            }
            else
            {
                var idFunc = VerificaUsuario.IdFunc;


                var func = await _context.Funcionarios
                    .Where(u => u.IdUsuario == idFunc)
                    .Select(u => new
                    {
                        u.PontosDis,
                        u.PontosRec,
                        u.EmailFunc,
                        u.NomeFunc,
                        u.DataNascimento,
                        u.Funcao,
                        u.RecAmigavel,
                        u.RecInovador,
                        u.RecProfissionalismo,
                        u.RecProtagonista,
                        u.FotoUrl
                    })
                    .FirstOrDefaultAsync();

                if (idFunc == 0 || func == null)
                {
                    return RedirectToAction("Index", "Acesso"); // Redireciona para a página de login se o idFunc não for válido ou curso não for encontrado
                }

                var viewModel = new FuncionarioViewModel
                {
                    PontosDis = func.PontosDis,
                    PontosRecPerfil = func.PontosRec,
                    PontosRec = func.PontosRec,
                    EmailFunc = func.EmailFunc,
                    NomeFunc = func.NomeFunc,
                    NomeFuncPerfil = func.NomeFunc,
                    DataNascimento = func.DataNascimento,
                    Funcao = func.Funcao,
                    RecProtagonista = func.RecProtagonista,
                    RecProfissionalismo = func.RecProfissionalismo,
                    RecInovador = func.RecInovador,
                    RecAmigavel = func.RecAmigavel,
                    FotoPerfil = func.FotoUrl
                };

                var combinedViewModel = new CombinedViewModel
                {
                    FuncionarioViewModel = viewModel,

                };

                return View(combinedViewModel);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetData()
        {
            var idFunc = HttpContext.Session.GetInt32("IdFunc");

            if (idFunc.HasValue)
            {
                  var func = await _context.Funcionarios
                     .Where(u => u.Id == idFunc)
                     .Select(u => new
                     {
                         u.Id,
                     })
                     .FirstOrDefaultAsync();



                if (func == null)
                {
                    return Json(new { success = false, message = "Funcionário não encontrado." });
                }

                var dados = await (from r in _context.Reconhecer
                                   join f in _context.Funcionarios.AsNoTracking() on r.IdFuncEnvio equals f.Id
                                   where r.IdFunc == func.Id
                                   let colunaComValor1 = r.Amigavel == 1 ? "Coluna1" :
                                                         r.Inovador == 1 ? "Coluna2" :
                                                         r.Protagonista == 1 ? "Coluna3" :
                                                         r.Profissionalismo == 1 ? "Coluna4" : null
                                   where colunaComValor1 != null
                                   orderby r.Mes descending
                                   select new
                                   {
                                       Nome = r.NomeFuncEnvio,
                                       Data = r.Mes.ToString("dd/MM/yyyy"),
                                       Texto = r.MSG,
                                       Pontos = r.Pontos,
                                       FotoUrl = f.FotoUrl,
                                       Cargo = f.Funcao,
                                       Medalha = colunaComValor1
                                   })
                         .Take(3)
                         .ToListAsync();
                HttpContext.Session.Remove("IdFunc"); // Limpa o ID da sessão se não foi passado nenhum ID na requisição
                if (dados.Any())
                {
                    return Json(dados);
                }
                return Json("erro");

            }

            else
            {
                var idFuncPerfil = VerificaUsuario.IdFunc;
                var func = await _context.Funcionarios
                    .Where(u => u.IdUsuario == idFuncPerfil)
                    .Select(u => new
                    {
                        u.Id,
                    })
                    .FirstOrDefaultAsync();

                if (func == null)
                {
                    return Json(new { success = false, message = "Funcionário não encontrado." });
                }

                var dados = await (from r in _context.Reconhecer
                                   join f in _context.Funcionarios.AsNoTracking() on r.IdFuncEnvio equals f.Id
                                   where r.IdFunc == func.Id
                                   let colunaComValor1 = r.Amigavel == 1 ? "Coluna1" :
                                                         r.Inovador == 1 ? "Coluna2" :
                                                         r.Protagonista == 1 ? "Coluna3" :
                                                         r.Profissionalismo == 1 ? "Coluna4" : null
                                   where colunaComValor1 != null
                                   orderby r.Mes descending
                                   select new
                                   {
                                       Nome = r.NomeFuncEnvio,
                                       Data = r.Mes.ToString("dd/MM/yyyy"),
                                       Texto = r.MSG,
                                       Pontos = r.Pontos,
                                       FotoUrl = f.FotoUrl,
                                       Cargo = f.Funcao,
                                       Medalha = colunaComValor1
                                   })
                         .Take(3)
                         .ToListAsync();

                if (dados.Any())
                {
                    return Json(dados);
                }
                return Json("erro");
            }
        }
    }
}
