using GlobExpressions;
using SharpVue.Common;
using SharpVue.Common.Documentation;
using SharpVue.Ingest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml;

namespace SharpVue.Loading
{
    public sealed class Workspace : IDisposable
    {
        public const string ConfigName = "sharpvue.yaml";

        public IList<Article> Articles { get; } = new List<Article>();
        public IList<(Type Type, MemberData Data)> ReferenceTypes { get; } = new List<(Type Type, MemberData Data)>();

        public string BaseFolder { get; }
        public Config Config { get; }

        private readonly Ingestion Ingestion;
        private readonly MetadataLoadContext LoadContext;
        private readonly IList<string> ContextAssemblies = new List<string>();

        public Workspace(string configPath)
        {
            string[] runtimeAssemblies = Directory.GetFiles(RuntimeEnvironment.GetRuntimeDirectory(), "*.dll");

            this.BaseFolder = Path.GetDirectoryName(configPath);
            this.Config = Config.Load(configPath);
            this.Ingestion = new CSharpXmlIngestion();
            this.LoadContext = new MetadataLoadContext(new PathAssemblyResolver(ContextAssemblies.Concat(runtimeAssemblies)));

            LoadTypes();
        }

        private void LoadTypes()
        {
            var root = new DirectoryInfo(BaseFolder);

            foreach (var path in Config.Assemblies)
            {
                foreach (var file in root.GlobFiles(path))
                {
                    LoadAssembly(file.FullName);
                }
            }
        }

        private void LoadAssembly(string dllPath)
        {
            ContextAssemblies.Add(dllPath);

            var ass = LoadContext.LoadFromAssemblyPath(dllPath);

            var xmlFile = new FileInfo(Path.ChangeExtension(dllPath, "xml"));
            if (xmlFile.Exists)
            {
                var doc = new XmlDocument();
                doc.Load(xmlFile.FullName);

                Ingestion.Load(doc["doc"]["members"]);
            }

            foreach (var type in ass.ExportedTypes)
            {
                var data = Ingestion.GetDataFor(type) ?? new MemberData("T:" + type.FullName);

                ReferenceTypes.Add((type, data));
            }
        }

        public void Dispose()
        {
            this.LoadContext.Dispose();
        }
    }
}
