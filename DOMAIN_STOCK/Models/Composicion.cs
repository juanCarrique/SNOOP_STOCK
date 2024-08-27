using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class Composicion
{
    public int Id { get; set; }

    public int ProductoId { get; set; }

    public int ComponenteId { get; set; }

    public int Cantidad { get; set; }

    public virtual Producto Componente { get; set; } = null!;

    public virtual Producto Producto { get; set; } = null!;
}
