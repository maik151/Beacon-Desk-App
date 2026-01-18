using BeaconDesk.Domain.AunthenticacionModule.Entities;

namespace BeaconDesk.Domain.AunthenticacionModule.Abstractions
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> GetByEmailAsync(string email);
    }
}
