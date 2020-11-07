using SharpVue.Common.Documentation;

namespace SharpVue.Generator.Json
{
    public abstract class Returner : Descriptable
    {
        public Content? ReturnType { get; set; }
        public Content? Returns { get; set; }
    }
}
