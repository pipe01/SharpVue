using SharpVue.Common;
using SharpVue.Common.Documentation;
using SharpVue.Loading;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpVue.Generator.Json
{
    public class TypeJson
    {
        public string? FullName { get; set; }
        public string? Name { get; set; }
        public Content? Summary { get; set; }
        public Content? Remarks { get; set; }
        public string? Kind { get; set; }
        public string? Namespace { get; set; }
        public string? Assembly { get; set; }
        public List<string>? Inherits { get; set; }
        public List<string>? Implements { get; set; }
        public List<Property> Properties { get; set; } = new List<Property>();

        public static TypeJson FromType(Type type, Workspace ws)
        {
            var json = new TypeJson
            {
                FullName = type.FullName,
                Name = type.GetPrettyName(),
                Namespace = type.Namespace,
                Assembly = type.Assembly.GetName().Name + ".dll",
                Inherits = new List<string>(type.GetBaseTypes()),
                Implements = new List<string>(type.GetInterfaces().Select(o => o.FullName!)),
                Kind = type.IsClass ? "class" :
                        type.IsEnum ? "enum" :
                        type.IsValueType ? "struct" :
                        type.IsInterface ? "interface" : "type"
            };

            if (ws.ReferenceData.TryGetValue(type.GetKey(), out var data))
            {
                json.Summary = data.Summary;
                json.Remarks = data.Remarks;
            }

            foreach (var prop in type.GetProperties())
            {
                json.Properties.Add(Property.FromProperty(prop, type, ws));
            }
            json.Properties.Sort((a, b) => a.Name!.CompareTo(b.Name));

            return json;
        }
    }
}
