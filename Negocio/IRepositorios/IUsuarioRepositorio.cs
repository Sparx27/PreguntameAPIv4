using Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.IRepositorios
{
    public interface IUsuarioRepositorio
    {
        Task<Usuario?> SelectPorNombreUsuario(string nombreUsuario);
        Task<Usuario?> SelectPorEmail(string email);
        Task<Usuario?> SelectPorId(Guid usuarioId);
        Task Insert(Usuario u);
        Task UpdateUsuario(Usuario u);
        Task UpdateContrasena(Usuario u, string nuevaContrasena);
    }
}
