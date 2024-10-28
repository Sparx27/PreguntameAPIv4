using System;
using System.Collections.Generic;

namespace Negocio.Entidades;

public partial class Pais
{
    public string PaisId { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
