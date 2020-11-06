using SharpVue.Loading;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpVue.Generator
{
    public class HtmlGenerator : IGenerator
    {
        public void Generate(Workspace ws)
        {
            string outFolder = ws.BaseFolder;

            if (Directory.Exists(outFolder))
                Directory.Delete(outFolder, true);
            Directory.CreateDirectory(outFolder);
        }
    }
}
