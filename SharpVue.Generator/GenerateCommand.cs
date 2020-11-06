using SharpVue.Loading;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpVue.Generator
{
    public class GenerateCommand
    {
        public string? ConfigFile { get; set; }

        public void Execute()
        {
            if (ConfigFile == null)
                ConfigFile = Path.GetFullPath(Workspace.ConfigName);

            if (!File.Exists(ConfigFile))
            {
                Console.Error.WriteLine($"Cannot find config file at \"{ConfigFile}\"");
                return;
            }

            var ws = new Workspace(ConfigFile);
        }
    }
}
