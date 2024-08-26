using System;
using System.Collections.Generic;

namespace DATA_STOCK.Models;

public partial class Factura
{
    public int Id { get; set; }

    public DateOnly Fecha { get; set; }

    public decimal Total { get; set; }

    public int Numero { get; set; }

    public string Tipo { get; set; } = null!;

    public int? ClienteId { get; set; }

    public int VendedorId { get; set; }

    public virtual Cliente? Cliente { get; set; }

    public virtual ICollection<ItemFactura> ItemFacturas { get; set; } = new List<ItemFactura>();

    public virtual Usuario Vendedor { get; set; } = null!;
}
