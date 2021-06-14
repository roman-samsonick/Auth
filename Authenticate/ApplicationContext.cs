using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Authenticate {
  public class ApplicationContext : IdentityDbContext<User>
  {
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
      : base(options) {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
      optionsBuilder.UseSqlite(EnvironmentConstants.DataBaseUrl);
      base.OnConfiguring(optionsBuilder);
    }
  }
}