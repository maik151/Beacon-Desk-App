using BeaconDesk.Api2.Filters;
using BeaconDesk.Api2.Middleware;
using BeaconDesk.Application.Dto.AuthenticacionDto;
using BeaconDesk.Application.Interfaces.AuthenticacionInterfaces;
using BeaconDesk.Application.Services.AutenticacionServices;
using BeaconDesk.Domain.AunthenticacionModule.Abstractions;
using BeaconDesk.Infraestructure;
using BeaconDesk.Infraestructure.Persistence.DbContext;
using BeaconDesk.Infraestructure.Persistence.Repositories;
using BeaconDesk.Infraestructure.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;
using System.Reflection;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------------------------------------
// CONFIGURACIÓN DE LOGS (Serilog)
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// ---------------------------------------------------------
// BASE DE DATOS

//var connectionString = builder.Configuration.GetConnectionString("BeaconDesk-AzureDatabase");
//builder.Services.AddDbContext<BeaconDeskDbContext>(options =>
//        options.UseSqlServer(connectionString));

builder.Services.AddInfrastructure(builder.Configuration);


builder.Services.AddHealthChecks()
    .AddDbContextCheck<BeaconDeskDbContext>(
        name: "Oracle-Database-Check",
        // timeout: TimeSpan.FromSeconds(3), // <--- ESTA LINEA DEBES BORRARLA, NO EXISTE EN ESTE MÉTODO
        failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy, // Opcional: Define qué pasa si falla
        tags: new[] { "db", "oracle" } // Opcional: Para agrupar checks
    );

// ---------------------------------------------------------
// CONFIGURACIÓN DE VALIDACIÓN (FluentValidation)
builder.Services.AddValidatorsFromAssemblyContaining<BeaconDesk.Application.Validation.LoginRequestValidator>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// ---------------------------------------------------------
// CONTROLADORES Y FILTROS

builder.Services.AddControllers(options =>
{
    options.Filters.Add<CorrelationIdResultFilter>();
    options.Filters.Add<ValidationFilter>();
});

// ---------------------------------------------------------
// SERVICIOS DE APLICACIÓN (Inyección de Dependencias)
builder.Services.AddScoped<IUsuarioRepository, ImpUsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, ImpUsuarioService>();
builder.Services.AddScoped<ITokenServices, TokenService>();

// ---------------------------------------------------------
// SWAGGER Y DOCUMENTACIÓN
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Beacon Desk API",
        Version = "v1",
        Description = "Documentación profesional."
    });

    
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    var assemblyApplication = Assembly.GetAssembly(typeof(LoginRequestDto));
    var xmlFilenameApp = $"{assemblyApplication.GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilenameApp));
});

// ---------------------------------------------------------
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

// =========================================================
// CONSTRUCCIÓN DE LA APP
var app = builder.Build();
// =========================================================

// ---------------------------------------------------------
// PIPELINE (Middleware)

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapScalarApiReference(options =>
    {
        options.WithOpenApiRoutePattern("/swagger/v1/swagger.json");
    });
}

app.UseHttpsRedirection();
app.UseCors("NewPolicy");

// Middlewares personalizados (Manejo de errores y Logs)
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();
app.UseSerilogRequestLogging();

// Autenticación
app.UseAuthentication();
app.UseAuthorization();

// Mapeo final
app.MapControllers();

//Endpotin de Health
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";

        var response = new
        {
            status = report.Status.ToString(),
            checkedAt = DateTime.UtcNow,
            duration = report.TotalDuration.TotalMilliseconds + " ms",
            services = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                // AGREGA ESTA LÍNEA PARA VER EL MENSAJE DE ORACLE:
                error = e.Value.Exception?.Message,

                description = e.Value.Description,
                duration = e.Value.Duration.TotalMilliseconds + " ms"
            })
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
});

app.Run();
