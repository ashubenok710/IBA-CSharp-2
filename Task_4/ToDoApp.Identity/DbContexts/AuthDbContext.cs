using ToDo.Identity.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationServer.DbContexts;

public class AuthDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=.;Database=ToDo;Trusted_Connection=True;");
    }

    public DbSet<UserProfile> UserProfile { get; set; }
}
