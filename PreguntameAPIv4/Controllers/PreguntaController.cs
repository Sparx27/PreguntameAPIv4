using Compartido.DTOs.Preguntas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreguntameAPIv4.Utilidades;
using Servicios.Preguntas;
using Negocio.Excepciones;

namespace PreguntameAPIv4.Controllers
{
    [Route("api/pregunta")]
    [ApiController]
    public class PreguntaController : ControllerBase
    {
        private readonly IPreguntaServicios _preguntaServicios;
        public PreguntaController(IPreguntaServicios preguntaServicios)
        {
            _preguntaServicios = preguntaServicios;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetPorUsuarioId()
        {
            string? usuarioId = ObtenerUsuarioEnToken.UsuarioId(HttpContext);
            if (String.IsNullOrEmpty(usuarioId))
            {
                Tokens.CerrarSesion(Response);
                return Unauthorized("Fallo en la autenticación. Por favor inicie sesión nuevamente");
            }

            try
            {
                return Ok(await _preguntaServicios.SelectPorUsuarioId(usuarioId));
            }
            catch (PreguntaException pex)
            {
                return BadRequest(new { message = pex.Message });
            }
            catch (UsuarioException uex)
            {
                return BadRequest(new { message =  uex.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, new { message = "Algo no salió correctamente. Por favor intente nuevamente" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PreguntaInsertDTO dto)
        {
            string? usuarioEnvia = ObtenerUsuarioEnToken.UsuarioId(HttpContext);
            if (usuarioEnvia != null) dto.UsuarioEnvia = usuarioEnvia;
            else dto.UsuarioEnvia = null; // Este reasignamiente es pensado en un caso poco probable, en donde una persona con conocimientos
                                          // intentara hacerse pasar por otro usuario al enviar la pregunta..
            try
            {
                await _preguntaServicios.Insert(dto);
                return Ok(new { message = "Pregunta enviada exitosamente" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Algo no salió correctamente, por favor intente nuevamente");
            }
        }

        [Authorize]
        [HttpDelete("{preguntaId}")]
        public async Task<IActionResult> Delete(string preguntaId)
        {
            string? usuarioId = ObtenerUsuarioEnToken.UsuarioId(HttpContext);
            if (String.IsNullOrEmpty(usuarioId))
            {
                Tokens.CerrarSesion(Response);
                return Unauthorized("Fallo en la autenticación. Por favor inicie sesión nuevamente");
            }

            try
            {
                await _preguntaServicios.Delete(preguntaId, usuarioId);
                return NoContent();
            }
            catch(PreguntaException pex)
            {
                return BadRequest(new { message = pex.Message });
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Algo no salió correctamente, por favor intente nuevamente");
            }
        }
    }
}
