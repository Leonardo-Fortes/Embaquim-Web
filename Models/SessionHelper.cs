namespace Web_Embaquim.Models
{
    public class SessionHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetFotoPerfil()
        {
            return _httpContextAccessor.HttpContext.Session.GetString("FotoPerfil") ?? "/uploads/default/default.jpg";
        }
    }
}
