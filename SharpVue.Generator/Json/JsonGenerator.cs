using SharpVue.Common;
using SharpVue.Common.Documentation;
using SharpVue.Loading;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

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

            var allNamespaces = ws.ReferenceTypes.Select(o => o.Namespace).Distinct();
            var rootNamespace = new Namespace();

            foreach (var type in ws.ReferenceTypes)
            {
                if (type.Namespace == null)
                    continue;

                var parts = type.Namespace.Split('.');
                var parent = rootNamespace;

                for (int i = 0; i < parts.Length; i++)
                {
                    var part = parts[i];

                    if (!parent.Children.TryGetValue(part, out var ns))
                    {
                        parent = parent.Children[part] = new Namespace
                        {
                            FullName = parent.FullName == null ? part : parent.FullName + "." + part
                        };
                    }
                    else
                    {
                        parent = ns;
                    }
                }

                parent.Types.Add(new TypeJson
                {
                    FullName = type.FullName,
                    Name = type.Name,
                    Namespace = type.Namespace,
                });
            }

            JsonSerializer.Serialize(new Utf8JsonWriter(outFile), rootNamespace);
        }
    }
}
