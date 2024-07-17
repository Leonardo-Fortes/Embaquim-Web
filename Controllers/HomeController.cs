using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using Web_Embaquim.Models;

namespace Web_Embaquim.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Context _context;

        // Construtor para injetar as dependências do logger e do contexto
        public HomeController(ILogger<HomeController> logger, Context context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost]
        // Método para salvar um curso
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
                    // Parsing bem-sucedido
                }
                else
                {
                    // Parsing falhou, tratar o erro
                    return Json(new { success = false, message = "Invalid duration format" });
                }

                if (cursoExistente != null)
                {
                    // Atualizar os campos do registro existente
                    cursoExistente.TemaCurso = model.Tema;
                    cursoExistente.DescCurso = model.Descricao;
                    cursoExistente.DuracaoCurso = duracao;
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
                        DuracaoCurso = duracao, // Usar o TimeSpan parsed
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

        // Método para exibir a página inicial
        public IActionResult Index()
        {
            var idFunc = VerificaUsuario.IdFunc;
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

            var func = _context.Funcionarios.Where(u => u.IdUsuario == idFunc).Select(u => new
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
                u.RecProtagonista
            }).FirstOrDefault();

            var viewModelRec = new FuncionarioViewModel
            {
                PontosDis = func.PontosDis,
                PontosRec = func.PontosRec,
                EmailFunc = func.EmailFunc,
                NomeFunc = func.NomeFunc,
                DataNascimento = func.DataNascimento,
                Funcao = func.Funcao,
                RecProtagonista = func.RecProtagonista,
                RecProfissionalismo = func.RecProtagonista,
                RecInovador = func.RecInovador,
                RecAmigavel = func.RecAmigavel
            };

            var combinedViewModel = new CombinedViewModel
            {
                FuncionarioViewModel = viewModelRec,
                CursoViewModel = viewModel
            };

            return View(combinedViewModel);
        }

        // Método para exibir a página de privacidade
        public IActionResult Privacy()
        {
            return View();
        }

        // Método para exibir a página de perfil
        public IActionResult Perfil()
        {
            var idFunc = VerificaUsuario.IdFunc;
            var func = _context.Funcionarios.Where(u => u.IdUsuario == idFunc).Select(u => new
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
                u.RecProtagonista
            }).FirstOrDefault();

            var viewModel = new FuncionarioViewModel
            {
                PontosDis = func.PontosDis,
                PontosRec = func.PontosRec,
                EmailFunc = func.EmailFunc,
                NomeFunc = func.NomeFunc,
                DataNascimento = func.DataNascimento,
                Funcao = func.Funcao,
                RecProtagonista = func.RecProtagonista,
                RecProfissionalismo = func.RecProtagonista,
                RecInovador = func.RecInovador,
                RecAmigavel = func.RecAmigavel
            };

            var fotoPerfil = HttpContext.Session.GetString("FotoPerfil") ?? "/uploads/default/default.jpg";

            var combinedViewModel = new CombinedViewModel
            {
                FuncionarioViewModel = viewModel,
                FotoPerfil = fotoPerfil
            };

            return View(combinedViewModel);
        }
        [HttpGet]
        public JsonResult GetData()
        {
            var idFunc = VerificaUsuario.IdFunc;
            var func = _context.Funcionarios.Where(u => u.IdUsuario == idFunc).Select(u => new
            {
                u.Id,

            }).FirstOrDefault();


            if (func == null)
            {
                return Json(new { success = false, message = "Funcionário não encontrado." });
            }

            var dados = _context.Reconhecer
            .Where(i => i.IdFunc == func.Id)
            .Select(i => new
            {
                Nome = i.NomeFuncEnvio,
                Data = i.Mes.ToString("dd/MM/yyyy"),
                Texto = i.MSG,
                FotoUrl = _context.Funcionarios
                        .Where(f => f.Id == i.IdFuncEnvio)
                        .Select(f => f.FotoUrl)
                        .FirstOrDefault()
            })
            .Take(3)
            .ToList();
             return Json(dados);
        }




        // Método para exibir a página de controle de usuário
        public IActionResult ControleUsuario()
        {
            return View();
        }

        [HttpPost]
        // Método para pré-cadastro de usuário
        public IActionResult PreCadastroUsuario([FromBody] CadastroViewModel cadastro)
        {
            if (cadastro == null || cadastro.solicitacao == null)
            {
                return Json(new { success = false, message = "Dados inválidos" });
            }

            if (ModelState.IsValid)
            {
                var newUsuario = new Usuario
                {
                    Name = cadastro.Usuario,
                    Senha = cadastro.Senha
                };

                _context.Usuarios.Add(newUsuario);
                _context.SaveChanges();

                int newIdUsu = newUsuario.Id;

                foreach (var solicitacao in cadastro.solicitacao)
                {
                    var newFunc = new Funcionarios
                    {
                        NomeFunc = solicitacao.NomeCad,
                        EmailFunc = solicitacao.EmailCad,
                        DataNascimento = solicitacao.DataNasciCad,
                        Funcao = solicitacao.FuncaoCad,
                        IdUsuario = newIdUsu
                    };

                    _context.Funcionarios.Add(newFunc);
                }

                _context.SaveChanges();

                return Ok(new { status = "success" });
            }

            return Json(new { success = false, message = "Model state is invalid" });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        // Método para exibir a página de erro
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
