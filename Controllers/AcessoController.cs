using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;
using Web_Embaquim.Models;
using Microsoft.AspNetCore.Hosting;
using Web_Embaquim.ViewModel;
using System.ComponentModel.DataAnnotations;

namespace Web_Embaquim.Controllers
{
    public class AcessoController : Controller
    {
        private readonly Context _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        // Construtor para injetar as dependências do contexto e ambiente do host
        public AcessoController(Context context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // Método para exibir a página inicial de login
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        // Método para verificar login
        public IActionResult ChecarLogin(string usuario, string senha)
        {
            var login = new VerificaUsuario(_context)
            {
                Usuario = usuario,
                Senha = senha,
            };

            // Verifica se o login é válido
            if (login.VerificaLogin())
            {
                // Cria uma lista de claims para autenticação
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                // Sign in do usuário com cookie de autenticação
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                // Busca o funcionário com base no ID do usuário
                int idUsuario = VerificaUsuario.IdFunc;
                var funcionario = _context.Funcionarios.FirstOrDefault(f => f.IdUsuario == idUsuario);

                // Se o funcionário for encontrado, armazena a URL da foto na sessão
                if (funcionario != null)
                {
                    HttpContext.Session.SetString("FotoPerfil", funcionario.FotoUrl ?? "/uploads/default/default.jpg");
                }

                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["MensagemErro"] = "Usuário ou senha inválidos";
                return RedirectToAction("Index");
            }
        }

        // Método para exibir a página de primeiro acesso
        public IActionResult PrimeiroAcesso()
        {
            // Obtém a lista de fotos predefinidas do diretório "uploads/default"
            var predefinedPhotos = Directory.EnumerateFiles(Path.Combine(_hostEnvironment.WebRootPath, "uploads", "default"))
                                             .Select(Path.GetFileName)
                                             .ToList();

            var viewModel = new PrimeiroAcessoViewModel
            {
                PredefinedPhotos = predefinedPhotos
            };

            return View(viewModel);
        }

        [HttpPost]
        // Método para upload de foto
        public async Task<IActionResult> UploadFoto(IFormFile file)
        {
            // Verifica se o arquivo foi selecionado
            if (file == null || file.Length == 0)
            {
                return Content("Arquivo não selecionado");
            }

            // Validações de tamanho e tipo de arquivo
            if (file.Length > 2 * 1024 * 1024) // Limite de 2 MB
            {
                return Content("Arquivo muito grande. O limite é de 2 MB.");
            }

            var permittedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            {
                return Content("Tipo de arquivo inválido. Apenas .jpg, .jpeg e .png são permitidos.");
            }

            // Caminho onde o arquivo será salvo
            var uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads", "user");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Salva o arquivo no diretório especificado
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Atualiza a URL da foto do funcionário
            var idFunc = VerificaUsuario.IdFunc;
            var funcionario = _context.Funcionarios.FirstOrDefault(f => f.Id == idFunc);

            if (funcionario != null)
            {
                funcionario.FotoUrl = "/uploads/user/" + uniqueFileName;
                _context.SaveChanges();
            }

            return RedirectToAction("Perfil"); // Redireciona para a ação que mostra o perfil do funcionário
        }

        // Método para escolher uma foto predefinida
        public IActionResult EscolherFotoPreDefinida(string fotoNome)
        {
            var usuarioId = VerificaUsuario.IdFunc;
            var funcionario = _context.Funcionarios.FirstOrDefault(f => f.IdUsuario == usuarioId);
            if (funcionario == null)
            {
                // Log para verificar o motivo da falha
                Console.WriteLine("Funcionário não encontrado para IdUsu: " + usuarioId);
                return Json(new { success = false, message = "Funcionário não encontrado." });
            }

            // Salva a URL completa da foto
            funcionario.FotoUrl = "/uploads/default/" + fotoNome;

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Erro ao salvar a foto de perfil: " + ex.Message });
            }

            return RedirectToAction("Index", "Home");
        }

        // Método para exibir a página de acesso negado
        public IActionResult Negado()
        {
            return View(); // View que informa ao usuário que o acesso foi negado
        }
    }
}
