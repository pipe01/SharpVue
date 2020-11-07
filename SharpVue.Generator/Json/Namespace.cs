using System.Collections.Generic;
using System.Diagnostics;

namespace SharpVue.Generator.Json
{
    [DebuggerDisplay("Namespace {FullName}")]
    public class Namespace
    {
        public string? FullName { get; set; }
        public List<TypeJson> Types { get; } = new List<TypeJson>();
    }
}
