using ApiGestionMaquinasVirtuales.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ApiGestionMaquinasVirtuales.Interfaces;
using ApiGestionMaquinasVirtuales.Services;
using ApiGestionMaquinasVirtuales.Repositories;
using ApiGestionMaquinasVirtuales.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Configuraci�n de la base de datos
builder.Services.AddDbContext<GestionmvContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("connectionDB")));

// Configuraci�n de los servicios de la API
builder.Services.AddControllers();

// Registrar SignalR
builder.Services.AddSignalR();

// Registrar servicios personalizados
builder.Services.AddScoped<IMaquinaVirtualRepository, MaquinaVirtualRepository>();
builder.Services.AddScoped<MaquinasVirtualesHubService>();
builder.Services.AddScoped<IMaquinaVirtualService, MaquinaVirtualService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IAuthService, AuthService>();

var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();

// Configuraci�n de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        // Permite solicitudes desde cualquier origen, pero puedes restringirlo a uno espec�fico para mayor seguridad
        policy
            .WithOrigins(allowedOrigins) // Aseg�rate de que la URL de tu frontend est� aqu�
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // Espec�ficamente necesario para SignalR
    });
});

// Configuraci�n de Swagger para documentaci�n de la API
builder.Services.AddSwaggerGen();

// Configuraci�n de autenticaci�n JWT
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

        // Configurar SignalR para usar tokens JWT
        options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;

                // Aseg�rate de que el path y el token est�n correctos para SignalR
                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/maquinasVirtualesHub"))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });

// Configurar pol�ticas de autorizaci�n basadas en roles
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Administrador"));
    options.AddPolicy("RequireClientRole", policy => policy.RequireRole("Cliente"));
});

var app = builder.Build();

// Middleware de manejo de excepciones global
app.UseExceptionHandler(appError =>
{
    appError.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(new
        {
            message = "Ocurri� un error interno. Por favor, int�ntelo m�s tarde."
        }));
    });
});

// Habilitar CORS
app.UseCors("AllowAllOrigins");

// Configuraci�n de la redirecci�n a Swagger en la ra�z
app.MapGet("/", (HttpContext context) =>
{
    context.Response.Redirect("/swagger/index.html", permanent: false);
});

// Configuraci�n del pipeline de solicitudes
if (app.Environment.IsDevelopment())
{
    // Habilita Swagger solo en el entorno de desarrollo
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Habilita la autenticaci�n y autorizaci�n
app.UseAuthentication();
app.UseAuthorization();

// Redirecci�n de HTTP a HTTPS en entornos de producci�n
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// Configuraci�n de los controladores
app.MapControllers();

// Mapear el hub de SignalR
app.MapHub<MaquinasVirtualesHub>("/maquinasVirtualesHub");

// Ejecuta la aplicaci�n
app.Run();
