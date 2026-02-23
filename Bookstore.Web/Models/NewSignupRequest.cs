using System.ComponentModel.DataAnnotations;

namespace Bookstore.Web.Models;

public record NewSignupRequest(
    [Required]
    string FullName,

    [Required]
    [EmailAddress]
    string Email,

    [Required]
    [MinLength(8)]
    string Password,

    [Required]
    string PhoneNumber);
