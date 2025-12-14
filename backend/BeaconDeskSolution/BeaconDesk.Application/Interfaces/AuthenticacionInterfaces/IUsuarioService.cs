using BeaconDesk.Application.Dto.AuthenticacionDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeaconDesk.Application.Interfaces.AuthenticacionInterfaces
{
    public interface IUsuarioService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto loginrequest);

    }
}
