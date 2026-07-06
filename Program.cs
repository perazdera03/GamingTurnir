using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using GamingTurnir.Data;

var builder = WebApplication.CreateBuilder(args);

// Dozvoljava zahteve sa bilo kog porekla (frontend na drugom portu moze da poziva API)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Swagger - automatski generise dokumentaciju i UI za testiranje API endpointa
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Gaming Turnir API", Version = "v1" });

    // Dodaje polje za unos JWT tokena u Swagger UI (dugme "Authorize")
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Unesi token"
    });

    // Zahteva token za sve zasticene endpointe u Swagger UI
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        }
    });
});

// Konekcija ka MySQL bazi - connection string se cita iz appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)));

// JWT podesavanja - kljuc, issuer i audience se citaju iz appsettings.json
var jwtSection = builder.Configuration.GetSection("Jwt");
var issuer = jwtSection["Issuer"];
var audience = jwtSection["Audience"];
var key = jwtSection["Key"];
var keyBytes = Encoding.UTF8.GetBytes(key!);

// Konfiguracija JWT autentifikacije - proverava svaki pristigli token
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero  // Token istice tacno na vreme, bez tolerancije
        };
    });

// Registruje servise za autorizaciju (role) i kontrolere (API endpointi)
builder.Services.AddAuthorization();
builder.Services.AddControllers();

var app = builder.Build();

// Swagger dostupan samo u development okruzenju, ne u produkciji
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");        // CORS mora biti pre autentifikacije
app.UseAuthentication();        // Citanje i validacija JWT tokena iz headera
app.UseAuthorization();         // Provera rola ([Authorize(Roles="Admin")] itd.)
app.UseStaticFiles();           // Servisira klijent.html iz wwwroot foldera
app.MapControllers();           // Povezuje URL rute sa kontrolerima

app.Run();
