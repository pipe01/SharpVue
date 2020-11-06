using SharpVue.Common;
using SharpVue.Common.Documentation;
using SharpVue.Loading;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace SharpVue.Generator
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
            using var writer = new Utf8JsonWriter(outFile, new JsonWriterOptions { Indented = true });

            writer.WriteStartObject();
            {
                writer.WritePropertyName("reference");

                writer.WriteStartArray();
                foreach (var type in ws.ReferenceTypes)
                {
                    WriteReferenceType(writer, type, ws);
                }
                writer.WriteEndArray();
            }
            writer.WriteEndObject();
        }

        private static void WriteReferenceType(Utf8JsonWriter writer, Type type, Workspace ws)
        {
            writer.WriteStartObject();
            {
                writer.WriteString("fullName", type.FullName);

                if (ws.ReferenceData.TryGetValue(type.GetKey(), out var data))
                    WriteMemberData(writer, data);

                WriteChildren();
            }
            writer.WriteEndObject();

            void WriteChildren()
            {
                WriteProperties(type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static));
            }

            void WriteProperties(IEnumerable<PropertyInfo> props)
            {
                writer.WritePropertyName("properties");
                writer.WriteStartArray();

                foreach (var prop in props)
                {
                    writer.WriteStartObject();
                    {
                        writer.WriteString("name", prop.Name);
                        writer.WriteString("returntype", prop.PropertyType.FullName);
                        writer.WriteBoolean("getter", prop.GetGetMethod() != null);
                        writer.WriteBoolean("setter", prop.GetSetMethod() != null);

                        if (ws.ReferenceData.TryGetValue(prop.GetKey(), out var data))
                            WriteMemberData(writer, data);
                    }
                    writer.WriteEndObject();
                }

                writer.WriteEndArray();
            }
        }

        private static void WriteMemberData(Utf8JsonWriter writer, MemberData data)
        {
            writer.WriteString("summary", data.Summary);
            writer.WriteString("remarks", data.Remarks);
            writer.WriteString("returns", data.Returns);

            if (data.Parameters.Count > 0)
            {
                writer.WritePropertyName("parameters");
                writer.WriteStartObject();
                foreach (var param in data.Parameters)
                {
                    writer.WriteString(param.Key, param.Value);
                }
                writer.WriteEndObject();
            }

            if (data.TypeParameters.Count > 0)
            {
                writer.WritePropertyName("typeparams");
                writer.WriteStartObject();
                foreach (var param in data.TypeParameters)
                {
                    writer.WriteString(param.Key, param.Value);
                }
                writer.WriteEndObject();
            }

            if (data.Exceptions.Count > 0)
            {
                writer.WritePropertyName("exceptions");
                writer.WriteStartArray();
                foreach (var param in data.Exceptions)
                {
                    writer.WriteStartObject();
                    writer.WriteString("type", param.Key.FullName);
                    writer.WriteString("when", param.Value);
                    writer.WriteEndObject();
                }
                writer.WriteEndArray();
            }

            if (data.SeeAlso.Count > 0)
            {
                writer.WritePropertyName("seealso");
                writer.WriteStartArray();
                foreach (var item in data.SeeAlso)
                {
                    writer.WriteStringValue(item.XmlKey);
                }
                writer.WriteEndArray();
            }
        }
    }
}
