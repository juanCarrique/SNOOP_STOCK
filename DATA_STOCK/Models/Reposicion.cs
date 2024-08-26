using System;
using System.Collections.Generic;

namespace DATA_STOCK.Models;

public partial class Reposicion
{
    public int Id { get; set; }

    public int ProductoId { get; set; }

    public int ProveedorId { get; set; }

    public int Cantidad { get; set; }

    public virtual Producto Producto { get; set; } = null!;

    public virtual Proveedor Proveedor { get; set; } = null!;
}
