using Negocio.Entidades;
using Negocio.IRepositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Negocio.Excepciones;
using Microsoft.EntityFrameworkCore;

namespace AccesoDatos.Repositorios
{
    public class PreguntaRepositorio : IPreguntaRepositorio
    {
        private readonly PreguntameDBv2Context _context;
        private readonly IUsuarioRepositorio _usuarioRepo;
        public PreguntaRepositorio(PreguntameDBv2Context context, IUsuarioRepositorio usuarioRepo)
        {
            _context = context;
            _usuarioRepo = usuarioRepo;
        }

        public async Task<List<Pregunta>> SelectPorUsuarioId(Guid usuarioId)
        {
            Usuario recibe = await _usuarioRepo.SelectPorId(usuarioId)
                ?? throw new PreguntaException("El usuario no existe o es incorrecto");
            return await _context.Preguntas.Where(p => p.UsuarioRecibe == usuarioId && p.Estado == false).ToListAsync();
        }

        public async Task Insert(Pregunta p)
        {
            Usuario recibe = await _usuarioRepo.SelectPorId(p.UsuarioRecibe)
                ?? throw new PreguntaException("El usuario a quien envía la pregunta no existe");
            if(p.UsuarioEnvia != null) {
                Usuario envia = await _usuarioRepo.SelectPorId(p.UsuarioEnvia.Value)
                    ?? throw new PreguntaException("El usuario  que intenta envíar la pregunta no existe");
            }

            await _context.Preguntas.AddAsync(p);
            await _context.SaveChangesAsync();
        }

        public Task Delete(Pregunta p)
        {
            throw new NotImplementedException();
        }


    }
}
