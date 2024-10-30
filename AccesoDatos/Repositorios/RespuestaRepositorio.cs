using Microsoft.EntityFrameworkCore;
using Negocio.Entidades;
using Negocio.IRepositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Negocio.Excepciones;

namespace AccesoDatos.Repositorios
{
    public class RespuestaRepositorio : IRespuestaRepositorio
    {
        private readonly PreguntameDBv2Context _context;
        private readonly IPreguntaRepositorio _preguntaRepo;
        public RespuestaRepositorio(PreguntameDBv2Context context, IPreguntaRepositorio preguntaRepo)
        {
            _context = context;
            _preguntaRepo = preguntaRepo;
        }

        public async Task<Respuesta?> SelectPorId(Guid respuestaId) =>
            await _context.Respuestas
                .Include(r => r.Pregunta)
                .ThenInclude(p => p.UsuarioEnviaNavigation)
                .FirstOrDefaultAsync(r => r.PreguntaId == respuestaId);

        public async Task<List<Respuesta>> SelectPorUsuarioId(Guid usuarioId) =>
            await _context.Respuestas
                .Include(r => r.Pregunta)
                .ThenInclude(p => p.UsuarioEnviaNavigation)
                .Where(r => r.Pregunta.UsuarioRecibe == usuarioId)
                .ToListAsync();

        public async Task Insert(Respuesta respuesta, Guid usuarioId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                Pregunta existe = await _preguntaRepo.SelectPorId(respuesta.PreguntaId)
                    ?? throw new RespuestaException("La pregunta que se intenta responder no existe");

                if (existe.UsuarioRecibe != usuarioId) 
                    throw new ForbiddenException("La pregunta que intenta responder, no le pertenece");

                await _context.AddAsync(respuesta);

                existe.Estado = true; // Marcar la Pregunta como respondida
                _context.Preguntas.Update(existe);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
            
        }

        // NOTA: En la base de datos, ya cree un trigger que elimina la pregunta luego de eliminar una respuesta
        // (No tiene sentido conservar una pregunta si la respuesta se borra)
        public async Task Delete(Guid respuestaId, Guid usuarioId)
        {
            Respuesta existe = await _context.Respuestas.Include(r => r.Pregunta).FirstOrDefaultAsync(r => r.RespuestaId == respuestaId)
                ?? throw new RespuestaException("La respuesta que se intenta eliminar no existe");

            if (existe.Pregunta.UsuarioRecibe != usuarioId) 
                throw new ForbiddenException("La respuesta que intenta eliminar, no le pertenece");

            _context.Respuestas.Remove(existe);
            await _context.SaveChangesAsync();
        }
    }
}
