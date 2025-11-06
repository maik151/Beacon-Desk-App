"1. Clonar el repositorio: 

 git clone https://github.com/maik151/Beacon-Desk-App.git

 2. Navegar al Backend: 

 cd /backend 

 3. Configurar Credenciales: 

 * Renombrar appsettings.Development.example.json a appsettings.Development.json. 

 * Llenar la cadena de conexión de Oracle (usando las credenciales del Wallet). 

 * Configurar el JwtSecret (una clave secreta larga). 

 4. Instalar dependencias y correr: 

 dotnet restore 

 dotnet build 

 dotnet run (El servidor (ej. localhost:7123) ahora está corriendo y la documentación de Swagger estará disponible en /swagger)."		