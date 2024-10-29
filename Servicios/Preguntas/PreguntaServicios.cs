using Compartido.DTOs.Pregunta;
using Negocio.IRepositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Negocio.Excepciones;
using Servicios.Validadores;
using Compartido.Mappers;
using Negocio.Entidades;

namespace Servicios.Preguntas
{
    public class PreguntaServicios : IPreguntaServicios
    {
        private readonly IPreguntaRepositorio _preguntaRepo;
        public PreguntaServicios(IPreguntaRepositorio preguntaRepo)
        {
            _preguntaRepo = preguntaRepo;
        }

        public async Task<IEnumerable<PreguntaDTO>> SelectPorUsuarioId(string id)
        {
            List<Pregunta> res = await _preguntaRepo.SelectPorUsuarioId(new Guid(id));
            return PreguntaMapper.ListaEntidadToListaDTO(res);
        }

        public async Task Insert(PreguntaInsertDTO dto)
        {
            if (String.IsNullOrEmpty(dto.UsuarioRecibe)) throw new PreguntaException("Falta el destinatario de la pregunta");
            ValidarPregunta.Dsc(dto.Dsc);
            await _preguntaRepo.Insert(PreguntaMapper.InsertToEntidad(dto));
        }
    }
}
