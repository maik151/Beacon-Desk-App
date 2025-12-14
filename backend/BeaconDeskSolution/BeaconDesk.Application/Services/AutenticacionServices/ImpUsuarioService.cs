
using BeaconDesk.Application.Dto.AuthenticacionDto;
using BeaconDesk.Application.Interfaces.AuthenticacionInterfaces;
using BeaconDesk.Domain.AunthenticacionModule.Abstractions;
using System.Security.Authentication;
using Microsoft.Extensions.Logging;

namespace BeaconDesk.Application.Services.AutenticacionServices
{
    public class ImpUsuarioService:IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITokenServices _tokenService;
        private readonly ILogger<ImpUsuarioService> _logger;


        public ImpUsuarioService(IUsuarioRepository usuarioRepository, ITokenServices tokenService, ILogger<ImpUsuarioService> logger)
        {
            _usuarioRepository = usuarioRepository;
            _tokenService = tokenService;
            _logger = logger;
        }


        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto loginrequest)
        {
            _logger.LogInformation("Iniciando intento de login para el correo: {Email}", loginrequest.Email);

            try
            {
                var usuario = await _usuarioRepository.GetByEmailAsync(loginrequest.Email!);

                if (usuario == null)
                {
                    _logger.LogWarning("Login fallido. El usuario {Email} no existe en la BD.", loginrequest.Email);
                    throw new AuthenticationException("Credenciales inválidas.");
                }

                bool esPasswordValido = BCrypt.Net.BCrypt.Verify(loginrequest.Password, usuario.PasswordHash);

                if (!esPasswordValido)
                {
                    _logger.LogWarning("Login fallido. Contraseña incorrecta para el usuario {UserId} ({Email}).", usuario.UsuarioID, usuario.Email);
                    throw new AuthenticationException("Credenciales inválidas.");
                }

                if (!usuario.EstaActivo)
                {
                    _logger.LogWarning("Login rechazado. El usuario {UserId} está inactivo.", usuario.UsuarioID);
                    throw new AuthenticationException("El usuario no está activo.");
                }

                var token = _tokenService.GenerateToken(usuario);
                var data = new LoginResponseDto { Token = token};
                _logger.LogInformation("Login exitoso. Token generado para el usuario {UserId}.", usuario.UsuarioID);

                return data;
            }
            catch (AuthenticationException)
            {
                
                throw;
            }
            catch (Exception ex)
            {
               
                _logger.LogError(ex, "Error crítico no controlado durante el login de {Email}", loginrequest.Email);

                throw new ApplicationException("Ocurrió un error inesperado en el servidor.", ex);
            }
        }


    }
}
