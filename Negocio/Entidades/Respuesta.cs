using System;
using System.Collections.Generic;

namespace Negocio.Entidades;

public partial class Respuesta
{
    public Guid RespuestaId { get; set; }

    public Guid PreguntaId { get; set; }

    public string Dsc { get; set; } = null!;

    public DateTime Fecha { get; set; }

    public int Nlikes { get; set; }

    public virtual ICollection<MeGusta> MeGusta { get; set; } = new List<MeGusta>();

    public virtual Pregunta Pregunta { get; set; } = null!;
}
