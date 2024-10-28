using Compartido.DTOs.Paises;
using Microsoft.EntityFrameworkCore;
using Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios.Paises
{
    public class PaisServicios : IPaisServicios
    {
        private readonly PreguntameDBv2Context _context;
        public PaisServicios(PreguntameDBv2Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PaisDTO>> SelectAll() => await _context.Paises.Select(p => new PaisDTO
            {
                PaisId = p.PaisId.ToString(),
                Nombre = p.Nombre
            }).ToListAsync();
    }
}
