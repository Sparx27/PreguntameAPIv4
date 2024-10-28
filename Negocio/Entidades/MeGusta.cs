using System;
using System.Collections.Generic;

namespace Negocio.Entidades;

public partial class MeGusta
{
    public Guid RespuestaId { get; set; }

    public Guid UsuarioId { get; set; }

    public DateTime Fecha { get; set; }

    public virtual ICollection<Notificacion> Notificaciones { get; set; } = new List<Notificacion>();

    public virtual Respuesta Respuesta { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
