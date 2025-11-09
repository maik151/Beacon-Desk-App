using BeaconDesk.Domain.AunthenticacionModule.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeaconDesk.Application.Interfaces.AuthenticacionInterfaces
{
    public interface ITokenServices
    {
        string GenerateToken(Usuario usuario);

    }
}
