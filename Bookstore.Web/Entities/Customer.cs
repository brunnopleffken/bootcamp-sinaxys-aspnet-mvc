namespace Bookstore.Web.Entities;

public class Customer
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; }
    public string PhoneNumber { get; set; }

    public ICollection<Order> Orders { get; set; }
}
