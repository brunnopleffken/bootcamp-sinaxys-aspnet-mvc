using System.Text;
using Bookstore.Web.Data;
using Bookstore.Web.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql("Server=localhost;Username=brunnopleffken;Password=;Database=bookstore"); // string de conexão com o banco de dados
    options.UseSnakeCaseNamingConvention(); // converte nomes de propriedades para snake_case
    options.EnableSensitiveDataLogging(); // mostra dados sensíveis no log do banco de dados (DEV ONLY!)
});

builder.Services.AddIdentityCore<Customer>(options =>
    {
        // personaliza as regras de validação do ASP.NET Identity
        options.Password.RequiredLength = 8;
        options.User.RequireUniqueEmail = true;
        options.SignIn.RequireConfirmedEmail = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>() // configura o Identity para usar a classe ApplicationDbContext
    .AddDefaultTokenProviders(); // adiciona os provedores de token padrão p/ recuperação de senha e confirmação de email

// Nossa chave privada secreta super segura para assinar os tokens JWT
// Obviamente, aqui deixamos hard-coded, mas em um cenário real, essa chave deve ser armazenada de forma segura
const string secret = "CH4V3-S3CR374-B3M-53GUR5-D1F1C1L";

// No projeto Web/MVC, usamos AddCookies() para configurar a autenticação baseada em cookies.
// Já na API, vamos usar AddJwtBearer() para configurar a autenticação baseada em tokens JWT (JSON Web Tokens).
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters.ValidateIssuerSigningKey = true;
        options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

        options.TokenValidationParameters.ValidateIssuer = true;
        options.TokenValidationParameters.ValidIssuer = "BookstoreApp"; // o emissor do token (quem criou o token)

        options.TokenValidationParameters.ValidateAudience = true;
        options.TokenValidationParameters.ValidAudience = "BookstoreApp"; // o público-alvo do token (quem pode usar o token)
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
