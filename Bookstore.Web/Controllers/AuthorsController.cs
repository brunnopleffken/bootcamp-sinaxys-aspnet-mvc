using Bookstore.Web.Data;
using Bookstore.Web.Entities;
using Bookstore.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Web.Controllers;

public class AuthorsController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        List<Author> authors = await context.Authors.ToListAsync();
        return View(authors);
    }

    public async Task<IActionResult> Show(int id)
    {
        AuthorResponse? author = await context.Authors
            .Where(a => a.Id == id)
            .Select(a => new AuthorResponse(
                Id: a.Id,
                FullName: a.FullName,
                Bio: a.Bio,
                Books: a.Books.ToList()
            ))
            .FirstOrDefaultAsync();

        if (author is null)
            return RedirectToAction("Index");

        return View(author);
    }

    public IActionResult New()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(NewAuthorRequest authorRequest)
    {
        Author author = new()
        {
            FullName = authorRequest.FullName,
            Bio = authorRequest.Bio
        };

        context.Authors.Add(author);
        await context.SaveChangesAsync();

        return RedirectToAction("Index");
    }
}
