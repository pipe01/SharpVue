﻿using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SharpVue.Generator.Json
{
    public class TypeJson
    {
        [JsonPropertyName("fullname")]
        public string? Fullname { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("namespace")]
        public string? Namespace { get; set; }

        [JsonPropertyName("properties")]
        public List<Property> Properties { get; set; } = new List<Property>();
    }
}
