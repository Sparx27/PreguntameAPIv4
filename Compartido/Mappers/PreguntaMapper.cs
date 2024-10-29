using Compartido.DTOs.Pregunta;
using Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Negocio.Excepciones;

namespace Compartido.Mappers
{
    public static class PreguntaMapper
    {
        public static Pregunta InsertToEntidad(PreguntaInsertDTO dto)
        {
            if (dto == null) throw new PreguntaException("Los datos de la pregunta se encuentran vacíos");
            return new Pregunta
            {
                Dsc = dto.Dsc,
                UsuarioEnvia = dto.UsuarioEnvia is null ? null : new Guid(dto.UsuarioEnvia),
                UsuarioRecibe = new Guid(dto.UsuarioRecibe)
            };
        }

        public static IEnumerable<PreguntaDTO> ListaEntidadToListaDTO(List<Pregunta> lista) =>
            lista.Select(p => new PreguntaDTO
            {
                PreguntaId = p.PreguntaId.ToString(),
                UsuarioEnvia = p.UsuarioEnvia is null ? null : p.UsuarioEnvia.ToString(),
                UsuarioRecibe = p.UsuarioRecibe.ToString(),
                Dsc = p.Dsc,
                Fecha = p.Fecha
            });
    }
}
