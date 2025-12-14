using BeaconDesk.Application.Dto.AuthenticacionDto;
// **********************************************
using BeaconDesk.Application.Dto.Errors; // 🚨 AÑADIR ESTE USING
using BeaconDesk.Application.Interfaces.AuthenticacionInterfaces;
using BeaconDesk.Domain.Common; // Necesario para DateTimeOffset
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Authentication;
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
        public async Task<ActionResult> Login([FromBody] LoginRequestDto loginRequest)
        {
            var data = await _usuarioService.LoginAsync(loginRequest);

            var response = new ApiResponse<LoginResponseDto>(data, "Login Exitoso");
            response.CorrelationId = HttpContext.TraceIdentifier;

            return StatusCode(response.StatusCode, response);
        }



        [HttpGet("generate-hash/{password}")]
        public IActionResult GenerateHash(string password)
        {
            // Usamos la misma librería que el servicio de login
            var hash = BCrypt.Net.BCrypt.HashPassword(password);
            
            var data = new { hash = hash};

            var response = new ApiResponse<object>(data, "Hash Correcto");
            response.CorrelationId = HttpContext.TraceIdentifier;
            return StatusCode(response.StatusCode, response);

           
        }
    }
}