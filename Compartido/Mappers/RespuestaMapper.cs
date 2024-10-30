using Compartido.DTOs.Preguntas;
using Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Negocio.Excepciones;
using Compartido.DTOs.Respuestas;

namespace Compartido.Mappers
{
    public static class RespuestaMapper
    {
        private static void ThrowError()
        {
            throw new RespuestaException("Los datos de la respuesta se encuentran vacíos");
        }
        public static Respuesta InsertToEntidad(RespuestaInsertDTO dto)
        {
            if (dto == null) ThrowError(); ;
            return new Respuesta
            {
                PreguntaId = new Guid(dto.PreguntaId),
                Dsc = dto.Dsc
            };
        }
        public static RespuestaDTO EntidadToDTO(Respuesta respuesta)
        {
            if (respuesta == null) ThrowError();
            return new RespuestaDTO
            {
                RespuestaId = respuesta.RespuestaId.ToString(),
                Dsc = respuesta.Dsc,
                Fecha = respuesta.Fecha,
                DscPregunta = respuesta.Pregunta.Dsc,
                UsuarioEnvia = respuesta.Pregunta.UsuarioEnvia is null ? null : respuesta.Pregunta.UsuarioEnviaNavigation.NombreUsuario,
                Nlikes = respuesta.Nlikes
            };
        }
        public static IEnumerable<RespuestaDTO> ListaEntidadToListaDTO(List<Respuesta> lista)
        {
            return lista.Select(r => new RespuestaDTO
            {
                RespuestaId = r.RespuestaId.ToString(),
                Dsc = r.Dsc,
                Fecha = r.Fecha,
                DscPregunta = r.Pregunta.Dsc,
                UsuarioEnvia = r.Pregunta.UsuarioEnvia is null ? null : r.Pregunta.UsuarioEnviaNavigation.NombreUsuario,
                Nlikes = r.Nlikes
            });
        }
    }
}
