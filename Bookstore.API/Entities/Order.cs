namespace Bookstore.Web.Entities;

public class Order
{
    public int Id { get; set; }
    public Guid ExternalId { get; set; } // deixar pro banco de dados gerar o UUID usando uuidv7()
    public PaymentMethod PaymentMethod { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; } // campo automático do banco de dados
    public Status Status { get; set; }

    public int CustomerId { get; set; }
    public Customer Customer { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; }
}

public enum PaymentMethod
{
    CreditCard,
    DebitCard,
    Pix,
    Boleto
}

public enum Status
{
    Pending,
    Paid,
    Shipped,
    Delivered,
    Canceled
}
