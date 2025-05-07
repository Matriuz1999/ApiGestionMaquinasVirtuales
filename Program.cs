using ApiGestionMaquinasVirtuales.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ApiGestionMaquinasVirtuales.Interfaces;
using ApiGestionMaquinasVirtuales.Services;
using ApiGestionMaquinasVirtuales.Repositories;
using ApiGestionMaquinasVirtuales.Hubs;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// ---------- CONFIGURACIÓN DE SERVICIOS ----------

// Base de datos
builder.Services.AddDbContext<GestionmvContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("connectionDB")));

// Servicios propios
builder.Services.AddScoped<IMaquinaVirtualRepository, MaquinaVirtualRepository>();
builder.Services.AddScoped<MaquinasVirtualesHubService>();
builder.Services.AddScoped<IMaquinaVirtualService, MaquinaVirtualService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Controladores y documentación
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

// SignalR
builder.Services.AddSignalR();

// CORS
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Autenticación JWT
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };

        options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;

                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/maquinasVirtualesHub"))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });

// Autorización por roles
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Administrador"));
    options.AddPolicy("RequireClientRole", policy => policy.RequireRole("Cliente"));
});

// ---------- CONFIGURACIÓN DE MIDDLEWARES ----------

var app = builder.Build();

// Manejo global de excepciones
app.UseExceptionHandler(appError =>
{
    appError.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(new
        {
            message = "Ocurrió un error interno. Por favor, inténtelo más tarde."
        }));
    });
});

// Redirección a Swagger desde la raíz
app.MapGet("/", (HttpContext context) =>
{
    context.Response.Redirect("/swagger/index.html", permanent: false);
});

// Swagger solo en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection(); // Redirección HTTP → HTTPS solo en producción
}

// Middleware de CORS, autenticación y autorización
app.UseCors("AllowAllOrigins");
app.UseAuthentication();
app.UseAuthorization();

// Controladores
app.MapControllers();

// SignalR (con política CORS)
app.MapHub<MaquinasVirtualesHub>("/maquinasVirtualesHub").RequireCors("AllowAllOrigins");

// Ejecutar aplicación
app.Run();
