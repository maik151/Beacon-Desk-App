using BeaconDesk.Infraestructure.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace BeaconDesk.Infraestructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // 1. Leemos la cadena de conexión del appsettings.json
            // Nota: Asegúrate de que tu cadena en el json tenga el placeholder {AppDir} 
            // Ejemplo: "TNS_ADMIN={AppDir}\\OracleWallet;..."
            string connectionString = configuration.GetConnectionString("BeaconDesk_OracleDatabase")!;

            // 2. Obtenemos la ruta REAL de donde se está ejecutando la API (sea tu PC o el servidor)
            string pathReal = AppContext.BaseDirectory;

            // 3. Reemplazamos el marcador {AppDir} por la ruta real
            // Si estás en Windows, esto transforma "{AppDir}" en "C:\...\bin\Debug\net8.0\"
            if (connectionString.Contains("{AppDir}"))
            {
                connectionString = connectionString.Replace("{AppDir}", pathReal);
            }

            // 4. Inyectamos el DbContext con la cadena ya corregida
            services.AddDbContext<BeaconDeskDbContext>(options =>
                options.UseOracle(connectionString)
            );

            return services;
        }
    }
}