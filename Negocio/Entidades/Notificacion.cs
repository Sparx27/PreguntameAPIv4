using System;
using System.Collections.Generic;

namespace Negocio.Entidades;

public partial class Notificacion
{
    public Guid NotificacionId { get; set; }

    public Guid UsuarioId { get; set; }

    public DateTime Fecha { get; set; }

    public bool Estado { get; set; }

    public DateTime? FechaVista { get; set; }

    public string Tipo { get; set; } = null!;

    public Guid? SUsuarioSeguido { get; set; }

    public Guid? SUsuarioSeguidor { get; set; }

    public Guid? MRespuestaId { get; set; }

    public Guid? MUsuarioId { get; set; }

    public virtual MeGusta? MeGusta { get; set; }

    public virtual Seguimiento? Seguimiento { get; set; }

    public virtual Usuario Usuario { get; set; } = null!;
}
