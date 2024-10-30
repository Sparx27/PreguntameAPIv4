using Compartido.DTOs.Preguntas;
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
        private static void ThrowError()
        {
            throw new PreguntaException("Los datos de la pregunta se encuentran vacíos");
        }
        public static Pregunta InsertToEntidad(PreguntaInsertDTO dto)
        {
            if (dto == null) ThrowError();
            return new Pregunta
            {
                Dsc = dto.Dsc,
                UsuarioEnvia = dto.UsuarioEnvia is null ? null : new Guid(dto.UsuarioEnvia),
                UsuarioRecibe = new Guid(dto.UsuarioRecibe)
            };
        }

        public static PreguntaDTO EntidadToDTO(Pregunta pregunta)
        {
            if(pregunta == null) ThrowError();
            return new PreguntaDTO
            {
                PreguntaId = pregunta.PreguntaId.ToString(),
                UsuarioEnvia = pregunta.UsuarioEnvia is null ? null : pregunta.UsuarioEnviaNavigation.NombreUsuario,
                UsuarioRecibe = pregunta.UsuarioRecibe.ToString(),
                Dsc = pregunta.Dsc,
                Fecha = pregunta.Fecha
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
