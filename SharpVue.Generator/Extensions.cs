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
            using var _ = StringBuilderPool.Rent(out var name);

            Write(type, name);

            return name.ToString();

            static void Write(Type type, StringBuilder str)
            {
                if (type.IsNested)
                {
                    str.Append(type.DeclaringType!.GetPrettyName());
                    str.Append('+');
                }

                if (type.ContainsGenericParameters)
                {
                    var backtick = type.Name.IndexOf('`');

                    if (backtick == -1)
                    {
                        // Edge case: when a generic type has a nested regular type, the inner type will be considered generic,
                        // even if it doesn't have any generic parameters itself.
                        // TODO Handle better
                        return;
                    }

                    str.Append(type.Name, 0, backtick);
                    str.Append("<");

                    var args = type.GetGenericArguments();
                    str.AppendNames(args);

                    str.Append(">");
                }
                else
                {
                    str.Append(type.Name);
                }
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

        public static Content GenerateNameContent(this Type type, bool plainText = false)
        {
            var c = new Content();
            type.GenerateNameContent(c, plainText);
            return c;
        }

        public static void GenerateNameContent(this Type type, Content c, bool plainText = false)
        {
            AddInsertions(type, c, plainText);

            static void AddInsertions(Type type, Content c, bool plainText)
            {
                if (type.IsGenericType)
                {
                    string name = type.Name;

                    c.Add(plainText ? InsertionType.PlainText : InsertionType.ReferenceType, $"{type.Namespace}.{type.Name}", type.GetGenericTypeDefinition().FullName);
                    c.AddPlainText("<");

                    var args = type.GetGenericArguments();
                    for (int i = 0; i < args.Length; i++)
                    {
                        var arg = args[i];

                        AddInsertions(arg, c, plainText);

                        if (i < args.Length - 1)
                            c.AddPlainText(", ");
                    }

                    c.AddPlainText(">");
                }
                else if (type.IsArray)
                {
                    AddInsertions(type.GetElementType()!, c, plainText);
                    c.AddPlainText("[]");
                }
                else if (type.IsPointer)
                {
                    AddInsertions(type.GetElementType()!, c, plainText);
                    c.AddPlainText("*");
                }
                else if (type.IsByRef)
                {
                    AddInsertions(type.GetElementType()!, c, plainText);
                    c.AddPlainText("&");
                }
                else if (type.IsGenericParameter)
                {
                    c.AddPlainText(type.Name);
                }
                else
                {
                    if (plainText)
                        c.AddPlainText(type.FullName ?? type.Name);
                    else
                        c.AddReferenceType(type);
                }
            }
        }
    }
}
