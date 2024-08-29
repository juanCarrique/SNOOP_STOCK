using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Services
{
    public class ComponenteDTO
    {
        [JsonIgnore]
        public int Id { get; set; }
        
        public int ComponenteId { get; set; }

        public int Cantidad { get; set; }
    }
}
