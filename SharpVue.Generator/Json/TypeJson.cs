﻿using SharpVue.Common;
using SharpVue.Common.Documentation;
using SharpVue.Loading;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpVue.Generator.Json
{
    public class TypeJson : Descriptable
    {
        public string? FullName { get; set; }
        public string? Kind { get; set; }
        public string? Namespace { get; set; }
        public string? Assembly { get; set; }
        public List<string>? Inherits { get; set; }
        public List<string>? Implements { get; set; }
        public List<Property> Properties { get; set; } = new List<Property>();
        public List<Method> Methods { get; set; } = new List<Method>();
        public List<Field> Fields { get; set; } = new List<Field>();

        public TypeJson(MemberData? data, string name) : base(data, name)
        {

        }

        public static TypeJson FromType(Type type, Workspace ws)
        {
            var json = new TypeJson(ws.GetDataFor(type), type.GetPrettyName())
            {
                FullName = type.FullName,
                Namespace = type.Namespace,
                Assembly = type.Assembly.GetName().Name + ".dll",
                Inherits = new List<string>(type.GetBaseTypes()),
                Implements = new List<string>(type.GetInterfaces().Where(o => o.IsPublic && o.FullName != null).Select(o => o.FullName!)),
                Kind = type.IsClass ? "class" :
                        type.IsEnum ? "enum" :
                        type.IsValueType ? "struct" :
                        type.IsInterface ? "interface" : "type"
            };

            foreach (var prop in type.GetProperties())
            {
                json.Properties.Add(Property.FromProperty(prop, type, ws));
            }
            json.Properties.Sort((a, b) => a.Name!.CompareTo(b.Name));

            foreach (var method in type.GetMethods())
            {
                if (method.IsSpecialName && (
                    method.Name.StartsWith("get_")
                    || method.Name.StartsWith("set_")
                    || method.Name.StartsWith("add_")
                    || method.Name.StartsWith("remove_")))
                {
                    continue;
                }

                json.Methods.Add(Method.FromMethod(method, type, ws));
            }
            json.Methods.Sort((a, b) => a.Name!.CompareTo(b.Name));

            foreach (var field in type.GetFields())
            {
                json.Fields.Add(Field.FromField(field, type, ws));
            }
            json.Fields.Sort((a, b) => a.Name!.CompareTo(b.Name));

            return json;
        }
    }
}
