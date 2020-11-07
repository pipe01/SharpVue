using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace SharpVue.Generator.Json
{
    [DebuggerDisplay("Namespace {FullName}")]
    public class Namespace
    {
        [JsonPropertyName("fullName")]
        public string? FullName { get; set; }

        [JsonPropertyName("types")]
        public List<TypeJson> Types { get; } = new List<TypeJson>();
    }
}
