using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Authenticate {
  public static class EnvironmentConstants {
    private static string Get(string name) => Environment.GetEnvironmentVariable(name);
    public static string Port => Get("PORT");
    public static string GitHubClientId => Get("GITHUB_CLIENT_ID");
    public static string GoogleClientId => Get("GOOGLE_CLIENT_ID");
    public static string GitHubEnterpriseDomain => Get("GITHUB_ENTERPRISE_DOMAIN");
    public static string GitHubClientSecret => Get("GITHUB_CLIENT_SECRET");
    public static string GoogleClientSecret => Get("GOOGLE_CLIENT_SECRET");
    public static string DataBaseUrl => Get("DATABASE_URL");
    public static string AuthIssuer => Get("AUTH_ISSUER");
    public static string AuthAudience => Get("AUTH_AUDIENCE");
    private static string AuthKey => Get("AUTH_KEY");
    public static string AuthLifetimeInHours => Get("AUTH_LIFETIME_IN_HOURS");

    public static TokenValidationParameters TokenValidationParameters => new TokenValidationParameters {
      ValidateIssuer = true,
      ValidateAudience = true,
      ValidateLifetime = true,
      ValidateIssuerSigningKey = true,
      ValidIssuer = AuthIssuer,
      ValidAudience = AuthAudience,
      IssuerSigningKey = AuthKey.ToSymmetricSecurityKey(),
      LifetimeValidator = LifetimeValidator
    };

    public static SigningCredentials SigningCredentials => new SigningCredentials(
      AuthKey.ToSymmetricSecurityKey(),
      SecurityAlgorithms.HmacSha256
    );

    private static SymmetricSecurityKey ToSymmetricSecurityKey(this string key)
      => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));

    private static bool LifetimeValidator(
      DateTime? notBefore,
      DateTime? expires,
      SecurityToken token,
      TokenValidationParameters parameters
    ) => expires > DateTime.Now;
  }
}
