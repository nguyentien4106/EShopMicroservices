using Auth.Domain.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace Auth.Domain.Models;

public class User : IdentityUser, IEntity
{
    public string FirstName { get; set; } = default!;
    
    public string LastName { get; set; } = default!;
    
    public int Status { get; set; }
    
    public DateTime? CreatedAt { get; set; } = DateTime.Now;
    
    public string? CreatedBy { get; set; } = default!;
    
    public DateTime? LastModified { get; set; } = DateTime.Now;
    
    public string? LastModifiedBy { get; set; } = default!;
}