using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class FacturaUpdateDTO
    {

        public DateTime? Fecha { get; set; }

        public int? Numero { get; set; }

        public string? Tipo { get; set; } = null!;

        public int? ClienteId { get; set; }

        public int? VendedorId { get; set; }

        public ICollection<ItemFacturaDTO>? ItemsFactura { get; set; }

    }
}
