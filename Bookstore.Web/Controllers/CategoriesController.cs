using Bookstore.Web.Data;
using Bookstore.Web.Entities;
using Bookstore.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Web.Controllers;

public class CategoriesController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        List<Category> categories = await context.Categories.ToListAsync();
        return View(categories);
    }

    public IActionResult New()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(NewCategoryRequest categoryRequest)
    {
        Category category = new()
        {
            Name = categoryRequest.Name
        };

        context.Categories.Add(category);
        await context.SaveChangesAsync();

        return RedirectToAction("Index");
    }
}
