using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ProductoCompuestoDTO
    {
        public int Id { get; set; }

        public string Detalle { get; set; } = null!;

        public decimal Precio { get; set; }

        public int CategoriaId { get; set; }

        public int? StockMinimo { get; set; }

        public ICollection<ComponenteDTO> Componentes { get; set; }
    }
}
