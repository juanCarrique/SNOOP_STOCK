using System;
using System.Collections.Generic;

namespace DATA_STOCK.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string? Mail { get; set; }

    public string? Telefono { get; set; }

    public string Password { get; set; } = null!;

    public int RolId { get; set; }

    public bool Estado { get; set; }

    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();

    public virtual Rol Rol { get; set; } = null!;
}
