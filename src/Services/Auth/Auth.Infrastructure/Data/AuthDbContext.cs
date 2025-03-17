using System.Reflection;
using Auth.Application.Data;
using Auth.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Data;

public class AuthDbContext: IdentityDbContext<User>, IAuthDbContext
{
    public AuthDbContext()
    {
        
    }
    
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost;Database=AuthDb;User Id=sa;Password=SwN12345678;Encrypt=False;TrustServerCertificate=True");

    } 


    public DbSet<User> ApplicationUsers => Users ;
}