using System.Security.Claims;
using System.Transactions;
using Bookstore.Web.Data;
using Bookstore.Web.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Web.Controllers;

[Authorize]
public class PurchasesController(ApplicationDbContext context, ILogger<PurchasesController> logger) : Controller
{
    [HttpGet("Purchases/{id:int}")]
    public async Task<IActionResult> Create(int id)
    {
        Book? book = await context.Books.FindAsync(id);

        if (book is null)
            return NotFound();

        int quantity = 1;
        int customerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        // Inicia BEGIN TRANSACTION
        using (var transaction = await context.Database.BeginTransactionAsync())
        {
            Order order = new Order()
            {
                PaymentMethod = PaymentMethod.Pix,
                TotalAmount = book.Price * quantity,
                Status = Status.Pending,
                CustomerId = customerId
            };

            context.Orders.Add(order);
            await context.SaveChangesAsync();

            OrderItem orderItem = new OrderItem()
            {
                BookId = book.Id,
                OrderId = order.Id,
                Quantity = quantity,
                Price = book.Price * quantity
            };

            context.OrderItems.Add(orderItem);
            await context.SaveChangesAsync();

            // Faz COMMIT ou ROLLBACK
            await transaction.CommitAsync();
        }

        return RedirectToAction("Success");
    }

    public IActionResult Success()
    {
        return View();
    }
}
