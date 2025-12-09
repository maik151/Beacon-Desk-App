using System.Net;
using System.Text.Json; // Necesitas este namespace para la serialización

namespace BeaconDesk.Application.Dto.Errors
{
    // 1. Cambiar a 'public' para que la capa API pueda acceder a ella.
    public class ErrorDetails
    {
        // Propiedad esencial para el código HTTP (400, 404, 500, etc.)
        public int StatusCode { get; set; }

        // Mensaje amigable para el usuario (ej: "Ticket no encontrado")
        public string Message { get; set; }

        // Opcional: Para errores de validación (ej: "El campo Nombre es requerido")
        public IDictionary<string, string[]> Errors { get; set; }

        // Método para serializar (convertir) el objeto a una cadena JSON
        public override string ToString()
        {
            // Usamos System.Text.Json para serializar el objeto a la respuesta HTTP
            return JsonSerializer.Serialize(this);
        }
    }
}