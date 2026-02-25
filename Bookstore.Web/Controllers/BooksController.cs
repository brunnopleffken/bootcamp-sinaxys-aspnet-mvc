using Bookstore.Web.Data;
using Bookstore.Web.Entities;
using Bookstore.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Web.Controllers;

[Authorize(Roles = "Author")]
public class BooksController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        List<Book> books = await context.Books
            .Include(b => b.Authors)
            .OrderBy(b => b.Title)
            .ToListAsync();

        return View(books);
    }

    public IActionResult Show(int id)
    {
        return View();
    }

    public async Task<IActionResult> New([FromQuery] int category)
    {
        List<Category> categories = await context.Categories.OrderBy(c => c.Name).ToListAsync();
        ViewBag.Categories = new SelectList(categories, "Id", "Name", category);

        List<Publisher> publishers = await context.Publishers.OrderBy(p => p.Name).ToListAsync();
        ViewBag.Publishers = new SelectList(publishers, "Id", "Name");

        List<Author> authors = await context.Authors.OrderBy(a => a.FullName).ToListAsync();
        ViewBag.Authors = new MultiSelectList(authors, "Id", "FullName");

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(NewBookRequest bookRequest)
    {
        Book book = new Book()
        {
            Title = bookRequest.Title,
            Description = bookRequest.Description,
            Isbn = bookRequest.Isbn,
            Price = bookRequest.Price,
            Format = bookRequest.Format,
            CategoryId = bookRequest.CategoryId,
            PublisherId = bookRequest.PublisherId,
            Authors = await context.Authors
                .Where(a => bookRequest.AuthorIds.Contains(a.Id))
                .ToListAsync()
        };

        context.Books.Add(book);
        await context.SaveChangesAsync();

        return RedirectToAction("Index");
    }
}
