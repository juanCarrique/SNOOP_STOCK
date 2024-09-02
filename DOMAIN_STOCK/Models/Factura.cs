using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class Factura
{
    public int Id { get; set; }

    public DateTime Fecha { get; set; }

    public decimal Total { get; set; }

    public int Numero { get; set; }

    public string Tipo { get; set; } = null!;

    public int? ClienteId { get; set; }

    public int VendedorId { get; set; }

    public virtual Cliente? Cliente { get; set; }

    public virtual ICollection<ItemFactura> ItemsFactura { get; set; } = new List<ItemFactura>();

    public virtual Usuario Vendedor { get; set; } = null!;
}
