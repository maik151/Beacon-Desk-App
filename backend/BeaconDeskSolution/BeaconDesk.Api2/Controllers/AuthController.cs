using BeaconDesk.Application.Dto.AuthenticacionDto;
using BeaconDesk.Application.Interfaces.AuthenticacionInterfaces;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;

namespace BeaconDesk.Api2.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController:ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public AuthController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest) {
            try
            {
                var token = await _usuarioService.LoginAsync(loginRequest);
                return Ok(new { AccesToken = token });

            }
            catch (AuthenticationException ex) { 
                return Unauthorized(new { Message = ex.Message });
            }
            catch (Exception ex) {
                return StatusCode(500, new { Message = $"Error interno del servidor.{ex}" });
            }
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
