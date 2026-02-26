using Microsoft.AspNetCore.Identity;

namespace Bookstore.Web.Entities;

public class Customer : IdentityUser<int>
{
    public string FullName { get; set; }
    public bool IsActive { get; set; }
    public string Role { get; set; }

    public ICollection<Order> Orders { get; set; }
}
