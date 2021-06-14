using System;
using Microsoft.AspNetCore.Identity;

namespace Authenticate {
  public class User : IdentityUser {
    public DateTime Created { get; set; }
    public DateTime LastLogin { get; set; }
    public bool Blocked { get; set; }
    public string Name { get; set; }
  }
}