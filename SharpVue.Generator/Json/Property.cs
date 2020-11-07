using SharpVue.Common.Documentation;
using SharpVue.Loading;
using System;
using System.Reflection;

namespace SharpVue.Generator.Json
{
    public class Property : Returner
    {
        public bool Getter { get; set; }
        public bool Setter { get; set; }

        public Property(MemberData? data, string name, MemberInfo member, Type declaringType, Type returnType) : base(data, name, member, declaringType, returnType)
        {
        }

        public static Property FromProperty(PropertyInfo prop, Type declaringType, Workspace ws)
        {
            return new Property(ws.GetDataFor(prop), prop.Name, prop, declaringType, prop.PropertyType)
            {
                Getter = prop.GetGetMethod() != null,
                Setter = prop.GetSetMethod() != null,
            };
        }
    }
}
