
using BeaconDesk.Domain.AunthenticacionModule.Abstractions;
using BeaconDesk.Domain.AunthenticacionModule.Entities;
using BeaconDesk.Infraestructure.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;


namespace BeaconDesk.Infraestructure.Persistence.Repositories
{
    public class ImpUsuarioRepository : IUsuarioRepository
    {
        //Declaramos Siempre las variales de contexto
        //En este caso la variable es _context para la clase BeaconDeskDbContext

        private readonly BeaconDeskDbContext _context;

        //Construimos el constructor para inicializar el contexto

        public ImpUsuarioRepository(BeaconDeskDbContext context)
        {
            _context = context;
        }


        //Desarrollo de los servicios de la interfaz IUsuarioRepository
        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            return await _context.Usuarios.Include(u => u.Rol).FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
