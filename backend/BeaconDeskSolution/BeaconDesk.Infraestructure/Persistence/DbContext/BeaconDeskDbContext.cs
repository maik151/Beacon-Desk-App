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

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.Property(e => e.EstaActivo).HasColumnType("NUMBER(1)");
            });



            // =================================================================
            // DATA SEEDING (Datos Iniciales)
            // =================================================================
            modelBuilder.Entity<Rol>().HasData(
                new Rol { RolID = 1, Nombre = "Administrador" },
                new Rol { RolID = 2, Nombre = "Soporte" },
                new Rol { RolID = 3, Nombre = "Usuario" }
            );

            
            modelBuilder.Entity<Equipo>().HasData(
                new Equipo { EquipoID = 1, Nombre = "Desarrollo" },
                new Equipo { EquipoID = 2, Nombre = "QA" },
                new Equipo { EquipoID = 3, Nombre = "Cyberseguridad" },
                new Equipo { EquipoID = 4, Nombre = "DBA" },
                new Equipo { EquipoID = 5, Nombre = "Infraestructura-Servidores" },
                new Equipo { EquipoID = 6, Nombre = "Infraestructura-Redes" },
                new Equipo { EquipoID = 7, Nombre = "Departamento Tecnico" },
                new Equipo { EquipoID = 8, Nombre = "Soporte de Aplicacion" },
                new Equipo { EquipoID = 9, Nombre = "BI" }
            );
        }

    }
}
