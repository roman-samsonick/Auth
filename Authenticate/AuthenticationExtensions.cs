using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Authenticate {
  public static class ServiceCollectionExtensions {
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services) =>
      services.AddAuthorization(options => {
        options.DefaultPolicy = AuthorizationPolicy.Combine(
          options.DefaultPolicy,
          new AuthorizationPolicy(
            new[] {new BlockedUserPolicyRequirement()},
            new[] {JwtBearerDefaults.AuthenticationScheme}
          )
        );
      }).AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = EnvironmentConstants.TokenValidationParameters;
      }).Services;
  }
}