using BeaconDesk.Application.Dto.AuthenticacionDto;
using BeaconDesk.Application.Interfaces.AuthenticacionInterfaces;
using BeaconDesk.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace BeaconDesk.Api2.Controllers
{
    /// <summary>
    /// Controlador encargado de la autenticación y seguridad de los usuarios.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")] // Indica que este controlador siempre habla JSON
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public AuthController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        /// <summary>
        /// Inicia sesión en el sistema validando credenciales.
        /// </summary>
        /// <remarks>
        /// Devuelve un Token JWT si las credenciales son correctas.
        /// Si hay errores de validación (ej. email vacío), retorna 400.
        /// Si la contraseña es incorrecta, retorna 401.
        /// </remarks>
        /// <param name="loginRequest">DTO con el email y contraseña del usuario.</param>
        /// <returns>Un objeto ApiResponse que contiene el Token de acceso.</returns>
        /// <response code="200">Login exitoso. Devuelve el Token.</response>
        /// <response code="400">Datos de entrada inválidos (Formato email, pass corto).</response>
        /// <response code="401">Credenciales inválidas (Usuario no existe o pass incorrecto).</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse<LoginResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Login([FromBody] LoginRequestDto loginRequest)
        {
            var data = await _usuarioService.LoginAsync(loginRequest);

            var response = new ApiResponse<LoginResponseDto>(data, "Login Exitoso");

            // NOTA: Como ya implementamos el CorrelationIdResultFilter en Program.cs,
            // NO necesitas asignar esto manualmente aquí. El filtro lo hará al salir.
            // response.CorrelationId = HttpContext.TraceIdentifier; 

            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Genera un hash BCrypt para una contraseña (UTILIDAD PARA DESARROLLO).
        /// </summary>
        /// <param name="password">La contraseña en texto plano a encriptar.</param>
        /// <returns>El hash generado.</returns>
        /// <response code="200">Hash generado correctamente.</response>
        [HttpGet("generate-hash/{password}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        public IActionResult GenerateHash(string password)
        {
            // Usamos la misma librería que el servicio de login
            var hash = BCrypt.Net.BCrypt.HashPassword(password);

            var data = new { hash = hash };

            var response = new ApiResponse<object>(data, "Hash Correcto");

            // Igual aquí, el filtro se encarga del ID.
            // response.CorrelationId = HttpContext.TraceIdentifier;

            return StatusCode(response.StatusCode, response);
        }
    }
}