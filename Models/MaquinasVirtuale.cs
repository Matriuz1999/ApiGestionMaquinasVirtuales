using System;
using System.Collections.Generic;

namespace ApiGestionMaquinasVirtuales.Models;

public partial class MaquinasVirtuale
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int Cores { get; set; }

    public int Ram { get; set; }

    public int Disco { get; set; }

    public string Os { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public DateTime FechaCreacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }
}
