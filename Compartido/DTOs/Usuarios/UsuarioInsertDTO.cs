using Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compartido.DTOs.Usuario
{
    public class UsuarioInsertDTO
    {
        public string Email { get; set; }
        public string Contrasena { get; set; }
        public string NombreUsuario { get; set; }
        public string Nombre { get; set; }
        public string? Apellido { get; set; }
        public string PaisId { get; set; }
    }
}
