using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeaconDesk.Domain.AunthenticacionModule.Entities
{
    public class Rol
    {
        public int RolID { get; set; }
        public string? Nombre { get; set; }

        // Propiedad de Navegación:
        // Un Rol puede tener muchos Usuarios
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
