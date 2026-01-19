using System.ComponentModel.DataAnnotations;

namespace Bookstore.Web.Entities;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Isbn { get; set; }
    public decimal Price { get; set; }
    public BookFormat Format { get; set; }
    public DateTime CreatedAt { get; set; } // criar campo com valor default (CURRENT_TIMESTAMP)

    public int CategoryId { get; set; }
    public Category Category { get; set; }

    public int PublisherId { get; set; }
    public Publisher Publisher { get; set; }

    public ICollection<Author> Authors { get; set; }
}

public enum BookFormat
{
    [Display(Name = "Capa dura")]
    Hardcover,
    [Display(Name = "Capa comum")]
    Paperback,
    [Display(Name = "Livro digital")]
    Ebook,
    [Display(Name = "Audiolivro")]
    Audiobook
}
