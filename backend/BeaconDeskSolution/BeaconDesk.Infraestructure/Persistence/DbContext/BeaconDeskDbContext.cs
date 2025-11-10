using BeaconDesk.Domain.AunthenticacionModule.Entities;
using Microsoft.EntityFrameworkCore;

namespace BeaconDesk.Infraestructure.Persistence.DbContext
{
    public  class BeaconDeskDbContext : Microsoft.EntityFrameworkCore.DbContext
    {

        public BeaconDeskDbContext(DbContextOptions<BeaconDeskDbContext> options) : base(options)
        {
        }


        //Dentro de este contexto definimos las tablas que existen
        //Tablas del grupo de Login

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Equipo> Equipos{ get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
