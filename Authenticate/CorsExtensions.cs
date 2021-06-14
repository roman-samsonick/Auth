using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace Authenticate {
  public static partial class ApplicationBuilderExtensions {
    private static void ConfigureCors(CorsPolicyBuilder builder) => builder
      .AllowAnyOrigin()
      .AllowAnyHeader()
      .AllowAnyMethod();

    public static IApplicationBuilder UseCorsOption(this IApplicationBuilder application)
      => application.UseCors(ConfigureCors);
  }
}