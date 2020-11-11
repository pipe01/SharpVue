using HtmlAgilityPack;
using SharpCompress.Readers.Tar;
using SharpVue.Generator.Json;
using SharpVue.Loading;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace SharpVue.Generator.Vue
{
    public class VueGenerator : IGenerator
    {
        public readonly struct Options
        {
            public bool InjectReload { get; }
            public bool SingleFile { get; }
            public string OutputFolder { get; }
            public IJsonGenerator JsonGenerator { get; }

            public Options(bool injectReload, string outputFolder, bool singleFile, IJsonGenerator? jsonGenerator = null)
            {
                this.InjectReload = injectReload;
                this.OutputFolder = outputFolder;
                this.JsonGenerator = jsonGenerator ?? Json.JsonGenerator.Instance;
                this.SingleFile = singleFile;
            }
        }

        private const string RegularArchiveName = "SharpVue.Generator.Vue.Static.Regular.tar.gz";
        private const string SingleFileName = "SharpVue.Generator.Vue.Static.SingleFile.html";

        public void Generate(Workspace ws)
        {
            var outFolder = ws.PrepareOutputFolder();
            var opts = new Options(false, outFolder, ws.Config.SingleFile);

            Generate(ws, opts);
        }

        public void Generate(Workspace ws, Options opts)
        {
            if (opts.SingleFile)
                GenerateSingleFile(ws, opts);
            else
                GenerateRegular(ws, opts);
        }

        private static void GenerateRegular(Workspace ws, Options opts)
        {
            using var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(RegularArchiveName);
            Debug.Assert(resource != null);

            using var reader = TarReader.Open(resource);

            while (reader.MoveToNextEntry())
            {
                if (reader.Entry.IsDirectory)
                    continue;

                var path = Path.Combine(opts.OutputFolder, reader.Entry.Key);
                Directory.CreateDirectory(Path.GetDirectoryName(path));

                using var outFile = File.OpenWrite(path);
                using var entry = reader.OpenEntryStream();

                if (reader.Entry.Key == "index.html")
                {
                    ProcessHtmlEntrypoint(entry, outFile, opts, false);
                }
                else
                {
                    entry.CopyTo(outFile);
                }
            }

            using var dataOut = File.OpenWrite(Path.Combine(opts.OutputFolder, "js/data.js"));
            opts.JsonGenerator.Generate(ws, dataOut);
        }

        private static void GenerateSingleFile(Workspace ws, Options opts)
        {
            using var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(SingleFileName);
            Debug.Assert(resource != null);

            using var htmlOut = File.OpenWrite(Path.Combine(opts.OutputFolder, "index.html"));

            string json;

            using (var jsonMem = new MemoryStream())
            using (var reader = new StreamReader(jsonMem))
            {
                opts.JsonGenerator.Generate(ws, jsonMem);

                jsonMem.Position = 0;

                json = reader.ReadToEnd();
            }

            ProcessHtmlEntrypoint(resource, htmlOut, opts, true, json);
        }

        private static void ProcessHtmlEntrypoint(Stream inHtml, Stream outHtml, Options opts, bool singleFile, string? jsData = null)
        {
            var doc = new HtmlDocument();
            doc.Load(inHtml);

            var head = doc.DocumentNode.SelectSingleNode("/html/head");

            if (singleFile)
                head.SelectSingleNode("link[@rel='preload']").Remove();

            {
                var script = doc.CreateElement("script");
                head.AppendChild(script);

                if (singleFile)
                    script.InnerHtml = jsData;
                else
                    script.SetAttributeValue("src", "/js/data.js");
            }

            if (opts.InjectReload)
            {
                var script = doc.CreateElement("script");
                head.AppendChild(script);
                script.InnerHtml = "window.reload=true";
            }

            doc.Save(outHtml);
        }
    }
}
