using System;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SharpVue.Loading
{
    public class Config
    {
        public string[] Assemblies { get; set; } = Array.Empty<string>();
        public string[] Articles { get; set; } = new[] { "articles/**/*.md" };
        public string OutFolder { get; set; } = "dist";

        public static Config Load(string file)
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .Build();

            return deserializer.Deserialize<Config>(File.ReadAllText(file));
        }
    }
}
