using SharpVue.Loading;
using SharpVue.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace SharpVue.Generator.Json
{
    public class JsonGenerator : IGenerator
    {
        public void Generate(Workspace ws)
        {
            Logger.Verbose("Generating JSON output");

            string outFolder = Path.Combine(ws.BaseFolder, ws.Config.OutFolder);

            if (Directory.Exists(outFolder))
                Directory.Delete(outFolder, true);
            Directory.CreateDirectory(outFolder);

            using var outFile = File.OpenWrite(Path.Combine(outFolder, "data.json"));

            var json = new JsonData();

            foreach (var group in ws.AssemblyLoader.ReferenceTypes.GroupBy(o => o.Namespace).OrderBy(o => o.Key))
            {
                Logger.Debug("Writing namespace {0}", group.Key);

                var ns = new Namespace
                {
                    FullName = group.Key ?? "<Root>"
                };

                foreach (var type in group)
                {
                    Logger.Debug("Writing type {0}", type.FullName);

                    var jsonType = TypeJson.FromType(type, ws);

                    ns.Types.Add(jsonType);
                }
                ns.Types.Sort((a, b) => a.Name!.CompareTo(b.Name));

                json.Namespaces.Add(ns);
            }

            JsonSerializer.Serialize(new Utf8JsonWriter(outFile), json, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                IgnoreNullValues = true,
#if DEBUG
                WriteIndented = true,
#endif
            });
        }
    }
}
