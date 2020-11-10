using HtmlAgilityPack;
using SharpCompress.Archives;
using SharpCompress.Archives.GZip;
using SharpCompress.Archives.Tar;
using SharpCompress.Common;
using SharpCompress.Compressors;
using SharpCompress.Compressors.Deflate;
using SharpCompress.Readers;
using SharpCompress.Readers.Tar;
using SharpVue.Generator.Json;
using SharpVue.Loading;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace SharpVue.Generator.Vue
{
    public class VueGenerator : IGenerator
    {
        private const string RegularArchiveName = "SharpVue.Generator.Vue.Static.Regular.tar.gz";
        private const string SingleFileName = "SharpVue.Generator.Vue.Static.SingleFile.index.html";

        public void Generate(Workspace ws)
        {
            var outFolder = ws.PrepareOutputFolder();

            var jsonGen = new JsonGenerator();

            if (ws.Config.SingleFile)
                GenerateSingleFile(ws, outFolder, jsonGen);
            else
                GenerateRegular(ws, outFolder, jsonGen);
        }

        private static void GenerateRegular(Workspace ws, string outFolder, IJsonGenerator jsonGen)
        {
            using var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(RegularArchiveName);
            Debug.Assert(resource != null);

            using var reader = TarReader.Open(resource);

            while (reader.MoveToNextEntry())
            {
                if (reader.Entry.IsDirectory)
                    continue;

                var path = Path.Combine(outFolder, reader.Entry.Key);
                Directory.CreateDirectory(Path.GetDirectoryName(path));

                using var outFile = File.OpenWrite(path);
                using var entry = reader.OpenEntryStream();

                if (reader.Entry.Key == "index.html")
                {
                    ProcessHtmlEntrypoint(entry, outFile, false);
                }
                else
                {
                    entry.CopyTo(outFile);
                }
            }

            using var dataOut = File.OpenWrite(Path.Combine(outFolder, "js/data.js"));
            jsonGen.Generate(ws, dataOut);
        }

        private static void GenerateSingleFile(Workspace ws, string outFolder, IJsonGenerator jsonGen)
        {
            using var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(SingleFileName);
            Debug.Assert(resource != null);

            using var htmlOut = File.OpenWrite(Path.Combine(outFolder, "index.html"));

            string json;

            using (var jsonMem = new MemoryStream())
            using (var reader = new StreamReader(jsonMem))
            {
                jsonGen.Generate(ws, jsonMem);

                jsonMem.Position = 0;

                json = reader.ReadToEnd();
            }

            ProcessHtmlEntrypoint(resource, htmlOut, true, json);
        }

        private static void ProcessHtmlEntrypoint(Stream inHtml, Stream outHtml, bool singleFile, string? jsData = null)
        {
            var doc = new HtmlDocument();
            doc.Load(inHtml);

            var head = doc.DocumentNode.SelectSingleNode("/html/head");

            if (singleFile)
                head.SelectSingleNode("link[@rel='preload']").Remove();

            var script = doc.CreateElement("script");
            head.AppendChild(script);

            if (singleFile)
                script.InnerHtml = jsData;
            else
                script.SetAttributeValue("src", "/js/data.js");

            doc.Save(outHtml);
        }
    }
}
