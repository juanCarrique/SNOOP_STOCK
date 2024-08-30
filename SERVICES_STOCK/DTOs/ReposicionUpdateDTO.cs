using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ReposicionUpdateDTO
    {
        public int? ProductoId { get; set; }

        public int? ProveedorId { get; set; }

        public int? Cantidad { get; set; }
    }
}
