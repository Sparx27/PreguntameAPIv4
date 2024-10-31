using Compartido.DTOs.Respuestas;
using Compartido.Mappers;
using Negocio.Entidades;
using Negocio.IRepositorios;
using Servicios.Validadores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios.Respuestas
{
    public class RespuestaServicios : IRespuestaServicios
    {
        private readonly IRespuestaRepositorio _respuestaRepo;
        public RespuestaServicios(IRespuestaRepositorio respuestaRepo)
        {
            _respuestaRepo = respuestaRepo;
        }

        public async Task<RespuestaDTO?> SelectPorId(string respuestaId)
        {
            Respuesta? res = await _respuestaRepo.SelectPorId(new Guid(respuestaId));
            return res is null ? null : RespuestaMapper.EntidadToDTO(res);
        }

        public async Task<IEnumerable<RespuestaDTO>> SelectPorUsuarioId(string usuarioId)
        {
            List<Respuesta> res = await _respuestaRepo.SelectPorUsuarioId(new Guid(usuarioId));
            return RespuestaMapper.ListaEntidadToListaDTO(res);
        }

        public async Task Insert(RespuestaInsertDTO dto, string usuarioId)
        {
            dto.Dsc = dto.Dsc.Trim();
            ValidarRespuesta.Dsc(dto.Dsc);
            await _respuestaRepo.Insert(RespuestaMapper.InsertToEntidad(dto), new Guid(usuarioId));
        }

        public async Task Delete(string respuestaId, string usuarioId) =>
            await _respuestaRepo.Delete(new Guid(respuestaId), new Guid(usuarioId));

        public async Task ToggleMeGusta(string respuestaId, string usuarioId) =>
            await _respuestaRepo.ToggleInsertMeGusta(new Guid(respuestaId), new Guid(usuarioId));
    }
}
