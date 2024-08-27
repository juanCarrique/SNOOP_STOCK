using Domain.Models; // TODO preguntar a nico si esta bien esto de acceder a los modelos desde esta capa
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DataAccess.Context; // Asegúrate de que este espacio de nombres es correcto

namespace AppStock.Controllers
{
    [ApiController]
    [Route("usuario")]
    public class UsuarioController : ControllerBase
    {
        private readonly StockappContext _context;
        private readonly IConfiguration _configuration;

        public UsuarioController(StockappContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult IniciarSesion([FromBody] Object optData)
        {
            if (optData == null)
            {
                return BadRequest(new { success = false, message = "Datos de entrada no válidos" });
            }

            var data = JsonConvert.DeserializeObject<dynamic>(optData.ToString());
            if (data == null || data.Mail == null || data.Password == null)
            {
                return BadRequest(new { success = false, message = "Datos de entrada no válidos" });
            }

            string mail = data.Mail.ToString();
            string password = data.Password.ToString();

            Usuario? usuario = _context.Usuarios
                .Where(x => x.Mail == mail && x.Password == password)
                .FirstOrDefault();

            if (usuario == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Usuario no encontrado",
                    result = ""
                });
            }

            var jwt = _configuration.GetSection("Jwt").Get<Jwt>();
            if (jwt == null || jwt.Key == null || jwt.Subject == null || jwt.Issuer == null || jwt.Audience == null)
            {
                return StatusCode(500, new { success = false, message = "Configuración JWT no válida" });
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("Id", usuario.Id.ToString()),
                new Claim("Mail", usuario.Mail ?? string.Empty),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                jwt.Issuer,
                jwt.Audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: signIn
            );
            return Ok(new
            {
                success = true,
                message = "Usuario encontrado",
                result = new JwtSecurityTokenHandler().WriteToken(token),
            });
        }
    }

    /* esto hay que mandar en la petición POST
        {
            "Mail": "carlos@mail.com",
            "Password": "password1"
        }
    */

    /* TODO esto puede ser un archivo aparte en Domain.Models, preguntar a nico que conviene*/
    public class Jwt
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Subject { get; set; }
    }
    
}