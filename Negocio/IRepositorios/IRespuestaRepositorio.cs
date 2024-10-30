using Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.IRepositorios
{
    public interface IRespuestaRepositorio
    {
        Task<Respuesta?> SelectPorId(Guid respuestaId);
        Task<List<Respuesta>> SelectPorUsuarioId(Guid usuarioId);
        Task Insert(Respuesta respuesta, Guid usuarioId);
        Task Delete(Guid respuestaId, Guid usuarioId);
    }
}
