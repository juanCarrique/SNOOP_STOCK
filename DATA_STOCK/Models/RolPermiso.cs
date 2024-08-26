using System;
using System.Collections.Generic;

namespace DATA_STOCK.Models;

public partial class RolPermiso
{
    public int Id { get; set; }

    public int RolId { get; set; }

    public int PermisoId { get; set; }

    public virtual Permiso Permiso { get; set; } = null!;

    public virtual Rol Rol { get; set; } = null!;
}
