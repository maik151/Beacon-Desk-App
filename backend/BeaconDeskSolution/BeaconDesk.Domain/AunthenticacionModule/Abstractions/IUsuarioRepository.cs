using BeaconDesk.Domain.AunthenticacionModule.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeaconDesk.Domain.AunthenticacionModule.Abstractions
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> GetByEmailAsync(string email);
    }
}
