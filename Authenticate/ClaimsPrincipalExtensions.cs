using System.Linq;
using System.Security.Claims;

namespace Authenticate {
  public static class ClaimsPrincipalExtensions {
    public static string GetUserId(this ClaimsPrincipal claimsPrincipal)
      => claimsPrincipal.Claims.First().Value;
  }
}