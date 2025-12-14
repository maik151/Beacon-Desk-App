using System;
using System.Collections.Generic;
using System.Linq;

namespace BeaconDesk.Application.Exceptions
{
    public class ValidationException : Exception
    {
        // 🚨 CAMBIO 1: La propiedad Errors ahora debe ser 'set' privado o público
        // La haremos pública para asignar el diccionario completo fácilmente.
        public IDictionary<string, string[]>? Errors { get; set; } // Permitimos 'set'

        // Constructor base
        public ValidationException()
            : base("Uno o más errores de validación ocurrieron.")
        {
            Errors = new Dictionary<string, string[]>();
        }

        // 🚨 CAMBIO 2: Añadir el constructor para recibir el diccionario de errores
        // Esto es esencial para que tus servicios puedan lanzar la excepción con los errores
        public ValidationException(IDictionary<string, string[]> validationErrors)
            : this() // Llama al constructor base para el mensaje general
        {
            // Asigna los errores de validación pasados
            Errors = validationErrors;
        }

        // NOTA: Mantenemos tu constructor simple si lo usas:
        public ValidationException(string message) : base(message)
        {
            Errors = new Dictionary<string, string[]>();
        }
    }
}