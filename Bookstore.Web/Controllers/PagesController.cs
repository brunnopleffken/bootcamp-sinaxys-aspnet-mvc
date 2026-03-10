using Bookstore.Web.Data;
using Bookstore.Web.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Bookstore.Web.Controllers;

public class PagesController(ApplicationDbContext context, IMemoryCache cache) : Controller
{
    public async Task<IActionResult> Index()
    {
        List<Book>? books = await cache.GetOrCreateAsync("books", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(12);

            return await context.Books
                .Include(b => b.Authors)
                .OrderBy(b => b.Title)
                .ToListAsync();
        });

        return View(books);
    }

    public IActionResult AccessDenied() => View();

    [Route("{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        // Obtém o cache para cada livro, usando o id como parte da chave (ex: "book_1", "book_2", etc.)
        // Se o cache não existe, o GetOrCreateAsync irá criar o cache para aquele livro específico,
        // buscando os dados do banco de dados e armazenando-os no cache para futuras requisições.
        Book? book = await cache.GetOrCreateAsync($"book_{id}", async entry =>
        {
            // Define uma data fixa para expiração do cache (1 hora a partir do momento em que o cache é criado)
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

            // ...ou... define uma expiração dinâmica para o cache (1 hora a partir da última vez que o cache foi acessado)
            // entry.SlidingExpiration = TimeSpan.FromHours(1);

            // Segue o SQL normal do Entity Framework...
            return await context.Books
                .Include(b => b.Authors)
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync();
        });

        if (book is null)
            return NotFound("O livro não foi encontrado");

        return View(book);
    }
}
