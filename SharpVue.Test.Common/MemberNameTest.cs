using NUnit.Framework;
using SharpVue.Common.Documentation;

namespace SharpVue.Test.Common
{
    public class MemberNameTest
    {
        [TestCase("T:Example.ExampleClass", MemberKind.Type, "Example", "ExampleClass")]
        [TestCase("M:Example.ExampleClass.ExampleMethod1", MemberKind.Method, "Example.ExampleClass", "ExampleMethod1")]
        [TestCase("T:Example.ExampleClass.ExampleNestedGenericClass`3", MemberKind.Type, "Example.ExampleClass", "ExampleNestedGenericClass")]
        [TestCase("M:Example.ExampleClass.ExampleNestedGenericClass`3.ExampleMethod2``3(`0,``0,`1[],``1[],`2[0:,0:,0:],``2[0:,0:,0:])", MemberKind.Method, "Example.ExampleClass.ExampleNestedGenericClass`3", "ExampleMethod2")]
        public void Parse(string fullName, MemberKind type, string ns, string name)
        {
            var member = MemberName.Parse(fullName);

            Assert.AreEqual(type, member.Kind);
            Assert.AreEqual(ns, member.Root);
            Assert.AreEqual(name, member.Name);
        }
    }
}