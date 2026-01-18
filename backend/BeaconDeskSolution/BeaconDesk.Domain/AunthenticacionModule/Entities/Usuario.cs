namespace BeaconDesk.Domain.AunthenticacionModule.Entities
{
    public class Usuario
    {
        public int UsuarioID { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? NombreCompleto { get; set; } // El '?' lo hace nulable
        public bool EstaActivo { get; set; }

        // --- Llaves Foráneas (Relaciones) ---

        // Relación con Rol (Un Usuario TIENE UN Rol)
        public int RolID { get; set; }
        public virtual Rol Rol { get; set; }

        // Relación con Equipo (Un Usuario PUEDE TENER UN Equipo)
        public int? EquipoID { get; set; } // El '?' lo hace nulable
        public virtual Equipo? Equipo { get; set; }
    }
}
