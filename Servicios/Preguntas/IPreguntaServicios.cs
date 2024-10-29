using Compartido.DTOs.Pregunta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios.Preguntas
{
    public interface IPreguntaServicios
    {
        Task<IEnumerable<PreguntaDTO>> SelectPorUsuarioId(string id);
        Task Insert(PreguntaInsertDTO dto);
    }
}
