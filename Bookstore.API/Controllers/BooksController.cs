using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class BooksController : ControllerBase
{
    public IActionResult Index()
    {
        return Ok(new
        {
            Book = "O Senhor dos Anéis",
            Author = "J.R.R. Tolkien"
        });
    }
}
