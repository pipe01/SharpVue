using SharpVue.Common;
using SharpVue.Common.Documentation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpVue.Generator
{
    public static class Extensions
    {
        public static IEnumerable<string> GetBaseTypes(this Type type)
        {
            Type? t = type;

            do
            {
                t = t!.BaseType;

                if (t != null)
                    yield return t.FullName!;

            } while (t != null);
        }

        public static string GetPrettyName(this Type type)
        {
            if (type.ContainsGenericParameters)
            {
                var backtick = type.Name.IndexOf('`');

                using var _ = StringBuilderPool.Rent(out var name);
                name.Append(type.Name, 0, backtick);
                name.Append("<");

                var args = type.GetGenericArguments();
                name.AppendNames(args);

                name.Append(">");

                return name.ToString();
            }
            else
            {
                return type.Name;
            }
        }

        public static void AppendNames(this StringBuilder builder, Type[] types)
        {
            for (int i = 0; i < types.Length; i++)
            {
                builder.Append(types[i].Name);

                if (i < types.Length - 1)
                    builder.Append(", ");
            }
        }

        public static Content GenerateNameContent(this Type type)
        {
            var c = new Content();
            type.GenerateNameContent(c);
            return c;
        }

        public static void GenerateNameContent(this Type type, Content c)
        {
            AddInsertions(type, c);

            static void AddInsertions(Type type, Content c)
            {
                if (type.IsGenericType)
                {
                    string name = type.Name;

                    c.Add(InsertionType.ReferenceType, $"{type.Namespace}.{type.Name}", type.GetGenericTypeDefinition().FullName);
                    c.AddPlainText("<");

                    var args = type.GetGenericArguments();
                    for (int i = 0; i < args.Length; i++)
                    {
                        var arg = args[i];

                        AddInsertions(arg, c);

                        if (i < args.Length - 1)
                            c.AddPlainText(", ");
                    }

                    c.AddPlainText(">");
                }
                else if (type.IsArray)
                {
                    AddInsertions(type.GetElementType()!, c);
                    c.AddPlainText("[]");
                }
                else if (type.IsPointer)
                {
                    AddInsertions(type.GetElementType()!, c);
                    c.AddPlainText("*");
                }
                else if (type.IsByRef)
                {
                    AddInsertions(type.GetElementType()!, c);
                    c.AddPlainText("&");
                }
                else if (type.IsGenericParameter)
                {
                    c.AddPlainText(type.Name);
                }
                else
                {
                    c.AddReferenceType(type);
                }
            }
        }
    }
}
