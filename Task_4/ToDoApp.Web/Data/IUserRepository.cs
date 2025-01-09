using ToDo.Web.Models;

namespace ToDo.Web.Data;

public interface IUserRepository
{
    Task<UserProfile> CreateAsync(UserProfile userProfile);
    Task<UserProfile> GetByEmailAsync(string email);
    Task<UserProfile> GetByIdAsync(int id);
}
