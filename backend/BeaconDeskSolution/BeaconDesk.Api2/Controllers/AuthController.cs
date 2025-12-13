using BeaconDesk.Application.Dto.AuthenticacionDto;
using BeaconDesk.Application.Interfaces.AuthenticacionInterfaces;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;
// **********************************************
using BeaconDesk.Application.Dto.Errors; // 🚨 AÑADIR ESTE USING
using System; // Necesario para DateTimeOffset
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
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
        {
            try
            {
                var token = await _usuarioService.LoginAsync(loginRequest);
                return Ok(new { AccesToken = token });

            }
            catch (AuthenticationException ex)
            {

                // =======================================================
                // 🚨 CÓDIGO MODIFICADO: Devolver el objeto ErrorDetails enriquecido
                // =======================================================
                var errorDetails = new ErrorDetails
                {
                    StatusCode = StatusCodes.Status401Unauthorized, // Usamos 401
                    Message = ex.Message,
                    Timestamp = DateTimeOffset.UtcNow,              // Hora actual del servidor
                    UserIdentifier = loginRequest.Email,            // Email del intento fallido
                    // ErrorId se genera automáticamente en el constructor de ErrorDetails
                };

                // Devolvemos la respuesta 401 con el cuerpo ErrorDetails
                return Unauthorized(errorDetails);
            }
            catch (Exception ex)
            {
                // Para errores 500 no relacionados con autenticación, 
                // devolvemos un 500 con el ErrorDetails estándar (manejo general)
                var errorDetails = new ErrorDetails
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Error interno del servidor. Consulte los logs con Error ID.",
                    Timestamp = DateTimeOffset.UtcNow,
                    // Aquí no tenemos acceso al email de forma segura
                };

                // NOTA: Tu ExceptionMiddleware capturaría esto si no tuviera try/catch.
                // Como tiene try/catch, usamos el 500 aquí para ser más explícitos.
                return StatusCode(StatusCodes.Status500InternalServerError, errorDetails);
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