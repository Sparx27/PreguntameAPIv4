using System;
using System.Collections.Generic;

namespace Negocio.Entidades;

public partial class Usuario
{
    public Guid UsuarioId { get; set; }

    public bool? Confirmado { get; set; }

    public bool? Activo { get; set; }

    public string Email { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public string NombreUsuario { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string? Apellido { get; set; }

    public string? NombreCompleto { get; set; }

    public string? Foto { get; set; }

    public string? Bio { get; set; }

    public string? CajaPreguntas { get; set; }

    public int Nlikes { get; set; }

    public int Nseguidores { get; set; }

    public string PaisId { get; set; } = null!;

    public virtual ICollection<MeGusta> MeGusta { get; set; } = new List<MeGusta>();

    public virtual ICollection<Notificacion> Notificaciones { get; set; } = new List<Notificacion>();

    public virtual Pais Pais { get; set; } = null!;

    public virtual ICollection<Pregunta> PreguntaUsuarioEnviaNavigations { get; set; } = new List<Pregunta>();

    public virtual ICollection<Pregunta> PreguntaUsuarioRecibeNavigations { get; set; } = new List<Pregunta>();

    public virtual ICollection<Seguimiento> SeguimientoUsuarioSeguidoNavigations { get; set; } = new List<Seguimiento>();

    public virtual ICollection<Seguimiento> SeguimientoUsuarioSeguidorNavigations { get; set; } = new List<Seguimiento>();
}
