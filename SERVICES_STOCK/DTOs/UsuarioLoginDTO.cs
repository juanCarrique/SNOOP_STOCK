using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs
{
    public class UsuarioLoginDTO
    {
        public String Mail { get; set; } = null!;
        public String Password { get; set; } = null!;
    }
}