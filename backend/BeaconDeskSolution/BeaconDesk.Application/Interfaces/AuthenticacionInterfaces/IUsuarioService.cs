using BeaconDesk.Application.Dto.AuthenticacionDto;

namespace BeaconDesk.Application.Interfaces.AuthenticacionInterfaces
{
    public interface IUsuarioService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto loginrequest);

    }
}
