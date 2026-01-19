using System.Diagnostics;
using Bookstore.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Bookstore.Web.Models;

namespace Bookstore.Web.Controllers;

public class PagesController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
