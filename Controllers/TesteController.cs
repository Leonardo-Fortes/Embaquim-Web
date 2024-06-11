using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Embaquim.Models;


namespace Web_Embaquim.Controllers
{
    public class TesteController : Controller
    {
        private readonly Context _context;

        public TesteController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Teste/Conexao")]
        public IActionResult TestarConexao()
        {
            try
            {
                _context.Database.OpenConnection();
                _context.Database.CloseConnection();
                return Ok("Conexão estabelecida com sucesso!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao estabelecer a conexão: {ex.Message}");
            }
        }
    }
}
