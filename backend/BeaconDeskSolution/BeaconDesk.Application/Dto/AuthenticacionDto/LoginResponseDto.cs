namespace BeaconDesk.Application.Dto.AuthenticacionDto
{
    /// <summary>
    /// Respuesta exitosa tras un inicio de sesión correcto.
    /// </summary>
    public class LoginResponseDto
    {
        /// <summary>
        /// Token JWT de acceso (Bearer Token).
        /// Úsalo en el header 'Authorization' para futuras peticiones.
        /// </summary>
        /// <example>eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...</example>
        public required string Token { get; set; }

        // Si tuvieras más campos, los documentas aquí:
        // /// <summary>
        // /// Fecha y hora en la que expira el token.
        // /// </summary>
        // public DateTime Expiration { get; set; }
    }
}