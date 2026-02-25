using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Bookstore.Web.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Bookstore.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(UserManager<Customer> userManager) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Index(AuthRequest request)
    {
        // Busca o Customer no banco de dados — igual o MVC
        Customer? customer = await userManager.FindByEmailAsync(request.Email);

        // Busca o Customer no banco de dados — também igual o MVC
        if (customer is null || !await userManager.CheckPasswordAsync(customer, request.Password))
            return BadRequest();

        // Lista as claims do usuário — também idêntico ao MVC
        // Mas aqui vamos usar essas claims para criar um token JWT, em vez de um cookie
        List<Claim> claims =
        [
            new Claim(ClaimTypes.Name, customer.FullName),
            new Claim(ClaimTypes.NameIdentifier, customer.Id.ToString()),
            new Claim(ClaimTypes.Role, "Reader") // Author, Reader, Publisher
        ];

        // Cria a chave de segurança e as credenciais de assinatura para o token JWT
        // A chave de segurança aqui deve ser IDÊNTICA àquela configurada no Program.cs
        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("CH4V3-S3CR374-B3M-53GUR5-D1F1C1L"));
        SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Cria o token JWT com as claims, expiração, emissor, público-alvo e credenciais de assinatura
        JwtSecurityToken jwt = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            audience: "BookstoreApp",
            issuer: "BookstoreApp",
            signingCredentials: credentials
        );

        // Serializa o token JWT para uma string que pode ser enviada na resposta da API
        string token = new JwtSecurityTokenHandler().WriteToken(jwt);

        return Ok(new { Token = token });
    }
}

// DTO para receber os dados de autenticação (email e senha)
// Naturalmente, isso ficaria em um arquivo separado, mas para fins de simplicidade, deixamos aqui mesmo...
public record AuthRequest(string Email, string Password);
