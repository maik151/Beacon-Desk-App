namespace BeaconDesk.Application.Dto.AuthenticacionDto
{
    /// <summary>
    /// Datos requeridos para iniciar sesión en el sistema.
    /// </summary>
    public class LoginRequestDto
    {
        /// <summary>
        /// Correo electrónico corporativo del usuario.
        /// </summary>
        /// <example>usuario@empresa.com</example>
        public string Email { get; set; }

        /// <summary>
        /// Contraseña del usuario. Debe cumplir con las políticas de seguridad (mínimo 8 caracteres).
        /// </summary>
        /// <example>P@ssw0rd123!</example>
        public string Password { get; set; }
    }
}