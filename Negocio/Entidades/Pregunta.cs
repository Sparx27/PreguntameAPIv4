using System;
using System.Collections.Generic;

namespace Negocio.Entidades;

public partial class Pregunta
{
    public Guid PreguntaId { get; set; }

    public Guid UsuarioRecibe { get; set; }

    public Guid? UsuarioEnvia { get; set; }

    public string Dsc { get; set; } = null!;

    public DateTime Fecha { get; set; }

    public bool Estado { get; set; }

    public virtual Respuesta? Respuesta { get; set; }

    public virtual Usuario? UsuarioEnviaNavigation { get; set; }

    public virtual Usuario UsuarioRecibeNavigation { get; set; } = null!;
}
