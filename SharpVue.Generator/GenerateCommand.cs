using SharpVue.Generator.Json;
using SharpVue.Loading;
using System;
using System.IO;

namespace SharpVue.Generator
{
    public class GenerateCommand
    {
        public string? ConfigFile { get; set; }

        public void Execute()
        {
            ConfigFile ??= Path.GetFullPath(Workspace.ConfigName);

            if (!File.Exists(ConfigFile))
            {
                Console.Error.WriteLine($"Cannot find config file at \"{ConfigFile}\"");
                return;
            }

            using var ws = new Workspace(ConfigFile);
            var gen = new JsonGenerator();
            gen.Generate(ws);
        }
    }
}
