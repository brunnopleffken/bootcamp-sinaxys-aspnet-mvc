using Bookstore.Web.Data;
using Bookstore.Web.Entities;
using Bookstore.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Web.Controllers;

[Authorize]
public class PublishersController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        List<Publisher> publishers = await context.Publishers.ToListAsync();
        return View(publishers);
    }

    [Authorize(Roles = "Author,Publisher")]
    public IActionResult New()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Author,Publisher")]
    public async Task<IActionResult> Create(NewPublisherRequest publisherRequest)
    {
        Publisher publisher = new Publisher()
        {
            Name = publisherRequest.Name,
            City = publisherRequest.City,
            Country = publisherRequest.Country
        };

        context.Publishers.Add(publisher);
        await context.SaveChangesAsync();

        return RedirectToAction("Index");
    }
}
