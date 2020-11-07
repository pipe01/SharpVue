using System;
using System.Collections.Generic;

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
    }
}
