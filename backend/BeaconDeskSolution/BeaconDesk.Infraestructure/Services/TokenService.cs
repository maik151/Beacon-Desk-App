using BeaconDesk.Application.Interfaces.AuthenticacionInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeaconDesk.Application.Interfaces;
using BeaconDesk.Domain.AunthenticacionModule.Entities;
using Microsoft.Extensions.Configuration; // Para leer appsettings
using Microsoft.IdentityModel.Tokens;   // Para la llave
using System.IdentityModel.Tokens.Jwt;  // Para el Handler
using System.Security.Claims;           // Para los Claims


namespace BeaconDesk.Infraestructure.Services
{
    public class TokenService:ITokenServices
    {
        private readonly SymmetricSecurityKey _key;
        private readonly string _issuer;

        public TokenService(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:Key"]!));
            _issuer = config["JwtSettings:Issuer"]!;
        }

        public string GenerateToken(Usuario usuario) {

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, usuario.UsuarioID.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, usuario.Email!),
                new Claim(ClaimTypes.Role, usuario.Rol.Nombre!)
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = creds,
                Issuer = _issuer
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }


    }
}
