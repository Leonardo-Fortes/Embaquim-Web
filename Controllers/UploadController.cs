
//using Microsoft.AspNetCore.Mvc;
//using Web_Embaquim.Models;

//namespace Web_Embaquim.Controllers
//{
//    public class UploadController : Controller
//    {
//        private readonly IAmazonS3 _s3Client;
//        private readonly IConfiguration _configuration;
//        private readonly Context _context;

//        public UploadController(IAmazonS3 s3Client, IConfiguration configuration, Context context)
//        {
//            _s3Client = s3Client;
//            _configuration = configuration;
//            _context = context;
//        }

//        [HttpPost]
//        public async Task<IActionResult> UploadImagem(IFormFile file)
//        {
//            if (file == null || file.Length == 0)
//            {
//                return Content("Arquivo não selecionado");
//            }

//            var bucketName = _configuration["AWS:BucketName"];
//            var keyName = Guid.NewGuid() + Path.GetExtension(file.FileName);

//            using (var stream = new MemoryStream())
//            {
//                await file.CopyToAsync(stream);
//                var request = new PutObjectRequest
//                {
//                    BucketName = bucketName,
//                    Key = keyName,
//                    InputStream = stream,
//                    ContentType = file.ContentType,
//                    AutoCloseStream = true
//                };

//                await _s3Client.PutObjectAsync(request);
//            }

//            // Salvar a URL da imagem no banco de dados
//            var usuarioId = VerificaUsuario.IdFunc;
//            var funcionario = _context.Funcionarios.FirstOrDefault(f => f.IdUsuario == usuarioId);
//            if (funcionario != null)
//            {
//                funcionario.FotoUrl = $"https://{bucketName}.s3.amazonaws.com/{keyName}";
//                _context.SaveChanges();
//            }

//            return RedirectToAction("Index", "Home");
//        }
//    }
//}
