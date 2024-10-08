﻿using System;
using System.Collections.Generic;

namespace DOMAIN_STOCK.Models;

public partial class Proveedor
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string? Mail { get; set; }

    public string? Telefono { get; set; }

    public virtual ICollection<Reposicion> Reposicions { get; set; } = new List<Reposicion>();
}
