using SharpVue.Loading;
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
            string outFolder = Path.Combine(ws.BaseFolder, ws.Config.OutFolder);

            if (Directory.Exists(outFolder))
                Directory.Delete(outFolder, true);
            Directory.CreateDirectory(outFolder);

            using var outFile = File.OpenWrite(Path.Combine(outFolder, "data.json"));

            var namespaces = new List<Namespace>();

            foreach (var group in ws.ReferenceTypes.GroupBy(o => o.Namespace).Where(o => o.Key != null).OrderBy(o => o.Key))
            {
                var ns = new Namespace
                {
                    FullName = group.Key
                };

                foreach (var type in group)
                {
                    var jsonType = TypeJson.FromType(type, ws);

                    ns.Types.Add(jsonType);
                }
                ns.Types.Sort((a, b) => a.Name!.CompareTo(b.Name));

                namespaces.Add(ns);
            }

            JsonSerializer.Serialize(new Utf8JsonWriter(outFile), namespaces, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }
    }
}
