using Compartido.DTOs.Usuario;
using Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios.Usuarios
{
    public interface IUsuarioServicios
    {
        Task<(UsuarioDatosDTO, string)> IniciarSesion(string email, string contrasena);
        Task Insert(UsuarioInsertDTO dto);
        Task UpdateDatosUsuario(string nombreUsuario, UsuarioDatosDTO dto);
        Task UpdateContrasena(string nombreUsuario, string nuevaContrasena);
        Task ToggleInsertSeguimiento(string usuarioId, string usuarioASeguirId);
    }
}
