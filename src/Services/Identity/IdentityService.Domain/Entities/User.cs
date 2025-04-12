using Microsoft.AspNetCore.Identity;
using Shared.Domain;

namespace IdentityService.Domain.Entities;

public class User : IdentityUser<Guid>, IEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime? LastLoginAt { get; set; }

    public void UpdateProfile(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public void RecordLogin()
    {
        LastLoginAt = DateTime.UtcNow;
    }
}