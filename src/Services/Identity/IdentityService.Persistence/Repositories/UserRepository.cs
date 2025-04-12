using IdentityService.Domain.Entities;
using IdentityService.Domain.Interfaces;
using IdentityService.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Shared.Persistence;

namespace IdentityService.Persistence.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    private new readonly IdentityDbContext _context;

    public UserRepository(IdentityDbContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }

    public override async Task<IReadOnlyList<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public override async Task<User> AddAsync(User entity)
    {
        await _context.Users.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public override async Task UpdateAsync(User entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public override async Task DeleteAsync(User entity)
    {
        _context.Users.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public override async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Users.AnyAsync(e => e.Id == id);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.UserName == username);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> IsUsernameUniqueAsync(string username)
    {
        return !await _context.Users
            .AnyAsync(u => u.UserName == username);
    }

    public async Task<bool> IsEmailUniqueAsync(string email)
    {
        return !await _context.Users
            .AnyAsync(u => u.Email == email);
    }
}
