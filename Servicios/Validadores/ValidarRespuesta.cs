using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Negocio.Excepciones;

namespace Servicios.Validadores
{
    public static class ValidarRespuesta
    {
        public static void Dsc(string dsc)
        {
            if (string.IsNullOrEmpty(dsc) || dsc.Length > 300)
                throw new RespuestaException("La respuesta puede contener hasta 300 caracteres");
        }
    }
}
