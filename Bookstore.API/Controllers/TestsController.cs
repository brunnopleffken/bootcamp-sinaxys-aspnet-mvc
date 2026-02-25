using Microsoft.AspNetCore.Mvc;

namespace Bookstore.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TestsController : ControllerBase
{
    public IActionResult Index()
    {
        return Ok(new { Message = "Deu certo!" });
    }
}
