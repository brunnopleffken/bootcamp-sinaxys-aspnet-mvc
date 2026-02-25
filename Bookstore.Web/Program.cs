using Bookstore.Web.Data;
using Bookstore.Web.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddLogging();

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

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme) // configura a autenticação para usar cookies
    .AddCookie(options =>
    {
        // personaliza as opções de cookie, como o caminho para a página de login e acesso negado
        options.LoginPath = "/Login";
        options.AccessDeniedPath = "/AccessDenied";
    });

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication(); // quem é
app.UseAuthorization(); // o que pode fazer

app.MapStaticAssets();

app.MapControllerRoute(
    name: "static",
    pattern: "{action=Home}",
    defaults: new { controller = "Pages" });

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Pages}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
