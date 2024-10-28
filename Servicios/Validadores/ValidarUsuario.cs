using Negocio.Entidades;
using Negocio.Excepciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Servicios.Validadores
{
    public static class ValidarUsuario
    {
        public static void Activo(Usuario u)
        {
            if (u.Activo == false) throw new UsuarioException("El usuario no se encuentra activo");
        }
        public static void Confirmado(Usuario u)
        {
            if (u.Confirmado == false) throw new UsuarioException("Debe confirmar su usuario a través de su email");
        }
        public static void Email(string email)
        {
            if (String.IsNullOrEmpty(email) || email.Length > 100)
                throw new UsuarioException("Email es requerido y puede contener hasta 100 caracteres");
            if (!Regex.IsMatch(email, @"^[\w\.-]+@[a-zA-Z\d\.-]+\.[a-zA-Z]{2,}$"))
                throw new UsuarioException("Formato de email incorrecto"); 
        }
        public static void Contrasena(string contrasena)
        {
            if (String.IsNullOrEmpty(contrasena) || contrasena.Length > 70 || contrasena.Length < 6)
                throw new UsuarioException("Contraña es requerida y debe tener al menos 6 caracteres");
            if (!Regex.IsMatch(contrasena, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$"))
                throw new UsuarioException("La contraseña debe contener al menos 1 minúscula, 1 mayúscula y 1 número");
        }

        public static void NombreUsuario(string nombreUsuario)
        {
            if (String.IsNullOrEmpty(nombreUsuario) || nombreUsuario.Length < 3)
                throw new UsuarioException("Nombre de usuario es requerido y debe contener al menos 3 caracteres");
        }

        public static void Nombre(string nombre)
        {
            if (String.IsNullOrEmpty(nombre)) throw new UsuarioException("Nombre es requerido");
            if (!String.IsNullOrEmpty(nombre) && nombre.Length > 20)
                throw new UsuarioException("El nombre puede contener hasta 20 caracteres");
        }

        public static void Apellido(string apellido)
        {
            if (!String.IsNullOrEmpty(apellido) && apellido.Length > 30)
                throw new UsuarioException("El apellido puede contener hasta 30 caracteres");
        }

        public static void Bio(string bio)
        {
            if (!String.IsNullOrEmpty(bio) && bio.Length > 250)
                throw new UsuarioException("La bio puede contener hasta 250 caracteres");
        }

        public static void CajaPreguntas(string cajaPreguntas)
        {
            if (!String.IsNullOrEmpty(cajaPreguntas) && cajaPreguntas.Length > 50)
                throw new UsuarioException("El texto en la caja de preguntas puede contener hasta 50 caracteres");
        }
    }
}
