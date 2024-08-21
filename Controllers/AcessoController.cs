using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;
using Web_Embaquim.Models;
using Microsoft.AspNetCore.Hosting;
using System.ComponentModel.DataAnnotations;
using Web_Embaquim.ViewModel;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> ChecarLoginAsync(string usuario, string senha)
        {
            HttpContext.Session.Clear();
            var login = new VerificaUsuario(_context)
            {
                Usuario = usuario,
                Senha = senha,
            };

            // Verifica se o login é válido
            if (!login.VerificaLogin())
            {
                TempData["MensagemErro"] = "Usuário ou senha inválidos";
                return RedirectToAction("Index");
            }

            // Cria uma lista de claims para autenticação
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10)  // Ajuste o tempo de expiração conforme necessário
            };

            // Sign in do usuário com cookie de autenticação
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            // Busca o funcionário com base no ID do usuário
            int idUsuario = VerificaUsuario.IdFunc;
          

       

            return RedirectToAction("Index", "Home");
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


        // Método para upload de foto
        [HttpPost]
        public async Task<IActionResult> UploadImagemAsync(string image)
        {
            // Verifica se a imagem foi enviada
            if (string.IsNullOrEmpty(image))
            {
                return Json(new { success = false, message = "Imagem não enviada" });
            }

            // Remove o prefixo "data:image/png;base64," ou similar
            var data = image.Substring(image.IndexOf(",") + 1);
            byte[] imageBytes = Convert.FromBase64String(data);

            // Verifica o tamanho da imagem
            if (imageBytes.Length > 2 * 1024 * 1024) // Limite de 2 MB
            {
                return Json(new { success = false, message = "Arquivo muito grande. O limite é de 2 MB." });
            }

            // Define o nome único do arquivo
            var uniqueFileName = Guid.NewGuid().ToString() + ".png"; // Salva como PNG
            var uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads", "user");

            // Cria o diretório se ele não existir
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Salva o arquivo no caminho especificado
            await System.IO.File.WriteAllBytesAsync(filePath, imageBytes);

            // Atualiza a URL da foto do funcionário
            var usuarioId = VerificaUsuario.IdFunc;
            var funcionario = await _context.Funcionarios.FirstOrDefaultAsync(f => f.IdUsuario == usuarioId);

            if (funcionario != null)
            {
                funcionario.FotoUrl = "/uploads/user/" + uniqueFileName;
                await _context.SaveChangesAsync();
            }

            return Json(new { success = true, imageUrl = funcionario.FotoUrl });
        }




        // Método para escolher uma foto predefinida
        public async Task<IActionResult> EscolherFotoPreDefinidaAsync(string fotoNome)
        {
            var usuarioId = VerificaUsuario.IdFunc;
            var funcionario = await _context.Funcionarios.FirstOrDefaultAsync(f => f.IdUsuario == usuarioId);
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
                await _context.SaveChangesAsync();
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
