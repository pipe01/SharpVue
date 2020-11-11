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
        public IWatcher Watcher { get; }

        public AssemblyLoader AssemblyLoader { get; }
        public ArticleLoader ArticleLoader { get; }

        public string OutFolder { get; set; }

        public Workspace(string configPath)
        {
            Logger.Verbose("Loading workspace at {0}", configPath);

            this.BaseFolder = Path.GetFullPath(Path.GetDirectoryName(configPath));
            this.Config = Config.Load(configPath);
            this.OutFolder = Path.Combine(BaseFolder, Config.OutFolder);
            this.Watcher = new Watcher(this);

            this.AssemblyLoader = new AssemblyLoader(Config, BaseFolder);
            this.ArticleLoader = new ArticleLoader(Config, BaseFolder);
        }

        public MemberData? GetDataFor(MemberInfo member)
            => AssemblyLoader.ReferenceData.TryGetValue(member.GetKey(), out var data) ? data : null;

        /// <returns>The full path to the output folder.</returns>
        public string PrepareOutputFolder(bool forceClean = false)
        {
            if (Directory.Exists(OutFolder) && (forceClean || Config.ClearOutputFolderOnGen))
                Directory.Delete(OutFolder, true);

            Directory.CreateDirectory(OutFolder);

            return Path.GetFullPath(OutFolder);
        }

        public void Reload(bool reloadArticles = true, bool reloadReference = true)
        {
            if (reloadArticles)
                ArticleLoader.Reload();

            if (reloadReference)
                AssemblyLoader.Reload();
        }

        public void Dispose()
        {
            this.AssemblyLoader.Dispose();
        }
    }
}
