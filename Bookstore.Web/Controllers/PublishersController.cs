using Bookstore.Web.Data;
using Bookstore.Web.Entities;
using Bookstore.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Web.Controllers;

public class PublishersController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        List<Publisher> publishers = await context.Publishers.ToListAsync();
        return View(publishers);
    }

    public IActionResult New()
    {
        return View();
    }

    [HttpPost]
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
