using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ClienteUpdateDTO
    {

        public string? Nombre { get; set; } = null!;

        public string? Apellido { get; set; } = null!;

        public string? Mail { get; set; }

        public string? Telefono { get; set; }
    }
}
