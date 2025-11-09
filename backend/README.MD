Clonar el repositorio git clone https://github.com/maik151/Beacon-Desk-App.git

Navegar al directorio del backend cd Beacon-Desk-App/backend

Configurar los secretos de desarrollo

Copia el archivo appsettings.Development.example.json y renómbralo a appsettings.Development.json.

Edita el nuevo appsettings.Development.json y completa los valores que faltan:

ConnectionStrings:DefaultConnection: Pega tu cadena de conexión completa de Azure SQL.

JwtSettings:Key: Pega tu llave secreta (debe tener al menos 64 caracteres).

Ejecutar la API dotnet run