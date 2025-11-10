
using BeaconDesk.Application.Dto.AuthenticacionDto;
using BeaconDesk.Application.Interfaces.AuthenticacionInterfaces;
using BeaconDesk.Domain.AunthenticacionModule.Abstractions;
using System.Security.Authentication; // Para la excepción

namespace BeaconDesk.Application.Services.AutenticacionServices
{
    public class ImpUsuarioService:IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITokenServices _tokenService;


        public ImpUsuarioService(IUsuarioRepository usuarioRepository, ITokenServices tokenService)
        {
            _usuarioRepository = usuarioRepository;
            _tokenService = tokenService;
        }


        public async Task<string> LoginAsync(LoginRequestDto loginrequest)
        {
            try
            {
                //Traemos al usuario por email
                var usuario = await _usuarioRepository.GetByEmailAsync(loginrequest.Email!);

                //Realizamos validaciones
                if (usuario == null)
                {
                    // Mejora de seguridad: Mensaje genérico
                    throw new AuthenticationException("Usuario no existe.");
                }

               
                bool esPasswordValido = BCrypt.Net.BCrypt.Verify(loginrequest.Password, usuario.PasswordHash);

                if (!esPasswordValido)
                {
                    // Mensaje genérico para no dar pistas
                    throw new AuthenticationException("Contraseña incorrecta.");
                }
               


                if (!usuario.EstaActivo)
                {
                    throw new AuthenticationException("El usuario no está activo.");
                }



                //Generamos el token de authenticacion
                var token = _tokenService.GenerateToken(usuario); // Sigue siendo un token de prueba
                return token;
            }
            catch (AuthenticationException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                // Aquí deberías loggear el error 'ex'
                throw new ApplicationException("Error durante el proceso de login.", ex);
            }
        }


    }
}
