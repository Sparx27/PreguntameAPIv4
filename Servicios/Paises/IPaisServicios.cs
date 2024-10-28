using Compartido.DTOs.Paises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios.Paises
{
    public interface IPaisServicios
    {
        Task<IEnumerable<PaisDTO>> SelectAll();
    }
}
