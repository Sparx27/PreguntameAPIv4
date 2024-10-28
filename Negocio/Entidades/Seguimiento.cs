using System;
using System.Collections.Generic;

namespace Negocio.Entidades;

public partial class Seguimiento
{
    public Guid UsuarioSeguido { get; set; }

    public Guid UsuarioSeguidor { get; set; }

    public DateTime Fecha { get; set; }

    public virtual ICollection<Notificacion> Notificaciones { get; set; } = new List<Notificacion>();

    public virtual Usuario UsuarioSeguidoNavigation { get; set; } = null!;

    public virtual Usuario UsuarioSeguidorNavigation { get; set; } = null!;
}
