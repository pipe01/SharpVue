using SharpVue.Common.Documentation;
using SharpVue.Loading;
using System.Reflection;
using SharpVue.Common;
using System;

namespace SharpVue.Generator.Json
{
    public class Property : Returner
    {
        public string? Name { get; set; }
        public bool Getter { get; set; }
        public bool Setter { get; set; }
        public string? InheritedFrom { get; set; }

        public static Property FromProperty(PropertyInfo prop, Type declaringType, Workspace ws)
        {
            var data = ws.ReferenceData.TryGetValue(prop.GetKey(), out var d) ? d : null;

            return new Property
            {
                Name = prop.Name,
                ReturnType = prop.PropertyType.GenerateNameContent(),
                Getter = prop.GetGetMethod() != null,
                Setter = prop.GetSetMethod() != null,
                InheritedFrom = prop.DeclaringType != declaringType ? prop.DeclaringType?.FullName : null,
                Summary = data?.Summary,
                Remarks = data?.Remarks,
                Returns = data?.Returns
            };
        }
    }
}
