using SharpVue.Common;
using SharpVue.Common.Documentation;
using SharpVue.Logging;
using System;
using System.IO;
using System.Reflection;

namespace SharpVue.Loading
{
    public sealed class Workspace : IDisposable
    {
        public const string ConfigName = "sharpvue.yaml";

        public string BaseFolder { get; }
        public Config Config { get; }

        public AssemblyLoader AssemblyLoader { get; }
        public ArticleLoader ArticleLoader { get; }

        public Workspace(string configPath)
        {
            Logger.Verbose("Loading workspace at {0}", configPath);

            this.BaseFolder = Path.GetDirectoryName(configPath);
            this.Config = Config.Load(configPath);

            this.AssemblyLoader = new AssemblyLoader(Config, BaseFolder);
            this.ArticleLoader = new ArticleLoader(Config, BaseFolder);
        }

        public MemberData? GetDataFor(MemberInfo member)
            => AssemblyLoader.ReferenceData.TryGetValue(member.GetKey(), out var data) ? data : null;

        /// <returns>The full path to the output folder.</returns>
        public string PrepareOutputFolder()
        {
            string outFolder = Path.Combine(BaseFolder, Config.OutFolder);

            if (Directory.Exists(outFolder) && Config.ClearOutputFolderOnGen)
                Directory.Delete(outFolder, true);

            Directory.CreateDirectory(outFolder);

            return Path.GetFullPath(outFolder);
        }

        public void Dispose()
        {
            this.AssemblyLoader.Dispose();
        }
    }
}
