﻿using SharpVue.Loading;
using SharpVue.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace SharpVue.Generator.Json
{
    public class JsonGenerator : IJsonGenerator
    {
        public void Generate(Workspace ws)
        {
            string outFolder = ws.PrepareOutputFolder();

            using var outFile = File.OpenWrite(Path.Combine(outFolder, "data.js"));

            Generate(ws, outFile);
        }

        public void Generate(Workspace ws, Stream jsonData)
        {
            Logger.Verbose("Generating JSON output");

            jsonData.Write(Encoding.UTF8.GetBytes("window.data="));

            var json = new JsonData
            {
                Config = ws.Config.Appearance,
                Articles = ws.ArticleLoader.Articles
            };

            AddNamespaces(json, ws);

            JsonSerializer.Serialize(new Utf8JsonWriter(jsonData), json, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                IgnoreNullValues = true,
#if DEBUG
                WriteIndented = true,
#endif
            });

            static void AddNamespaces(JsonData json, Workspace ws)
            {
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
            }
        }
    }
}
