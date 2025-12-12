using BeaconDesk.Application.Interfaces.AuthenticacionInterfaces;
using BeaconDesk.Application.Services.AutenticacionServices;
using BeaconDesk.Domain.AunthenticacionModule.Abstractions;
using BeaconDesk.Infraestructure.Persistence.DbContext;
using BeaconDesk.Infraestructure.Persistence.Repositories;
using BeaconDesk.Infraestructure.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
// IMPORTANTE: Añade el using para poder acceder a tu middleware
using BeaconDesk.Api2.Middleware;
using Microsoft.AspNetCore.Builder; // Asegúrate de que este using esté si lo necesitas para métodos de extensión

var builder = WebApplication.CreateBuilder(args);

//--------Configuracion de Servicios ------------------

//Configuracion de cadena de conexion
var connectionString = builder.Configuration.GetConnectionString("BeaconDesk-AzureDatabase");

//Registar Inyeccion de dependencias del DbContext y demas Servicios

builder.Services.AddDbContext<BeaconDeskDbContext>(options =>
  options.UseSqlServer(connectionString));

// Add services to the container.
//Registro de Servicios de Autenticacion
builder.Services.AddScoped<IUsuarioRepository, ImpUsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, ImpUsuarioService>();
builder.Services.AddScoped<ITokenServices, TokenService>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Solucion para el tema del CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("NewPolicy", app =>
    {
        app.WithOrigins("http://localhost:4200") // <--- La URL exacta de tu Angular
               .AllowAnyMethod()                     // Permite GET, POST, PUT, DELETE
               .AllowAnyHeader();                    // Permite enviar Tokens y Content-Type
    });
});


var app = builder.Build();

// =======================================================
// 1. TU MIDDLEWARE DE EXCEPCIONES (DEBE IR PRIMERO)
// =======================================================
app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapScalarApiReference(options =>
    {
        options.WithOpenApiRoutePattern("/swagger/v1/swagger.json");
    });
}

// =======================================================
// 2. MIDDLEWARES DEL LOGIN/CORS/SEGURIDAD (DEL CÓDIGO DE DEVELOP)
// =======================================================
app.UseHttpsRedirection();
app.UseCors("NewPolicy"); // Debe ir antes de UseAuthorization/UseAuthentication
app.UseAuthentication();  // Necesario para el Login
app.UseAuthorization();
// OJO: Si usas UseAuthentication, también deberás configurarlo en builder.Services (JWT Bearer, etc.), 
// pero ese código debe estar en la sección de builder.Services.

app.MapControllers();

app.Run();