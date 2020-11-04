using NUnit.Framework;
using SharpVue.Common;

namespace SharpVue.Test.Common
{
    public class MemberNameTest
    {
        [TestCase("T:Example.ExampleClass", MemberType.Type, "Example", "ExampleClass")]
        [TestCase("M:Example.ExampleClass.ExampleMethod1", MemberType.Method, "Example.ExampleClass", "ExampleMethod1")]
        [TestCase("T:Example.ExampleClass.ExampleNestedGenericClass`3", MemberType.Type, "Example.ExampleClass", "ExampleNestedGenericClass")]
        [TestCase("M:Example.ExampleClass.ExampleNestedGenericClass`3.ExampleMethod2``3(`0,``0,`1[],``1[],`2[0:,0:,0:],``2[0:,0:,0:])", MemberType.Method, "Example.ExampleClass.ExampleNestedGenericClass`3", "ExampleMethod2")]
        public void Parse(string fullName, MemberType type, string ns, string name)
        {
            var member = MemberName.Parse(fullName);

            Assert.AreEqual(type, member.Type);
            Assert.AreEqual(ns, member.Namespace);
            Assert.AreEqual(name, member.Name);
        }
    }
}