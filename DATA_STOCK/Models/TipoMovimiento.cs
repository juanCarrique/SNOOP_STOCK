using System;
using System.Collections.Generic;

namespace DATA_STOCK.Models;

public partial class TipoMovimiento
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<HistorialMovimiento> HistorialMovimientos { get; set; } = new List<HistorialMovimiento>();
}
