using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Excepciones
{
    public class ExcepcionInesperadaControlada : Exception
    {
        public ExcepcionInesperadaControlada() : base() { }
        public ExcepcionInesperadaControlada(string message) : base(message) { }
        public ExcepcionInesperadaControlada(string message, Exception innerException) 
            : base(message, innerException) { }

    }
}
