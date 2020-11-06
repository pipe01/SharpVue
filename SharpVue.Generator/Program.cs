using SharpVue.Logging;
using Yaclip;

namespace SharpVue.Generator
{
    class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            Logger.Level = LogLevel.Debug;
#endif

            var app = YaclipApp.New()
                .Name("SharpVue Generator")
                .Command<GenerateCommand>("gen", o => o
                    .Callback(c => c.Execute())
                    .Argument(o => o.ConfigFile, o => o
                        .Name("configFile")));

            app.Build().Run(args);
        }
    }
}
