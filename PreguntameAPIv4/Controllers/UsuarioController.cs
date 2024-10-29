using Compartido.DTOs.Usuario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Negocio.Excepciones;
using PreguntameAPIv4.Utilidades;
using Servicios.Usuarios;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PreguntameAPIv4.Controllers
{
    [Route("api/usuario")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioServicios _serviciosUsuarios;
        public UsuarioController(IUsuarioServicios serviciosUsuarios)
        {
            _serviciosUsuarios = serviciosUsuarios;
        }

        [HttpPost("iniciar-sesion")]
        public async Task<IActionResult> IniciarSesion([FromBody] CredencialesDTO credenciales)
        {
            try
            {
                var res = await _serviciosUsuarios.IniciarSesion(credenciales.Email, credenciales.Contrasena);
                Response.Cookies.Append("jwttoken", res.Item2, new CookieOptions
                {
                    Secure = true,
                    HttpOnly = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddHours(1)
                });
                return Ok(res.Item1);
            }
            catch (UsuarioException uex)
            {
                return BadRequest(new { message = uex.Message });
            }
            catch (Exception ex)
            {
                // En vez de un simple console, se podría guardar en algún log
                Console.WriteLine(ex.Message);
                return StatusCode(500, new { message = "Ocurrió un error, por favor intente nuevamente" });
            }
        }

        [HttpGet("cerrar-sesion")]
        public IActionResult CerrarSesion()
        {
            Tokens.CerrarSesion(Response);
            return Ok(new { Message = "Sesión cerrada" });
        }

        [Authorize]
        [HttpPatch("datos/actualizar")]
        public async Task<IActionResult> PatchDatosUsuario([FromBody] UsuarioDatosDTO dto)
        {
            string? nombreUsuario = ObtenerUsuarioEnToken.NombreUsuario(HttpContext);
            if (String.IsNullOrEmpty(nombreUsuario))
            {
                Tokens.CerrarSesion(Response);
                return Unauthorized("Fallo en la autenticación. Por favor inicie sesión nuevamente");
            }

            try
            {
                await _serviciosUsuarios.UpdateDatosUsuario(nombreUsuario, dto);
                return Ok(dto);
            }
            catch(UsuarioException uex)
            {
                return BadRequest(new { message = uex.Message });
            }
            catch(ConflictException cex)
            {
                return Conflict(new { message = cex.Message });
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, new { message = "Algo no salió correctamente, por favor intente nuevamente" });
            }
        }

        [Authorize]
        [HttpPatch("datos/actualizar-contrasena")]
        public async Task<IActionResult> PatchContrasena([FromBody] CredencialesDTO dto)
        {
            string? nombreUsuario = ObtenerUsuarioEnToken.NombreUsuario(HttpContext);
            if (String.IsNullOrEmpty(nombreUsuario))
            {
                Tokens.CerrarSesion(Response);
                return Unauthorized("Fallo en la autenticación. Por favor inicie sesión nuevamente");
            }

            try
            {
                await _serviciosUsuarios.UpdateContrasena(nombreUsuario, dto.Contrasena);
                return Ok(new { message = "Contraseña actualizada" });
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("registro")]
        public async Task<IActionResult> Registro([FromBody] UsuarioInsertDTO dto)
        {
            try
            {
                await _serviciosUsuarios.Insert(dto);
                return Ok(new { message = "Usuario registrado con éxito, puede iniciar sesión" });
            }
            catch (UsuarioException uex)
            {
                return BadRequest(new { message = uex.Message });
            }
            catch(ConflictException cex)
            {
                return Conflict(new { message = cex.Message });
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, new { message = "Algo no salió correctamente, por favor intente nuevamente" });
            }
        }
    }
}
