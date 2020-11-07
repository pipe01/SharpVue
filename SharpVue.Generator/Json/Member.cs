using SharpVue.Common.Documentation;
using System;
using System.Reflection;

namespace SharpVue.Generator.Json
{
    public abstract class Member : Descriptable
    {
        public string? InheritedFrom { get; }

        protected Member(MemberData? data, string name, MemberInfo member, Type declaringType) : base(data, name)
        {
            if (member.DeclaringType != declaringType)
                InheritedFrom = member.DeclaringType?.FullName;
        }
    }
}
