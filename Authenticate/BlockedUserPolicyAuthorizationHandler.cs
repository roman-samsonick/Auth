using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Authenticate {
  public class BlockedUserPolicyAuthorizationHandler : AuthorizationHandler<BlockedUserPolicyRequirement> {
    private readonly SignInManager<User> _signInManager;

    public BlockedUserPolicyAuthorizationHandler(
      SignInManager<User> signInManager
    ) {
      _signInManager = signInManager;
    }
    

    protected override async Task HandleRequirementAsync(
      AuthorizationHandlerContext context,
      BlockedUserPolicyRequirement requirement
    ) {
      if (await requirement.Pass(_signInManager, context.User)) {
        context.Succeed(requirement);
      }
      else {
        context.Fail();
      }
    }
  }
}