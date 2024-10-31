using Negocio.Entidades;
using Negocio.IRepositorios;
using Compartido.DTOs.Usuario;
using Compartido.Mappers;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Servicios.Utilidades;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Servicios.Validadores;
using Negocio.Excepciones;
using Compartido.DTOs.Paises;
using Servicios.Paises;
using BCrypt.Net;

namespace Servicios.Usuarios
{
    public class UsuarioServicios : IUsuarioServicios
    {
        private readonly IUsuarioRepositorio _repoUsuarios;
        private readonly IConfiguration _config;
        private readonly IPaisServicios _paisServicios;
        public UsuarioServicios(IUsuarioRepositorio repoUsuarios, IConfiguration config, IPaisServicios paisServicios)
        {
            _repoUsuarios = repoUsuarios;
            _config = config;
            _paisServicios = paisServicios;
        }

        public async Task Insert(UsuarioInsertDTO dto)
        {
            ObjectStringsTrimmer<UsuarioInsertDTO>.Ejecutar(dto);
            ValidarUsuario.Email(dto.Email);
            ValidarUsuario.NombreUsuario(dto.NombreUsuario);
            ValidarUsuario.Contrasena(dto.Contrasena);
            dto.Contrasena = BCrypt.Net.BCrypt.HashPassword(dto.Contrasena);
            ValidarUsuario.Nombre(dto.Nombre);
            IEnumerable<PaisDTO> liPaises = await _paisServicios.SelectAll();
            if (!liPaises.Any(p => p.PaisId == dto.PaisId)) throw new UsuarioException("Ingrese un país correcto");

            await _repoUsuarios.Insert(UsuarioMapper.InsertToEntidad(dto));
        }

        public async Task UpdateDatosUsuario(string nombreUsuario, UsuarioDatosDTO dto)
        {
            Usuario? u = await _repoUsuarios.SelectPorNombreUsuario(nombreUsuario)
                ?? throw new UsuarioException("No existe usuario con ese nombre de usuario");
            ObjectStringsTrimmer<UsuarioDatosDTO>.Ejecutar(dto);

            ValidarUsuario.Nombre(dto.Nombre);
            ValidarUsuario.Apellido(dto.Apellido);
            ValidarUsuario.Bio(dto.Bio);
            ValidarUsuario.CajaPreguntas(dto.CajaPreguntas);

            IEnumerable<PaisDTO> liPaises = await _paisServicios.SelectAll();
            PaisDTO? existePais = liPaises.FirstOrDefault(p => p.PaisId == dto.PaisId)
                ?? throw new UsuarioException("Ingrese un país correcto");

            u.Nombre = dto.Nombre;
            u.Apellido = dto.Apellido;
            u.Bio = dto.Bio;
            u.CajaPreguntas = dto.CajaPreguntas;
            u.PaisId = dto.PaisId;

            await _repoUsuarios.UpdateUsuario(u);
            dto.NombrePais = existePais.Nombre;
        }

        public async Task UpdateContrasena(string nombreUsuario, string nuevaContrasena)
        {
            Usuario? u = await _repoUsuarios.SelectPorNombreUsuario(nombreUsuario)
                ?? throw new UsuarioException("No existe usuario con ese nombre de usuario");
            nuevaContrasena = nuevaContrasena.Trim();
            ValidarUsuario.Contrasena(nuevaContrasena);

            nuevaContrasena = BCrypt.Net.BCrypt.HashPassword(nuevaContrasena);

            await _repoUsuarios.UpdateContrasena(u, nuevaContrasena);
        }

        public async Task<(UsuarioDatosDTO, string)> IniciarSesion(string email, string contrasena)
        {
            email?.Trim();
            contrasena?.Trim();

            Usuario? u = await _repoUsuarios.SelectPorEmail(email) 
                ?? throw new UsuarioException("Credenciales incorrectas");

            if (!BCrypt.Net.BCrypt.Verify(contrasena, u.Contrasena)) 
                throw new UsuarioException("Credenciales incorrectas");

            ValidarUsuario.Confirmado(u);
            ValidarUsuario.Activo(u);

            UsuarioDatosDTO usuarioInfo = UsuarioMapper.EntidadToDatosDTO(u);
            string token = GenerarToken(u.NombreUsuario, u.UsuarioId.ToString());
            return (usuarioInfo, token);
        }
        private string GenerarToken(string nombreUsuario, string usuarioId)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("NombreUsuario", nombreUsuario),
                new Claim("UsuarioId", usuarioId)
            };

            string? secret = _config["JWTConfig:Secret"]
                ?? throw new ExcepcionInesperadaControlada("Error en la configuracion de JWT");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            return new JwtSecurityTokenHandler()
                .WriteToken(new JwtSecurityToken(
                    claims: claims, 
                    expires: DateTime.UtcNow.AddHours(1), 
                    signingCredentials: creds
                ));
        }

        public async Task ToggleInsertSeguimiento(string usuarioId, string usuarioASeguirId) => 
            await _repoUsuarios.ToggleInsertSeguimiento(new Guid(usuarioId), new Guid(usuarioASeguirId));
    }
}
