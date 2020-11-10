using SharpVue.Common;
using SharpVue.Loading;
using System.Collections.Generic;

namespace SharpVue.Generator.Json
{
    public class JsonData
    {
        public IList<Namespace> Namespaces { get; set; } = new List<Namespace>();
        public IList<Article>? Articles { get; set; }
        public Config.AppearanceConfig? Config { get; set; }
    }
}
