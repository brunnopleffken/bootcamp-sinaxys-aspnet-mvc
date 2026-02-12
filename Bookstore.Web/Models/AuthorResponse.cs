using Bookstore.Web.Entities;

namespace Bookstore.Web.Models;

public record AuthorResponse(
    int Id,
    string FullName,
    string Bio,
    List<Book> Books
);
