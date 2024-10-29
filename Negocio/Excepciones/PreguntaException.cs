using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Excepciones
{
    public class PreguntaException : Exception
    {
        public PreguntaException() { }
        public PreguntaException(string message) : base(message) { }
        public PreguntaException(string message, Exception innerException) : base(message, innerException) { }
    }
}
