using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Web_Embaquim.Models;

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

        public IActionResult Index()
        {
            var idteste = VerificaUsuario.IdFunc;
            var pontos = _context.Funcionarios.Where(u => u.IdUsu == idteste).Select(u => new
            {
                u.PontosDis,
                u.PontosRec
            }).FirstOrDefault();


            var viewModelRec = new FuncionarioViewModel
            {
                PontosDis = pontos.PontosDis,
                PontosRec = pontos.PontosRec
            };

            var combinedViewModel = new CombinedViewModel
            {
                FuncionarioViewModel = viewModelRec,
            };

            return View(combinedViewModel);
        }
        [HttpGet]
        public IActionResult BuscarUsuarios(string prefixo)
        {
            var usuarios = _context.Funcionarios
          .Where(u => EF.Functions.Like(u.NomeFunc, prefixo + "%"))
          .Select(u => new { u.Id, u.NomeFunc })
          .ToList();

            return Json(usuarios);
        }
        [HttpPost]
        public IActionResult EnviarReconhecer([FromBody] ReconhecerViewModel recModel)
        {
            var nameFunc = _context.Funcionarios
                .Where(u => u.IdUsu == VerificaUsuario.IdFunc)
                .Select(u => new { u.NomeFunc, u.Id })
                .FirstOrDefault();

            int mes = DateTime.Now.Month;
            int ano = DateTime.Now.Year;

            // Consulta para verificar se existe um registro para o mesmo mês e ano
            var validacao = _context.Reconhecer
                .Any(u => u.NomeFunc == recModel.Nome
                          && u.Mes.Month == mes
                          && u.Mes.Year == ano);

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
                    Profissionalismo = profissionalismo
                };

                _context.Reconhecer.Add(reconhecer);
               

                var funcionarioRec = _context.Funcionarios.FirstOrDefault(x => x.Id == recModel.IdFuncRec);

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

                    var funcionarioEnv = _context.Funcionarios.FirstOrDefault(f => f.Id == VerificaUsuario.IdFunc);
                    if (funcionarioEnv != null)
                    {
                        funcionarioEnv.PontosDis -= recModel.Pontos;
                    }

                    _context.SaveChanges();

                    return Json(new { success = true });
                
            }

            return Json(new { success = false, message = "Invalid model state" });
        }

    }
}
