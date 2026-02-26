using System.Security.Claims;
using Bookstore.Web.Entities;
using Bookstore.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Web.Controllers;

public class LoginController(UserManager<Customer> userManager) : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Authenticate(LoginRequest loginRequest)
    {
        if (!ModelState.IsValid)
            return View("Index", loginRequest);

        Customer? customer = await userManager.FindByEmailAsync(loginRequest.Email);

        if (customer is null || !await userManager.CheckPasswordAsync(customer, loginRequest.Password))
        {
            // Não encontrou usuário com o e-mail especificado || senha não bate
            ModelState.AddModelError("", "Email ou senha inválidos");
            return View("Index", loginRequest);
        }

        // Crio uma lista de claims (declarações) que representam as informações do usuário autenticado
        List<Claim> claims =
        [
            new Claim(ClaimTypes.Name, customer.FullName),
            new Claim(ClaimTypes.NameIdentifier, customer.Id.ToString()),
            new Claim(ClaimTypes.Role, customer.Role) // Author, Reader, Publisher
        ];

        // Crio uma identidade ("crachá") com as claims informadas
        ClaimsIdentity identity = new (claims, CookieAuthenticationDefaults.AuthenticationScheme);
        ClaimsPrincipal principal = new (identity);

        // Usa o HttpContext para criar um novo cookie no navegador do maluco
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return RedirectToAction("Index", "Pages");
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("Index", "Pages");
    }
}
