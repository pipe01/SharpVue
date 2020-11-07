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

            var namespaces = new List<Namespace>();

            foreach (var group in ws.ReferenceTypes.GroupBy(o => o.Namespace).Where(o => o.Key != null).OrderBy(o => o.Key))
            {
                var ns = new Namespace
                {
                    FullName = group.Key
                };

                foreach (var type in group)
                {
                    ns.Types.Add(new TypeJson
                    {
                        FullName = type.FullName,
                        Name = type.Name,
                        Namespace = type.Namespace,
                        Assembly = type.Assembly.GetName().Name + ".dll",
                        Inherits = new List<string>(type.GetBaseTypes()),
                        Implements = new List<string>(type.GetInterfaces().Select(o => o.FullName!)),
                        Kind = type.IsClass ? "class" :
                               type.IsValueType ? "struct" :
                               type.IsEnum ? "enum" :
                               type.IsInterface ? "interface" : "type"
                    });
                }
                ns.Types.Sort((a, b) => a.Name!.CompareTo(b.Name));

                namespaces.Add(ns);
            }

            JsonSerializer.Serialize(new Utf8JsonWriter(outFile), namespaces);
        }
    }
}
