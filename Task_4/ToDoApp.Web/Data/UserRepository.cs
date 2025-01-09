using ToDo.Web.DbContexts;
using ToDo.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace ToDo.Web.Data;

public class UserRepository : IUserRepository
{
    private readonly DBToDoContext _context;

    public UserRepository(DBToDoContext context)
    {
        _context = context;
    }

    public async Task<UserProfile> CreateAsync(UserProfile userProfile)
    {
        _context.UserProfile.Add(userProfile);
        await _context.SaveChangesAsync();

        return userProfile;
    }

    public async Task<UserProfile> GetByEmailAsync(string email)
    {
        return await _context.UserProfile.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<UserProfile> GetByIdAsync(int id)
    {
        return await _context.UserProfile.FirstOrDefaultAsync(u => u.Id == id);
    }
}
