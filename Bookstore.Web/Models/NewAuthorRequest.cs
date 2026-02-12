using System.ComponentModel.DataAnnotations;

namespace Bookstore.Web.Models;

public record NewAuthorRequest(
    [Required]
    [MaxLength(50)]
    string FullName,

    [Required]
    string Bio);
