using BeaconDesk.Domain.AunthenticacionModule.Entities;

namespace BeaconDesk.Application.Interfaces.AuthenticacionInterfaces
{
    public interface ITokenServices
    {
        string GenerateToken(Usuario usuario);

    }
}
