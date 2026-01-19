using Microsoft.EntityFrameworkCore;

namespace Bookstore.Web.Entities;

[PrimaryKey(nameof(OrderId), nameof(BookId))]
public class OrderItem
{
    public int OrderId { get; set; }
    public Order Order { get; set; }

    public int BookId { get; set; }
    public Book Book { get; set; }

    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
