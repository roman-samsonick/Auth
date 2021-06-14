using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Authenticate {
  public class Program {
    public static void Main(string[] args) {
      DotEnvUtils.InjectDotEnvVars();
      CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
      .ConfigureWebHostDefaults(
        webBuilder => webBuilder.UseStartup<Startup>()
          .UseUrls($"http://*:{int.Parse(Environment.GetEnvironmentVariable("PORT"))}")
      );
  }
}