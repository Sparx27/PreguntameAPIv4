using Negocio.Entidades;
using Negocio.IRepositorios;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Negocio.Excepciones;

namespace AccesoDatos.Repositorios
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly PreguntameDBv2Context _context;
        public UsuarioRepositorio(PreguntameDBv2Context context)
        {
            _context = context;
        }
        public async Task<Usuario?> SelectPorNombreUsuario(string nombreUsuario)
            => await _context.Usuarios.Include(u => u.Pais).SingleOrDefaultAsync(u => u.NombreUsuario == nombreUsuario);

        public async Task<Usuario?> SelectPorEmail(string email)
            => await _context.Usuarios.Include(u => u.Pais).SingleOrDefaultAsync(u => u.Email == email);

        public async Task<Usuario?> SelectPorId(Guid usuarioId)
            => await _context.Usuarios.SingleOrDefaultAsync(u => u.UsuarioId == usuarioId);

        public async Task Insert(Usuario u)
        {
            Usuario? buscarNombreUsuario = await SelectPorNombreUsuario(u.NombreUsuario);
            if (buscarNombreUsuario != null)
                throw new ConflictException("El nombre de usuario ya fue registrado");
            Usuario? buscarEmail = await SelectPorEmail(u.Email);
            if (buscarEmail != null)
                throw new ConflictException("El email ya fue registrado");

            // Mientras no se implemente confirmación por Email
            u.Confirmado = true;
            u.Activo = true;
            
            await _context.Usuarios.AddAsync(u);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateContrasena(Usuario u, string nuevaContrasena)
        {
            u.Contrasena = nuevaContrasena;
            _context.Usuarios.Update(u);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUsuario(Usuario u)
        {
            _context.Usuarios.Update(u);
            await _context.SaveChangesAsync();
        }

        public async Task ToggleInsertSeguimiento(Guid usuarioId, Guid usuarioASeguirId)
        {
            Usuario? seguidor = await _context.Usuarios.FirstOrDefaultAsync(u => u.UsuarioId == usuarioId)
                ?? throw new Exception("Error en el seguimiento: el usuario que intenta seguir a otro no fue encontrado por su id");
            Usuario? seguido = await _context.Usuarios.FirstOrDefaultAsync(u => u.UsuarioId == usuarioASeguirId)
                ?? throw new UsuarioException("El usuario que intenta seguir no existe");

            // Verifica que quien esta dando Seguimiento a otro usuario, si ya lo sigue y en base a eso:
            // Si no lo sigue, entonces lo quiere seguir y se crea el seguimiento.
            // Si ya lo sigue, entonces es un escenario en donde lo quiere dejar de seguir.
            Boolean yaLoSigue = await _context.Seguimientos.AnyAsync(s => s.UsuarioSeguidor == usuarioId && s.UsuarioSeguido == usuarioASeguirId);

            // Acá utilizo una transacción para mantener la atomicidad
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if(yaLoSigue)
                {
                    await _context.Seguimientos.AddAsync(new Seguimiento
                    {
                        UsuarioSeguidor = usuarioId,
                        UsuarioSeguido = usuarioASeguirId
                    });

                    seguido.Nseguidores++;
                    _context.Usuarios.Update(seguido);

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                else
                {
                    Seguimiento? s = await _context.Seguimientos
                        .FirstOrDefaultAsync(s => s.UsuarioSeguidor == usuarioId && s.UsuarioSeguido == usuarioASeguirId)
                            ?? throw new Exception("Error, se intentó buscar un seguimiento que supuestamente existía pero no lo encontró");

                    _context.Seguimientos.Remove(s);

                    seguido.Nseguidores--;
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
            }
            catch(Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
