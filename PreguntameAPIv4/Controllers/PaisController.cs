using Compartido.DTOs.Paises;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Negocio.Entidades;
using Servicios.Paises;

namespace PreguntameAPIv4.Controllers
{
    [Route("api/paises")]
    [ApiController]
    public class PaisController : ControllerBase
    {
        private readonly IPaisServicios _paisServicios;
        
        public PaisController(IPaisServicios paisServicios)
        {
            _paisServicios = paisServicios;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _paisServicios.SelectAll());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Algo no salió correctamente obteniendo los Paises, por favor nuevamente");
            }
        }
    }
}
