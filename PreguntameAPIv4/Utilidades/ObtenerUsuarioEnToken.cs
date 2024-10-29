using Negocio.Excepciones;
using System.IdentityModel.Tokens.Jwt;

namespace PreguntameAPIv4.Utilidades
{
    public static class ObtenerUsuarioEnToken
    {
        public static string? NombreUsuario(HttpContext httpContext)
        {
            var token = httpContext.Request.Cookies["jwtToken"];
            string? nombreUsuario = null;
            if (token != null)
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

                nombreUsuario = jwtToken?.Claims.FirstOrDefault(claim => claim.Type == "NombreUsuario")?.Value;
            }
            return nombreUsuario;
        }

        public static string? UsuarioId(HttpContext httpContext)
        {
            var token = httpContext.Request.Cookies["jwtToken"];
            string? usuarioId = null;
            if (token != null)
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

                usuarioId = jwtToken?.Claims.FirstOrDefault(claim => claim.Type == "UsuarioId")?.Value;
            }
            return usuarioId;
        }
    }
}
