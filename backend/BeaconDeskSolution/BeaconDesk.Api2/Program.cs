using BeaconDesk.Api2.Middleware;
using BeaconDesk.Application.Interfaces.AuthenticacionInterfaces;
using BeaconDesk.Application.Services.AutenticacionServices;
using BeaconDesk.Domain.AunthenticacionModule.Abstractions;
using BeaconDesk.Infraestructure.Persistence.DbContext;
using BeaconDesk.Infraestructure.Persistence.Repositories;
using BeaconDesk.Infraestructure.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

using Serilog;



var builder = WebApplication.CreateBuilder(args);


//----------------------------------------
// CONFIGURACIÓN DE SERILOG
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration) // Lee de appsettings
    .Enrich.FromLogContext() // <--- ¡NECESARIO para que funcione el CorrelationID!
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day) // Archivo diario
    .CreateLogger();

builder.Host.UseSerilog();

// ---------------------------------------------------------
// CONFIGURACIÓN DE SERVICIOS (Service Collection)

var connectionString = builder.Configuration.GetConnectionString("BeaconDesk-AzureDatabase");

builder.Services.AddDbContext<BeaconDeskDbContext>(options =>
        options.UseSqlServer(connectionString));

// Servicios de Autenticación
builder.Services.AddScoped<IUsuarioRepository, ImpUsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, ImpUsuarioService>();
builder.Services.AddScoped<ITokenServices, TokenService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("NewPolicy", app =>
    {
        app.WithOrigins("http://localhost:4200")
           .AllowAnyMethod()
           .AllowAnyHeader();
    });
});


//Inicializador de APP
var app = builder.Build();

// ---------------------------------------------------------
// CONFIGURACIÓN DEL MIDDLEWARE (Pipeline HTTP)

// A. Swagger y Scalar (Deben ir PRIMERO en entorno de desarrollo)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // UI Clásica en /swagger

    // UI de Scalar en /scalar/v1
    app.MapScalarApiReference(options =>
    {
        options.WithOpenApiRoutePattern("/swagger/v1/swagger.json");
    });
}

// B. Redirección y CORS
app.UseHttpsRedirection();
app.UseCors("NewPolicy");


// E. Middleware de Manejo de Excepciones Globales
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();
app.UseSerilogRequestLogging();

// C. Autenticación y Autorización (¡Orden Importante!)
// Primero verificas quién es (Authentication), luego si tiene permiso (Authorization)
app.UseAuthentication();
app.UseAuthorization();

// D. Mapeo de Controladores
app.MapControllers();





// E. Ejecución (SOLO UNA VEZ AL FINAL)
app.Run();