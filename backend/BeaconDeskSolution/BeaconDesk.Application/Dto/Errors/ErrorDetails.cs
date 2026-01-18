using System.Text.Json;
using System.Text.Json.Serialization; // Necesario para [JsonIgnore]

namespace BeaconDesk.Application.Dto.Errors
{
    public class ErrorDetails
    {
        // Propiedad esencial para el código HTTP (400, 404, 500, etc.)
        public int StatusCode { get; set; }

        // Mensaje amigable para el usuario (ej: "Ticket no encontrado")
        public string Message { get; set; }

        // =======================================================
        // PROPIEDADES AÑADIDAS PARA DEPURACIÓN Y SOPORTE
        // =======================================================

        // ID único para rastrear este error específico en los logs
        public Guid ErrorId { get; set; } = Guid.NewGuid();

        // Fecha y hora del error (usando UTC/Offset para precisión)
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public DateTimeOffset Timestamp { get; set; }

        // Identificador del usuario que causó el error (ej: email)
        public string? UserIdentifier { get; set; }

        // =======================================================

        // Opcional: Para errores de validación (ej: "El campo Nombre es requerido")
        public IDictionary<string, string[]>? Errors { get; set; }

        // Método para serializar (convertir) el objeto a una cadena JSON
        public override string ToString()
        {
            // Usaremos WriteIndented = true para que el JSON se vea bonito y legible en Swagger/Postman.
            return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}