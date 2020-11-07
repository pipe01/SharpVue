using SharpVue.Common.Documentation;

namespace SharpVue.Generator.Json
{
    public abstract class Descriptable
    {
        public Content? Summary { get; set; }
        public Content? Remarks { get; set; }
    }
}
