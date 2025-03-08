using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Data;
using Ordering.Domain.Models;

namespace Ordering.Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext()
    {
        
    }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options) { }
    
    public DbSet<Customer> Customers => Set<Customer>();
    
    public DbSet<Order> Orders => Set<Order>();
    
    public DbSet<Product> Products => Set<Product>();
    
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder buider)
    {
        buider.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(buider);
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost;Database=OrderDb;User Id=sa;Password=SwN12345678;Encrypt=False;TrustServerCertificate=True");

    }
}