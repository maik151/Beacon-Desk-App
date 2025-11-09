using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeaconDesk.Application.Dto.AuthenticacionDto
{
    public class LoginRequestDto
    {
        public string? Email { get; set; }
        public string? Password { get; set; }

    }
}
