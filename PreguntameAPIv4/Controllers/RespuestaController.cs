using Compartido.DTOs.Respuestas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Negocio.Excepciones;
using PreguntameAPIv4.Utilidades;
using Servicios.Respuestas;

namespace PreguntameAPIv4.Controllers
{
    [Route("api/respuesta")]
    [ApiController]
    public class RespuestaController : ControllerBase
    {
        private readonly IRespuestaServicios _respuestaServicios;
        public RespuestaController(IRespuestaServicios respuestaServicios)
        {
            _respuestaServicios = respuestaServicios;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        [HttpGet("{respuestaId}")]
        public async Task<IActionResult> GetPorId(string respuestaId)
        {
            try
            {
                RespuestaDTO? res = await _respuestaServicios.SelectPorId(respuestaId);
                return res is null ? NotFound(new { message = "No se encontró la respuesta" }) : Ok(res);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, new { message = "Algo no salió correctamente, por favor intente nuevamente" });
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("respuestas-usuario/{usuarioId}")]
        public async Task<IActionResult> GetPorUsuarioId(string usuarioId)
        {
            try
            {
                IEnumerable<RespuestaDTO> res = await _respuestaServicios.SelectPorUsuarioId(usuarioId);
                return res.Any() ? Ok(res) : NotFound(new { message = "El usuario no tiene respuestas" });
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, new { message = "Algo no salió correctamente, por favor intente nuevamente" });
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RespuestaInsertDTO dto)
        {
            string? usuarioId = ObtenerUsuarioEnToken.UsuarioId(HttpContext);
            if(usuarioId == null)
            {
                Tokens.CerrarSesion(Response);
                return Unauthorized("Fallo en la autenticación. Por favor inicie sesión nuevamente");
            }

            try
            {
                await _respuestaServicios.Insert(dto, usuarioId);
                return Ok(new { message = "Respuesta enviada" });
            }
            catch (RespuestaException rex)
            {
                return BadRequest(new { message = rex.Message });
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, new { message = "Algo no salió correctamente, por favor intente nuevamente" });
            }
        }


        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        [HttpDelete("{respuestaId}")]
        public async Task<IActionResult> Delete(string respuestaId)
        {
            string? usuarioId = ObtenerUsuarioEnToken.UsuarioId(HttpContext);
            if (usuarioId == null)
            {
                Tokens.CerrarSesion(Response);
                return Unauthorized("Fallo en la autenticación. Por favor inicie sesión nuevamente");
            }

            try
            {
                await _respuestaServicios.Delete(respuestaId, usuarioId);
                return NoContent();
            }
            catch(RespuestaException rex)
            {
                return BadRequest(new { message = rex.Message });
            }
            catch(ForbiddenException fex)
            {
                return StatusCode(403, new { message = fex.Message });
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, new { message = "Algo no salió correctamente, por favor intente nuevamente" });
            }
        }

        [Authorize]
        [HttpGet("me-gusta/{respuestaId}")]
        public async Task<IActionResult> MeGusta(string respuestaId)
        {
            string? usuarioId = ObtenerUsuarioEnToken.UsuarioId(HttpContext);
            if(usuarioId == null)
            {
                Tokens.CerrarSesion(Response);
                return Unauthorized("Fallo en la autenticación. Por favor inicie sesión nuevamente");
            }

            try
            {
                await _respuestaServicios.ToggleMeGusta(respuestaId, usuarioId);
                return Ok(new { message = "Acción realizada correctamente" });
            }
            catch(RespuestaException rex)
            {
                return BadRequest(new { message = rex.Message });
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, new { message = "Algo no salió correctamente, por favor intente nuevamente" });
            }
        }

    }
}
