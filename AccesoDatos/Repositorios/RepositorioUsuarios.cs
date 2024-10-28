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
    public class RepositorioUsuarios : IRepositorioUsuarios
    {
        private readonly PreguntameDBv2Context _context;
        public RepositorioUsuarios(PreguntameDBv2Context context)
        {
            _context = context;
        }
        public async Task<Usuario?> SelectPorNombreUsuario(string nombreUsuario)
            => await _context.Usuarios.Include(u => u.Pais).SingleOrDefaultAsync(u => u.NombreUsuario == nombreUsuario);

        public async Task<Usuario?> SelectPorEmail(string email)
            => await _context.Usuarios.Include(u => u.Pais).SingleOrDefaultAsync(u => u.Email == email);

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
    }
}
