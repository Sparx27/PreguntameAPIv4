using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Excepciones
{
    public class RespuestaException : Exception
    {
        public RespuestaException() { }
        public RespuestaException(string message) : base(message) { }
        public RespuestaException(string message,  Exception innerException) : base(message, innerException) { }
    }
}
