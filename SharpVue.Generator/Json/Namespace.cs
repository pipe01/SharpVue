using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SharpVue.Generator.Json
{
    public class Namespace
    {
        [JsonPropertyName("types")]
        public List<TypeJson> Types { get; } = new List<TypeJson>();

        [JsonPropertyName("children")]
        public List<Namespace> Children { get; } = new List<Namespace>();
    }
}
