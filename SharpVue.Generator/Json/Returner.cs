using SharpVue.Common.Documentation;
using System;
using System.Reflection;

namespace SharpVue.Generator.Json
{
    public abstract class Returner : Member
    {
        public Content ReturnType { get; }
        public Content? Returns { get; }

        public Returner(MemberData? data, string name, MemberInfo member, Type declaringType, Type returnType) : base(data, name, member, declaringType)
        {
            this.Returns = data?.Returns;
            this.ReturnType = returnType.GenerateNameContent();
        }
    }
}
