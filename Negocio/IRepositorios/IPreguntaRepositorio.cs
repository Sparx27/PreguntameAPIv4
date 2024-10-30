using Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.IRepositorios
{
    public interface IPreguntaRepositorio
    {
        Task<Pregunta?> SelectPorId(Guid preguntaId);
        Task<List<Pregunta>> SelectPorUsuarioId(Guid usuarioId);
        Task Insert(Pregunta p);
        Task Delete(Guid preguntaId, Guid usuarioId);
    }
}
