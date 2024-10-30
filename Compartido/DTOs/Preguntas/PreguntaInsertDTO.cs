using Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compartido.DTOs.Preguntas
{
    public class PreguntaInsertDTO
    {
        public string UsuarioRecibe { get; set; }
        public string? UsuarioEnvia { get; set; }
        public string Dsc { get; set; }
    }
}
