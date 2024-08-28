using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ReposicionDTO
    {
        public int Id { get; set; }

        public int ProductoId { get; set; }

        public int ProveedorId { get; set; }

        public int Cantidad { get; set; }
    }
}
