using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeaconDesk.Domain.AunthenticacionModule.Entities
{
    public class Equipo
    {
        public int EquipoID { get; set; }
        public string? Nombre { get; set; }

        // Propiedad de Navegación:
        // Un Equipo puede tener muchos Usuarios
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
