using System.ComponentModel.DataAnnotations;
using Bookstore.Web.Entities;

namespace Bookstore.Web.Models;

public record NewBookRequest(
    [Required]
    [MaxLength(100)]
    string Title,

    [Required]
    [MaxLength(2000)]
    string Description,

    [Required]
    [MaxLength(13)]
    string Isbn,

    [Required]
    [Range(0.00, 999999.99)]
    decimal Price,

    [Required]
    BookFormat Format,

    [Required]
    int CategoryId,

    [Required]
    int PublisherId,

    [Required]
    List<int> AuthorIds);
