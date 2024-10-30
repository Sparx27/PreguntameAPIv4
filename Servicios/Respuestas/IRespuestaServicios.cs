using Compartido.DTOs.Respuestas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios.Respuestas
{
    public interface IRespuestaServicios
    {
        Task<RespuestaDTO?> SelectPorId(string respuestaId);
        Task<IEnumerable<RespuestaDTO>> SelectPorUsuarioId(string usuarioId);
        Task Insert(RespuestaInsertDTO dto, string usuarioId);
        Task Delete(string respuestaId, string usuarioId);
    }
}
