using Azure;

namespace PreguntameAPIv4.Utilidades
{
    public static class Tokens
    {
        public static void CerrarSesion(HttpResponse res)
        {
            res.Cookies.Delete("jwttoken");
            res.Cookies.Append("jwttoken", "", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddHours(-1)
            });
        }
    }
}
