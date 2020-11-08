using System.Collections.Generic;

namespace SharpVue.Generator.Json
{
    public class JsonData
    {
        public IList<Namespace> Namespaces { get; } = new List<Namespace>();
    }
}
