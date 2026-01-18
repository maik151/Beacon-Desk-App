namespace BeaconDesk.Application.Exceptions
{
    // 1. Debe ser pública (public) y 2. debe heredar de Exception
    public class NotFoundException : Exception
    {
        // Constructor que acepta solo un mensaje
        public NotFoundException(string message)
            : base(message)
        {
        }

        // 3. Constructor más específico: Permite lanzar la excepción indicando la entidad y el ID.
        public NotFoundException(string name, object key)
            : base($"La entidad \"{name}\" ({key}) no fue encontrada.")
        {
        }
    }
}