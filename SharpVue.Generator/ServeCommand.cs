using SharpVue.Generator.Util;
using SharpVue.Generator.Vue;
using SharpVue.Loading;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharpVue.Generator
{
    public class ServeCommand
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
            var gen = new VueGenerator();

            string tempFolder = Path.Combine(Path.GetTempPath(), "sharpvue");
            if (Directory.Exists(tempFolder))
                Directory.Delete(tempFolder, true);
            Directory.CreateDirectory(tempFolder);

            ws.OutFolder = tempFolder;

            var http = new HttpServer(tempFolder, 8080);
            http.Start();

            gen.Generate(ws);

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
