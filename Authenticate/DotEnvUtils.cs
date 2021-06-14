using System;
using System.IO;
using DotNetEnv;

namespace Authenticate {
  public static class DotEnvUtils {
    public static void ExecuteInDevelopment(Action action) {
#if DEBUG
      action();
#endif
    }
    
    public static void InjectDotEnvVars() {
      ExecuteInDevelopment(() => {
        while (Directory.GetFiles(Directory.GetCurrentDirectory(), "*.csproj").Length == 0) {
          Directory.SetCurrentDirectory("../");
        }

        Env.Load();
      });
    }
  }
}
