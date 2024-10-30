using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compartido.DTOs.Respuestas
{
    public class RespuestaDTO
    {
        public string RespuestaId { get; set; }
        public string Dsc { get; set; }
        public DateTime Fecha { get; set; }
        public int Nlikes { get; set; }
        public string DscPregunta { get; set; }
        public string UsuarioEnvia {  get; set; }
    }
}
