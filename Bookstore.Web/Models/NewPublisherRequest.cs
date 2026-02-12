using System.ComponentModel.DataAnnotations;

namespace Bookstore.Web.Models;

public record NewPublisherRequest(
    [Required]
    string Name,

    [Required]
    string City,

    [Required]
    string Country);
