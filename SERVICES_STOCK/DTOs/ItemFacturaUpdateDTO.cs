using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ItemFacturaUpdateDTO
    {
        public int? FacturaId { get; set; }

        public int? ProductoId { get; set; }

        public int? Cantidad { get; set; }

        public decimal? Precio { get; set; }
    }
}
