using Bookstore.Web.Data;
using Bookstore.Web.Entities;
using Bookstore.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Web.Controllers;

[Route("crie-sua-conta")]
public class SignupsController(UserManager<Customer> userManager) : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(NewSignupRequest newSignupRequest)
    {
        if (!ModelState.IsValid)
            return View("Index", newSignupRequest);

        Customer customer = new Customer()
        {
            FullName = newSignupRequest.FullName,
            UserName = newSignupRequest.Email,
            Email = newSignupRequest.Email,
            PhoneNumber = newSignupRequest.PhoneNumber
        };

        // cria um novo usuário usando o UserManager do ASP.NET Identity (validação, hash de senha, INSERT)
        IdentityResult result = await userManager.CreateAsync(customer, newSignupRequest.Password);

        foreach (IdentityError error in result.Errors)
            ModelState.AddModelError("", error.Description);

        if (!result.Succeeded)
            return View("Index", newSignupRequest);

        return RedirectToAction("Index", "Pages");
    }
}
