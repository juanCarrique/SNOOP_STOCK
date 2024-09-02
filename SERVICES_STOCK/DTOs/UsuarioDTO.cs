using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs
{
    public class UsuarioDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;

        public string Apellido { get; set; } = null!;

        public string? Mail { get; set; }

        public string? Telefono { get; set; }

        public string Password { get; set; } = null!;

        public int RolId { get; set; }

        public bool Estado { get; set; }
    } 
}
