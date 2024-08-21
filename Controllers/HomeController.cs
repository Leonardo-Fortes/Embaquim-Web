using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using Web_Embaquim.Models;
using Web_Embaquim.ViewModel;

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
        public async Task<IActionResult> SalvarCurso([FromBody] CursoViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Buscar o registro existente
                var cursoExistente = await _context.Cursos.FirstOrDefaultAsync();

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

                    await _context.Cursos.AddAsync(novoCurso);
                }

                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        // Método para exibir a página inicial
        public async Task<IActionResult> Index()
        {
            var idFunc = VerificaUsuario.IdFunc;
            var curso = await _context.Cursos.FirstOrDefaultAsync();

            var funcionario = await _context.Funcionarios.FirstOrDefaultAsync(f => f.IdUsuario == idFunc);

            // Se o funcionário for encontrado, armazena a URL da foto na sessão
            if (funcionario != null)
            {
                HttpContext.Session.SetString("FotoPerfil", funcionario.FotoUrl ?? "/uploads/default/default.jpg");
            }


            if (idFunc == 0 || curso == null)
            {
                return RedirectToAction("Index", "Acesso"); // Redireciona para a página de login se o idFunc não for válido ou curso não for encontrado
            }

            var viewModel = new CursoViewModel
            {
                Tema = curso.TemaCurso,
                Descricao = curso.DescCurso,
                DuracaoHr = curso.DuracaoCurso.ToString(@"hh\:mm"), // Converte TimeSpan para string no formato "hh:mm"
                DataFim = curso.DataFim,
                Pontos = curso.PontosCurso,
                LinkCurso = curso.LinkCurso,
            };

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
                    u.RecProtagonista
                }).FirstOrDefaultAsync();



            if (func == null)
            {
                // Lidar com o caso onde o funcionário não é encontrado
                return RedirectToAction("Error", "Home", new { message = "Funcionário não encontrado." });
            }

            var viewModelRec = new FuncionarioViewModel
            {
                PontosDis = func.PontosDis,
                PontosRecPerfil = func.PontosRec,
                EmailFunc = func.EmailFunc,
                NomeFuncPerfil = func.NomeFunc,
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

        // Método para exibir a página de controle de usuário
        public IActionResult ControleUsuario()
        {
            return View();
        }

        [HttpPost]
        // Método para pré-cadastro de usuário
        public async Task<IActionResult> PreCadastroUsuario([FromBody] CadastroViewModel cadastro)
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

                await _context.Usuarios.AddAsync(newUsuario);
                await _context.SaveChangesAsync();

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

                    await _context.Funcionarios.AddAsync(newFunc);
                }

                await _context.SaveChangesAsync();

                return Ok(new { status = "success" });
            }

            return Json(new { success = false, message = "Model state is invalid" });
        }

        [HttpGet]
        public async Task<IActionResult> PesquisaPerfil(string prefixo)
        {
            var idFunc = VerificaUsuario.IdFunc;
            var currentUser = await _context.Funcionarios.AsNoTracking()
                .Where(u => u.IdUsuario == idFunc)
                .Select(u => u.NomeFunc)
                .FirstOrDefaultAsync();


            var usuarios = await _context.Funcionarios.AsNoTracking()
                .Where(u => EF.Functions.Like(u.NomeFunc, prefixo + "%") && u.NomeFunc != currentUser)
                .Select(u => new { u.Id, u.NomeFunc })
                .ToListAsync();

            return Json(usuarios);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        // Método para exibir a página de erro
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
