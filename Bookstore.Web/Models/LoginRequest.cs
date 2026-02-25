using System.ComponentModel.DataAnnotations;

namespace Bookstore.Web.Models;

public record LoginRequest(
    [EmailAddress]
    string Email,
    string Password);
