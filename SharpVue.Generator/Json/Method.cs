using SharpVue.Common;
using SharpVue.Common.Documentation;
using SharpVue.Loading;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SharpVue.Generator.Json
{
    public class Method : Returner
    {
        public string? Name { get; set; }
        public string? InheritedFrom { get; set; }
        public List<Parameter> Parameters { get; set; } = new List<Parameter>();

        public static Method FromMethod(MethodInfo method, Type declaringType, Workspace ws)
        {
            var data = ws.ReferenceData.TryGetValue(method.GetKey(), out var d) ? d : null;

            var json = new Method
            {
                Name = method.Name,
                InheritedFrom = method.DeclaringType != declaringType ? method.DeclaringType?.FullName : null,
                ReturnType = method.ReturnType.GenerateNameContent(),
                Returns = data?.Returns,
                Summary = data?.Summary,
                Remarks = data?.Remarks,
            };

            foreach (var param in method.GetParameters())
            {
                Content? content = null;
                data?.Parameters.TryGetValue(param.Name!, out content);

                json.Parameters.Add(Parameter.FromParameter(param, content));
            }

            return json;
        }
    }

    public class Parameter
    {
        public string? Name { get; set; }
        public Content? Type { get; set; }
        public Content? Description { get; set; }

        public static Parameter FromParameter(ParameterInfo param, Content? desc)
        {
            return new Parameter
            {
                Name = param.Name,
                Type = param.ParameterType.GenerateNameContent(),
                Description = desc
            };
        }
    }
}
