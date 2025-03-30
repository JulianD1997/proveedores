using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SuppliersApi.Context;
using SuppliersApi.Services;
using SuppliersApi.Services.Interfaces;
using SuppliersApi.Models;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configuración JWT
var jwtSection = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JWTModel>(jwtSection);
var jwtVariables = jwtSection.Get<JWTModel>();
var key = Encoding.UTF8.GetBytes(jwtVariables.Key);

// Configuración MongoDB
var connectionString = Environment.GetEnvironmentVariable("MONGODB_URI");
builder.Services.AddDbContext<SupplierDbContext>(options =>
{
    options.UseMongoDB(connectionString, "suppliers_db");
});

// Autenticación JWT
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = jwtVariables.Issuer,
            ValidAudience = jwtVariables.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

// Servicios 
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IJwtService, JwtService>();

// Configuración Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Proveedores API",
        Version = "v1",
        Description = "API para gestionar proveedores",
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Autorización",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingresa tu token JWT: [cadena]"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement{
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

// Agregar Controladores
builder.Services.AddControllers();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.Urls.Add("http://*:80");

app.MapControllers();

app.Run();
