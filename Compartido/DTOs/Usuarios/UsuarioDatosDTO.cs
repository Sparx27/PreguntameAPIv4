using Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compartido.DTOs.Usuario
{
    public class UsuarioDatosDTO
    {
        public string Email { get; set; }
        public string NombreUsuario { get; set; }
        public string Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Foto { get; set; }
        public string? Bio { get; set; }
        public string? CajaPreguntas { get; set; }
        public string NombrePais { get; set; }
        public string PaisId { get; set; }
    }
}
