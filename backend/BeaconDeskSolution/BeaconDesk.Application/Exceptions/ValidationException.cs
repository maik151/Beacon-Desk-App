using System;
using System.Collections.Generic;
using System.Linq; // Necesario para el método ToArray() si usas FluentValidation

namespace BeaconDesk.Application.Exceptions
{
    // 1. Debe ser pública (public) y 2. debe heredar de Exception
    public class ValidationException : Exception
    {
        // 3. Propiedad para almacenar los errores: Campo (Key) -> Lista de Mensajes (Value)
        public IDictionary<string, string[]> Errors { get; }

        // Constructor base
        public ValidationException()
            : base("Uno o más errores de validación ocurrieron.")
        {
            // Inicializa el diccionario vacío
            Errors = new Dictionary<string, string[]>();
        }

        /* // 4. Constructor común cuando se usa FluentValidation: 
        // Permite pasar los resultados del validador directamente.
        public ValidationException(IEnumerable<ValidationFailure> failures) : this()
        {
            Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, 
                              failureGroup => failureGroup.ToArray());
        }
        */

        // Versión simple (si no usas FluentValidation)
        public ValidationException(string message) : base(message)
        {
            Errors = new Dictionary<string, string[]>();
        }
    }
}