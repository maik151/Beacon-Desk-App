using BeaconDesk.Application.Interfaces.AuthenticacionInterfaces;
using BeaconDesk.Application.Services.AutenticacionServices;
using BeaconDesk.Domain.AunthenticacionModule.Abstractions;
using BeaconDesk.Infraestructure.Persistence.DbContext;
using BeaconDesk.Infraestructure.Persistence.Repositories;
using BeaconDesk.Infraestructure.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;


var builder = WebApplication.CreateBuilder(args);

// ---------------------------------------------------------
// 1. CONFIGURACI�N DE SERVICIOS (Service Collection)
// ---------------------------------------------------------

var connectionString = builder.Configuration.GetConnectionString("BeaconDesk-AzureDatabase");



//Registar Inyeccion de dependencias del DbContext y demas Servicios

builder.Services.AddDbContext<BeaconDeskDbContext>(options =>
  options.UseSqlServer(connectionString));

// Servicios de Autenticaci�n
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
        app.WithOrigins("http://localhost:4200") // <--- La URL exacta de tu Angular
           .AllowAnyMethod()                     // Permite GET, POST, PUT, DELETE
           .AllowAnyHeader();                    // Permite enviar Tokens y Content-Type
    });
});

// --- AQU� SE CONSTRUYE LA APP ---
var app = builder.Build();

//Configuacion del Middleware para CORS 
app.UseHttpsRedirection();
app.UseCors("NewPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // UI Cl�sica en /swagger

    // UI de Scalar en /scalar/v1
    app.MapScalarApiReference(options =>
    {
        options.WithOpenApiRoutePattern("/swagger/v1/swagger.json");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();
// OJO: Si usas UseAuthentication, tambi�n deber�s configurarlo en builder.Services (JWT Bearer, etc.), 
// pero ese c�digo debe estar en la secci�n de builder.Services.

// D. Mapeo de Controladores
app.MapControllers();

// E. Ejecuci�n (SOLO UNA VEZ AL FINAL)
app.Run();