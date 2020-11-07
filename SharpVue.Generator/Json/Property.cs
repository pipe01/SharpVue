﻿using System.Text.Json.Serialization;

namespace SharpVue.Generator.Json
{
    public class Property
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("returnType")]
        public string? ReturnType { get; set; }

        [JsonPropertyName("getter")]
        public bool Getter { get; set; }

        [JsonPropertyName("setter")]
        public bool Setter { get; set; }

        [JsonPropertyName("summary")]
        public string? Summary { get; set; }

        [JsonPropertyName("remarks")]
        public string? Remarks { get; set; }

        [JsonPropertyName("returns")]
        public string? Returns { get; set; }
    }
}
