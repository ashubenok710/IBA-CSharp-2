using AuthenticationServer.Models;
using ToDo.Identity.Models;

namespace ToDo.Identity.Data;

public interface IUserRepository
{
    public Task<UserProfile> GetByEmailAsync(string email);
}
