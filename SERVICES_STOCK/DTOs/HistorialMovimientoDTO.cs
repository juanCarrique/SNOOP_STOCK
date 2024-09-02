using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs
{
    public class HistorialMovimientoDTO
    {
        public int Id { get; set; }

        public int ProductoId { get; set; }

        public int TipoId { get; set; }

        public int Cantidad { get; set; }

        public DateOnly Fecha { get; set; }

        public int UsuarioId { get; set; }

        public string? Detalle { get; set; }
    }
}
