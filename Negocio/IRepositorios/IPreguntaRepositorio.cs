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
        Task<List<Pregunta>> SelectPorUsuarioId(Guid usuarioId);
        Task Insert(Pregunta p);
        Task Delete(Pregunta p);
    }
}
