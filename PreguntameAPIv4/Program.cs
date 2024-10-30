using Negocio.Entidades;
using AccesoDatos.Repositorios;
using Negocio.IRepositorios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Servicios.Usuarios;
using System.Text;
using System.Text.Json;
using Servicios.Paises;
using Microsoft.OpenApi.Models;
using Servicios.Preguntas;
using Servicios.Respuestas;

var builder = WebApplication.CreateBuilder(args);

// DB Context
builder.Services.AddDbContext<PreguntameDBv2Context>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("preguntamedb")));

// JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opts =>
    {
        opts.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JWTConfig:Secret")))
        };

        opts.Events = new JwtBearerEvents
        {
            // Obtener el token de la cookie
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["jwtToken"];
                return Task.CompletedTask;
            },

            // Personalizar respuesta para cuando falla [Authorize] en controladores
            OnChallenge = async context =>
            {
                // Evitar respuesta predeterminada
                context.HandleResponse();

                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(JsonSerializer.Serialize(
                    new { message = "Error en autorización, por favor inicie sesión"}));
            }
        };
    });

// Paises
builder.Services.AddScoped<IPaisServicios, PaisServicios>();
// Usuarios
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddScoped<IUsuarioServicios, UsuarioServicios>();
// Preguntas
builder.Services.AddScoped<IPreguntaRepositorio, PreguntaRepositorio>();
builder.Services.AddScoped<IPreguntaServicios, PreguntaServicios>();
// Respuestas
builder.Services.AddScoped<IRespuestaRepositorio, RespuestaRepositorio>();
builder.Services.AddScoped<IRespuestaServicios, RespuestaServicios>();

builder.Services.AddControllers();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        policyBuilder => policyBuilder.WithOrigins("http://localhost:5173") // URL Front
                                      .AllowAnyMethod()
                                      .AllowAnyHeader()
                                      .AllowCredentials()); // Habilita el uso de credenciales (cookies)
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddSwaggerGen(c =>
//{
//    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//    {
//        Name = "Authorization",
//        Type = SecuritySchemeType.Http,
//        Scheme = "bearer",
//        BearerFormat = "JWT",
//        In = ParameterLocation.Header,
//        Description = "Ingrese el token JWT en el formato: Bearer {token}"
//    });

//    c.AddSecurityRequirement(new OpenApiSecurityRequirement
//    {
//        {
//            new OpenApiSecurityScheme
//            {
//                Reference = new OpenApiReference
//                {
//                    Type = ReferenceType.SecurityScheme,
//                    Id = "Bearer"
//                }
//            },
//            new string[] {}
//        }
//    });
//});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthorization();

app.MapControllers();

app.Run();
