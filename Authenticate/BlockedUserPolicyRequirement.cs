using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Authenticate {
  public class BlockedUserPolicyRequirement : IAuthorizationRequirement {
    public async Task<bool> Pass(SignInManager<User> signInManager, ClaimsPrincipal userClaimsPrincipal) {
      var user = await signInManager.UserManager.FindByIdAsync(
        userClaimsPrincipal.GetUserId()
      );

      return !(user?.Blocked ?? true);
    }
  }
}