using BeaconDesk.Application.Interfaces.AuthenticacionInterfaces;
using BeaconDesk.Application.Services.AutenticacionServices;
using BeaconDesk.Domain.AunthenticacionModule.Abstractions;
using BeaconDesk.Infraestructure.Persistence.DbContext;
using BeaconDesk.Infraestructure.Persistence.Repositories;
using BeaconDesk.Infraestructure.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;


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
           .AllowAnyHeader();                    // Permite enviar Tokens y Content-Type
    });
});


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
    app.UseSwaggerUI();
    app.MapScalarApiReference(options =>
    {
        // Aquí le decimos a Scalar dónde encontrar el JSON de Swagger
        // (Esta NO es la opción ProxyUrl que rompía las pruebas)
        options.WithOpenApiRoutePattern("/swagger/v1/swagger.json");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
