using Microsoft.AspNetCore.Authentication;

namespace Authenticate {
  public class AuthProvider {
    public static AuthProvider FromAuthenticationProvider(AuthenticationScheme scheme) {
      return new AuthProvider {
        Name = scheme.Name,
        DisplayName = scheme.DisplayName
      };
    }
    
    public string Name { get; set; }
    public string? DisplayName { get; set; }
  }
}