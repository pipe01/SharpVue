using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace SharpVue.Common
{
    // Based on https://github.com/ZacharyPatten/Towel/blob/master/Sources/Towel/Meta.cs
    public static class KeyHelper
    {
        private static readonly IDictionary<MemberInfo, string> KeyCache = new Dictionary<MemberInfo, string>();

        public static string GetKey(this MemberInfo member)
        {
            if (KeyCache.TryGetValue(member, out var key))
                return key;

            key = member switch
            {
                Type type => "T:" + XmlDocumentationKeyHelper(type.FullName, null),
                PropertyInfo _ => "P:" + XmlDocumentationKeyHelper(member.DeclaringType.FullName, member.Name),
                FieldInfo _ => "F:" + XmlDocumentationKeyHelper(member.DeclaringType.FullName, member.Name),
                EventInfo _ => "E:" + XmlDocumentationKeyHelper(member.DeclaringType.FullName, member.Name),

                MethodInfo method => method.GetKey(),
                ConstructorInfo ctor => ctor.GetKey(),

                _ => throw new ArgumentException("Cannot get key of " + member.GetType().Name),
            };

            KeyCache[member] = key;
            return key;
        }

        public static string GetKey(this MethodInfo methodInfo)
        {
            var typeGenericMap = methodInfo.DeclaringType.GetGenericArguments().ToTypeArgumentDictionary();
            var methodGenericMap = methodInfo.GetGenericArguments().ToTypeArgumentDictionary();

            var parameterInfos = methodInfo.GetParameters();

            using var _ = StringBuilderPool.Rent(out var key);
            key.Append("M:");
            key.Append(methodInfo.DeclaringType.GetXmlName(false, typeGenericMap, methodGenericMap));
            key.Append(".");
            key.Append(methodInfo.Name);

            if (methodGenericMap.Count > 0)
                key.Append("``").Append(methodGenericMap.Count);

            if (parameterInfos.Length > 0)
            {
                key.Append('(');
                key.AppendJoin(',', parameterInfos.Select(x => x.ParameterType.GetXmlName(true, typeGenericMap, methodGenericMap)));
                key.Append(')');
            }

            if (methodInfo.Name == "op_Implicit" ||
                methodInfo.Name == "op_Explicit")
            {
                key.Append("~");
                key.Append(methodInfo.ReturnType.GetXmlName(true, typeGenericMap, methodGenericMap));
            }

            return key.ToString();
        }

        public static string GetKey(this ConstructorInfo constructorInfo)
        {
            var typeGenericMap = constructorInfo.DeclaringType.GetGenericArguments().ToTypeArgumentDictionary();

            using var _ = StringBuilderPool.Rent(out var key);

            var parameterInfos = constructorInfo.GetParameters();

            key.Append("M:");
            key.Append(constructorInfo.DeclaringType.GetXmlName(false, typeGenericMap));
            key.Append(".#ctor");

            if (parameterInfos.Length > 0)
            {
                key.Append('(');
                key.AppendJoin(',', parameterInfos.Select(x => x.ParameterType.GetXmlName(true, typeGenericMap)));
                key.Append(')');
            }

            return key.ToString();
        }

        private static string GetXmlName(
            this Type type,
            bool isMethodParameter,
            IDictionary<string, int> typeGenericMap,
            IDictionary<string, int>? methodGenericMap = null)
        {
            using var _ = StringBuilderPool.Rent(out var str);

            if (type.IsGenericParameter)
            {
                if (methodGenericMap != null && methodGenericMap.TryGetValue(type.Name, out int methodIndex))
                    str.Append("``").Append(methodIndex);
                else
                    str.Append('`').Append(typeGenericMap[type.Name]);
            }
            else if (type.HasElementType)
            {
                string elementTypeString = type.GetElementType().GetXmlName(
                    isMethodParameter,
                    typeGenericMap,
                    methodGenericMap);

                str.Append(elementTypeString);

                if (type.IsPointer)
                {
                    str.Append('*');
                }
                else if (type.IsArray)
                {
                    int rank = type.GetArrayRank();

                    str.Append('[');
                    if (rank > 1)
                        str.AppendJoin(',', Enumerable.Repeat("0:", rank));
                    str.Append(']');
                }
                else if (type.IsByRef)
                {
                    str.Append('@');
                }
                else
                {
                    throw new Exception("Unknown element type case");
                }
            }
            else
            {
                if (type.IsNested)
                    str.Append(type.DeclaringType.GetXmlName(isMethodParameter, typeGenericMap, methodGenericMap));
                else
                    str.Append(type.Namespace);

                str.Append('.');

                if (isMethodParameter)
                    str.Append(Regex.Replace(type.Name, @"`\d+", string.Empty));
                else
                    str.Append(type.Name);

                if (type.IsGenericType && isMethodParameter)
                {
                    str.Append('{');
                    str.AppendJoin(',', type.GetGenericArguments().Select(argument
                        => argument.GetXmlName(isMethodParameter, typeGenericMap, methodGenericMap)));
                    str.Append('}');
                }
            }

            return str.ToString();
        }

        private static Dictionary<string, int> ToTypeArgumentDictionary(this Type[] types)
        {
            int i = 0;
            return types.ToDictionary(o => o.Name, _ => i++);
        }

        private static string XmlDocumentationKeyHelper(string typeFullNameString, string? memberNameString = null)
        {
            string key = Regex.Replace(typeFullNameString, @"\[.*\]", string.Empty).Replace('+', '.');

            if (memberNameString != null)
                key += "." + memberNameString;

            return key;
        }
    }
}
