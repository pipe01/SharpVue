using System;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SharpVue.Loading
{
    public class Config
    {
        public class AppearanceConfig
        {
            /// <summary>
            /// The name that will be used when referring to the application in the documentation.
            /// </summary>
            public string AppName { get; set; } = "sharpvue";

            public bool DarkMode { get; set; } = true;
        }

        /// <summary>
        /// Globs that resolve to assemblies that will be scanned for reference.
        /// </summary>
        public string[] Assemblies { get; set; } = Array.Empty<string>();

        /// <summary>
        /// Globs that resolve to assemblies that are needed to load the reference assemblies.
        /// </summary>
        public string[] Dependencies { get; set; } = Array.Empty<string>();

        /// <summary>
        /// Folders that will be scanned for Markdown articles.
        /// </summary>
        public string[] Articles { get; set; } = new[] { "articles/" };
        public string OutFolder { get; set; } = "dist";

        public AppearanceConfig Appearance { get; set; } = new AppearanceConfig();

        public static Config Load(string file)
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .Build();

            return deserializer.Deserialize<Config>(File.ReadAllText(file));
        }
    }
}
