using BeaconDesk.Application.Dto.AuthenticacionDto;
using BeaconDesk.Application.Interfaces.AuthenticacionInterfaces;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;
// **********************************************
using BeaconDesk.Application.Dto.Errors; // 🚨 AÑADIR ESTE USING
using System;
using BeaconDesk.Domain.Common; // Necesario para DateTimeOffset
// **********************************************

namespace BeaconDesk.Api2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public AuthController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<string>>> Login([FromBody] LoginRequestDto loginRequest)
        {
                var token = await _usuarioService.LoginAsync(loginRequest);
                var response = new ApiResponse<string>(token, "Login exitoso");
                response.CorrelationId = HttpContext.TraceIdentifier;
                return Ok(response);
        }

        [HttpGet("generate-hash/{password}")]
        public IActionResult GenerateHash(string password)
        {
            // Usamos la misma librería que el servicio de login
            var hash = BCrypt.Net.BCrypt.HashPassword(password);

            // Devolvemos el hash y un recordatorio
            return Ok(new
            {
                Password = password,
                Hash = hash,
                Message = "Copia este HASH y pégalo en la columna 'PasswordHash' de tu usuario admin en la BD."
            });
        }
    }
}