﻿using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Services.DTOs;
using Services.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AppStock.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly LoginService loginService;
        private IConfiguration config;

        public LoginController(LoginService loginService, IConfiguration config)
        {
            this.loginService = loginService;
            this.config = config;
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult> Login(UsuarioLoginDTO usuario)
        {
            var user = await loginService.GetUsuario(usuario);
            if (user == null)
            {
                return BadRequest(new { message = "Credenciales invalidas." });
            }
            var (jwtToken, expiration) = GenerateToken(user);
            return Ok(new { token = jwtToken, expiration = expiration }); //TODO aca iria expiration 
        }

        private (string, DateTime) GenerateToken(Usuario usuario)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, usuario.Nombre),
                //new Claim(ClaimTypes.Email, usuario.Mail),
                new Claim("Rol", usuario.RolId.ToString())
            };

            var jwtKey = config.GetSection("JWT:Key").Value;
            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new InvalidOperationException("JWT Key is not configured.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var expiration = DateTime.Now.AddMinutes(60);

            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: expiration,
                signingCredentials: creds
            );

            string token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return (token, expiration);
        }
    }
}