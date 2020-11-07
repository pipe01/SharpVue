using SharpVue.Common.Documentation;
using SharpVue.Loading;
using System;
using System.Reflection;

namespace SharpVue.Generator.Json
{
    public class Field : Returner
    {
        public bool ReadOnly { get; set; }

        public Field(MemberData? data, string name, MemberInfo member, Type declaringType, Type returnType) : base(data, name, member, declaringType, returnType)
        {
        }

        public static Field FromField(FieldInfo field, Type declaringType, Workspace ws)
        {
            return new Field(ws.GetDataFor(field), field.Name, field, declaringType, field.FieldType)
            {
                ReadOnly = field.IsInitOnly
            };
        }
    }
}
