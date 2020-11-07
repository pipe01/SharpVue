﻿using SharpVue.Common;
using SharpVue.Common.Documentation;
using SharpVue.Loading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SharpVue.Generator.Json
{
    public class Method : Returner
    {
        public string? Name { get; set; }
        /// <summary>
        /// Method name with generic parameter names included
        /// </summary>
        public Content? PrettyName { get; set; }
        public string? InheritedFrom { get; set; }
        public List<Parameter> Parameters { get; set; } = new List<Parameter>();

        public static Method FromMethod(MethodInfo method, Type declaringType, Workspace ws)
        {
            var data = ws.ReferenceData.TryGetValue(method.GetKey(), out var d) ? d : null;

            var json = new Method
            {
                Name = method.Name,
                PrettyName = BuildPrettyName(method),
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

        private static Content BuildPrettyName(MethodInfo method)
        {
            var c = new Content();

            c.AddPlainText(method.Name);

            if (method.ContainsGenericParameters)
            {
                c.AddPlainText("<" + string.Join(", ", method.GetGenericArguments().Select(o => o.Name)) + ">");
            }

            c.AddPlainText("(");

            var args = method.GetParameters();
            for (int i = 0; i < args.Length; i++)
            {
                args[i].ParameterType.GenerateNameContent(c);
                c.AddPlainText(" " + args[i].Name + (i < args.Length - 1 ? ", " : null));
            }

            c.AddPlainText(")");

            return c;
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
