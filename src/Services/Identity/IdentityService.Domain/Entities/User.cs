using Microsoft.AspNetCore.Identity;

namespace IdentityService.Domain.Entities;

public class User : IdentityUser
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