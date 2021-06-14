using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SpaServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace Authenticate {
  public class Startup {
    public void ConfigureServices(IServiceCollection services) => services
      .AddControllers()
      .AddNewtonsoftJson(ConfigureJson).Services
      .AddCors()
      .AddTransient<JwtService>()
      .AddJwtAuthentication()
      .AddDbContext<ApplicationContext>()
      .AddIdentity<User, IdentityRole>()
      .AddEntityFrameworkStores<ApplicationContext>()
      .Services
      .AddTransient<IAuthorizationHandler, BlockedUserPolicyAuthorizationHandler>()
      .AddSpaStaticFiles(options => options.RootPath = SpaRootPath);


    public void Configure(IApplicationBuilder app, IServiceProvider provider, IHostEnvironment env) {
      DotEnvUtils.ExecuteInDevelopment(() => {
        app.UseDeveloperExceptionPage();
        // var appContext = provider.GetRequiredService<ApplicationContext>();
        // appContext.Database.EnsureDeleted();
        // appContext.Database.EnsureCreated();
      });
      
      var appContext = provider.GetRequiredService<ApplicationContext>();
      appContext.Database.EnsureCreated();

      app.UseCorsOption()
        .UseRouting()
        .UseAuthentication()
        .UseAuthorization()
        .UseEndpoints(MapControllers);

      // app.UseSpa(UseSpa);
      app.UseStaticFiles();
      // .UseSpaStaticFiles();
    }

    private void MapControllers(IEndpointRouteBuilder endpointRouteBuilder) {
      endpointRouteBuilder.MapControllers();
      endpointRouteBuilder.MapFallbackToFile("index.html");
    }

    private static string SpaRootPath => "../AuthenticateFrontend/authenticate";

    private void ConfigureJson(MvcNewtonsoftJsonOptions options) {
      options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

      DotEnvUtils.ExecuteInDevelopment(() => {
        options.SerializerSettings.Formatting = Formatting.Indented;
      });
    }

    private void UseSpa(ISpaBuilder spa) {
      spa.Options.SourcePath = SpaRootPath;

      DotEnvUtils.ExecuteInDevelopment(() => spa.UseProxyToSpaDevelopmentServer("http://localhost:3000"));
    }
  }
}