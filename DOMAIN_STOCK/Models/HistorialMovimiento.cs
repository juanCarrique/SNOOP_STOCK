using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class HistorialMovimiento
{
    public int Id { get; set; }

    public int ProductoId { get; set; }

    public int TipoId { get; set; }

    public int Cantidad { get; set; }

    public DateOnly Fecha { get; set; }

    public int UsuarioId { get; set; }

    public string? Detalle { get; set; }

    public virtual Producto Producto { get; set; } = null!;

    public virtual TipoMovimiento Tipo { get; set; } = null!;
}
