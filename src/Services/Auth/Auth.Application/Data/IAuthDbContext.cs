using Auth.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Auth.Application.Data;

public interface IAuthDbContext
{
    public DbSet<User> ApplicationUsers { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}