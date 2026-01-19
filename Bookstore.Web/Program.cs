using Bookstore.Web.Data;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql("Server=localhost;Username=brunnopleffken;Password=;Database=bookstore"); // string de conexão com o banco de dados
    options.UseSnakeCaseNamingConvention(); // converte nomes de propriedades para snake_case
    options.EnableSensitiveDataLogging(); // mostra dados sensíveis no log do banco de dados (DEV ONLY!)
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

app.UseAuthorization();

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
