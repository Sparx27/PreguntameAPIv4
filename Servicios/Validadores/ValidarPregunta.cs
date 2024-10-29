using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Negocio.Excepciones;

namespace Servicios.Validadores
{
    public static class ValidarPregunta
    {
        public static void Dsc(string dsc)
        {
            if(string.IsNullOrEmpty(dsc) || dsc.Length > 300)
            {
                throw new PreguntaException("La pregunta debe contener hasta 300 caracteres");
            }
        }
    }
}
