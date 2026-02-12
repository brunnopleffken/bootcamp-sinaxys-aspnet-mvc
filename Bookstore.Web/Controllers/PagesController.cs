using Bookstore.Web.Data;
using Bookstore.Web.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Web.Controllers;

public class PagesController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        List<Book> books = await context.Books
            .Include(b => b.Authors)
            .OrderBy(b => b.Title)
            .ToListAsync();

        return View(books);
    }

    [Route("{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        Book? book = await context.Books
            .Include(b => b.Authors)
            .Where(b => b.Id == id)
            .FirstOrDefaultAsync();

        if (book is null)
            return NotFound("O livro não foi encontrado");

        return View(book);
    }
}
