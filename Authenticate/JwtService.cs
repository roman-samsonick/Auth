using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Authenticate {
  public class JwtService {
    private DateTime JwtExpiredAt => DateTime.Now.Add(
      TimeSpan.FromHours(EnvironmentConstants.AuthLifetimeInHours.ParseInt())
    );

    public string CreateJwt(string accountId) {
      return new JwtSecurityTokenHandler().WriteToken(
        new JwtSecurityToken(
          issuer: EnvironmentConstants.AuthIssuer,
          audience: EnvironmentConstants.AuthAudience,
          notBefore: DateTime.Now,
          claims: CreateAccountClaims(accountId),
          expires: JwtExpiredAt,
          signingCredentials: EnvironmentConstants.SigningCredentials
        )
      );
    }

    private static IEnumerable<Claim> CreateAccountClaims(string accountId) => new [] {
      new Claim(
        ClaimsIdentity.DefaultNameClaimType,
        accountId
      )
    };
  }
}
