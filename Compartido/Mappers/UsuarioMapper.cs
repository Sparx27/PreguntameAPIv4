using Negocio.Entidades;
using Compartido.DTOs.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Negocio.Excepciones;

namespace Compartido.Mappers
{
    public static class UsuarioMapper
    {
        public static UsuarioDatosDTO EntidadToDatosDTO(Usuario usuario)
        {
            if (usuario == null) throw new UsuarioException("Datos de Usuario vacíos");
            return new UsuarioDatosDTO
            {
                Email = usuario.Email,
                NombreUsuario = usuario.NombreUsuario,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Foto = usuario.Foto,
                Bio = usuario.Bio,
                CajaPreguntas = usuario.CajaPreguntas,
                PaisId = usuario.PaisId,
                NombrePais = usuario.Pais.Nombre
            };
        }

        public static Usuario InsertToEntidad(UsuarioInsertDTO dto)
        {
            if (dto == null) throw new UsuarioException("Datos de Usuario vacíos");
            return new Usuario
            {
                Email = dto.Email,
                Contrasena = dto.Contrasena,
                NombreUsuario = dto.NombreUsuario,
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                PaisId = dto.PaisId
            };
        }
    }
}
