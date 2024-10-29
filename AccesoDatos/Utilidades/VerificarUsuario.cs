using Negocio.Entidades;
using Negocio.Excepciones;
using Negocio.IRepositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.Utilidades
{
    public class VerificarUsuario
    {
        private readonly IUsuarioRepositorio _usuarioRepo;
        public VerificarUsuario(IUsuarioRepositorio usuarioRepo)
        {
            _usuarioRepo = usuarioRepo;
        }

        public async Task Existe(Guid usuarioId)
        {
            Usuario existe = await _usuarioRepo.SelectPorId(usuarioId)
                ?? throw new UsuarioException("No existe usuario con ese id");
        }
    }
}
