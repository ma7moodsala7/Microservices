using Common.Persistence;
using IdentityService.Domain.Entities;
using IdentityService.Domain.Interfaces;
using IdentityService.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Persistence.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    private new readonly IdentityDbContext _context;

    public UserRepository(IdentityDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Set<User>()
            .FirstOrDefaultAsync(u => u.Username == username && !u.IsDeleted);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Set<User>()
            .FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);
    }

    public async Task<bool> IsUsernameUniqueAsync(string username)
    {
        return !await _context.Set<User>()
            .AnyAsync(u => u.Username == username && !u.IsDeleted);
    }

    public async Task<bool> IsEmailUniqueAsync(string email)
    {
        return !await _context.Set<User>()
            .AnyAsync(u => u.Email == email && !u.IsDeleted);
    }
}
