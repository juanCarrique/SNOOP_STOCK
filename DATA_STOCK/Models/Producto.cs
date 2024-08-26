using System;
using System.Collections.Generic;

namespace DATA_STOCK.Models;

public partial class Producto
{
    public int Id { get; set; }

    public string Detalle { get; set; } = null!;

    public decimal Precio { get; set; }

    public int CategoriaId { get; set; }

    public int Stock { get; set; }

    public int? StockMinimo { get; set; }

    public virtual Categoria Categoria { get; set; } = null!;

    public virtual ICollection<Composicion> ComposicionComponentes { get; set; } = new List<Composicion>();

    public virtual ICollection<Composicion> ComposicionProductos { get; set; } = new List<Composicion>();

    public virtual ICollection<HistorialMovimiento> HistorialMovimientos { get; set; } = new List<HistorialMovimiento>();

    public virtual ICollection<ItemFactura> ItemFacturas { get; set; } = new List<ItemFactura>();

    public virtual ICollection<Reposicion> Reposicions { get; set; } = new List<Reposicion>();
}
