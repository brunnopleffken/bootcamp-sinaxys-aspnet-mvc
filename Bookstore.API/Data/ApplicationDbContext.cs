using Bookstore.Web.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Web.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Author> Authors { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Publisher> Publishers { get; set; }

    // Configurações adicionais para as entidades usando Fluent API

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(e =>
        {
            e.Property(p => p.FullName).HasMaxLength(50);
        });

        modelBuilder.Entity<Book>(e =>
        {
            e.Property(p => p.Description).HasMaxLength(2000);
            e.Property(p => p.Title).HasMaxLength(100);
            e.Property(p => p.Isbn).HasMaxLength(13);
            e.Property(p => p.Price).HasPrecision(8, 2);
            e.Property(p => p.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Campo "price" deve ser maior que zero, exceto se o formato for eBook (2) ou Audiobook (3)
            e.ToTable(t => t.HasCheckConstraint("chk_price_non_zero_or_ebook", "price > 0 OR format IN (2,3)"));
        });

        modelBuilder.Entity<Customer>(e =>
        {
            e.Property(p => p.FullName).HasMaxLength(50);
            e.Property(p => p.Email).HasMaxLength(50);
            e.Property(p => p.IsActive).HasDefaultValue(true);
            e.Property(p => p.PhoneNumber).HasMaxLength(15);
        });

        modelBuilder.Entity<Order>(e =>
        {
            e.Property(p => p.ExternalId).HasDefaultValueSql("uuidv7()"); // PostreSQL 18+; Outros, usar: gen_random_uuid()
            e.Property(p => p.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Campo "total_amount" deve ser maior ou igual a zero
            e.ToTable(t => t.HasCheckConstraint("chk_total_amount_positive", "total_amount >= 0"));
        });

        modelBuilder.Entity<OrderItem>(e =>
        {
            // Campo "quantity" deve ser maior que zero
            e.ToTable(t => t.HasCheckConstraint("chk_quantity_non_zero", "quantity > 0"));
        });

        base.OnModelCreating(modelBuilder);
    }
}
