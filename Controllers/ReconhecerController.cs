using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Threading.Tasks;
using Web_Embaquim.Models;
using Web_Embaquim.ViewModel;

namespace Web_Embaquim.Controllers
{
    [Authorize]
    public class ReconhecerController : Controller
    {
        private readonly Context _context;
        public ReconhecerController(Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var idteste = VerificaUsuario.IdFunc;
            var pontos = await _context.Funcionarios.AsNoTracking().Where(u => u.IdUsuario == idteste).Select(u => new
            {
                u.PontosDis,
                u.PontosRec,
                u.NomeFunc
            }).FirstOrDefaultAsync();

            if (idteste == 0)
            {
                return RedirectToAction("Index", "Acesso"); // Redireciona para a página de login se o idFunc não for válido ou curso não for encontrado
            }

            var viewModelRec = new FuncionarioViewModel
            {
                PontosDis = pontos.PontosDis,
                PontosRecPerfil = pontos.PontosRec,
                NomeFuncPerfil = pontos.NomeFunc
            };

            var combinedViewModel = new CombinedViewModel
            {
                FuncionarioViewModel = viewModelRec,
            };

            return View(combinedViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> BuscarUsuarios(string prefixo)
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

      

        [HttpPost]
        public async Task<IActionResult> EnviarReconhecer([FromBody] ReconhecerViewModel recModel)
        {
            var nameFunc = await _context.Funcionarios.AsNoTracking()
                .Where(u => u.IdUsuario == VerificaUsuario.IdFunc)
                .Select(u => new { u.NomeFunc, u.Id })
                .FirstOrDefaultAsync();

            int mes = DateTime.Now.Month;
            int ano = DateTime.Now.Year;

            // Consulta para verificar se existe um registro para o mesmo mês e ano
            var validacao = await _context.Reconhecer.AsNoTracking()
                .AnyAsync(u => u.NomeFunc == recModel.Nome
                               && u.Mes.Month == mes
                               && u.Mes.Year == ano && u.NomeFuncEnvio == nameFunc.NomeFunc);

            var idFuncEnvio = await _context.Funcionarios.AsNoTracking()
                .Where(i => i.IdUsuario == VerificaUsuario.IdFunc)
                .Select(i => new { i.Id })
                .FirstOrDefaultAsync();

            if (validacao)
            {
                return Json(new { success = false, message = "Já existe um Reconhecimento para este mês e ano deste colaborador" });
            }

            if (ModelState.IsValid)
            {
                // Variáveis para armazenar os valores das medalhas
                int amigavel = 0, inovador = 0, protagonista = 0, profissionalismo = 0;

                // Definir a medalha apropriada
                switch (recModel.Medalha)
                {
                    case 1:
                        amigavel = recModel.Medalha;
                        break;
                    case 2:
                        inovador = recModel.Medalha;
                        break;
                    case 3:
                        protagonista = recModel.Medalha;
                        break;
                    default:
                        profissionalismo = recModel.Medalha;
                        break;
                }

                var reconhecer = new Reconhecer
                {
                    NomeFunc = recModel.Nome,
                    NomeFuncEnvio = nameFunc.NomeFunc,
                    MSG = recModel.Msg,
                    Mes = DateTime.Now,
                    IdFunc = recModel.IdFuncRec,
                    Pontos = recModel.Pontos,
                    Amigavel = amigavel,
                    Inovador = inovador,
                    Protagonista = protagonista,
                    Profissionalismo = profissionalismo,
                    IdFuncEnvio = idFuncEnvio.Id
                };

                await _context.Reconhecer.AddAsync(reconhecer);

                var funcionarioRec = await _context.Funcionarios.FirstOrDefaultAsync(x => x.Id == recModel.IdFuncRec);

                // Atualizar os pontos do funcionário somando com os novos pontos
                funcionarioRec.PontosRec += recModel.Pontos;

                // Atualizar as medalhas do funcionário
                switch (recModel.Medalha)
                {
                    case 1:
                        funcionarioRec.RecAmigavel += 1;
                        break;
                    case 2:
                        funcionarioRec.RecInovador += 1;
                        break;
                    case 3:
                        funcionarioRec.RecProtagonista += 1;
                        break;
                    default:
                        funcionarioRec.RecProfissionalismo += 1;
                        break;
                }

                var funcionarioEnv = await _context.Funcionarios.FirstOrDefaultAsync(f => f.IdUsuario == VerificaUsuario.IdFunc);
                if (funcionarioEnv != null)
                {
                    funcionarioEnv.PontosDis -= recModel.Pontos;
                }

                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Invalid model state" });
        }
    }
}
