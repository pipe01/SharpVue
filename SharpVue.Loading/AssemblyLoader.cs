using GlobExpressions;
using SharpVue.Common.Documentation;
using SharpVue.Ingest;
using SharpVue.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml;

namespace SharpVue.Loading
{
    public class AssemblyLoader : IDisposable, ILoader
    {
        public IList<Type> ReferenceTypes { get; } = new List<Type>();
        public IDictionary<string, MemberData> ReferenceData { get; } = new Dictionary<string, MemberData>();

        private readonly Config Config;
        private readonly string BaseFolder;

        private readonly Ingestion Ingestion;

        private readonly DynamicAssemblyResolver AssemblyResolver;
        private MetadataLoadContext? LoadContext;

        public AssemblyLoader(Config config, string baseFolder)
        {
            this.Config = config;
            this.BaseFolder = baseFolder;
            this.Ingestion = new CSharpXmlIngestion();
            this.AssemblyResolver = new DynamicAssemblyResolver();

            Reload();
        }

        public void Reload()
        {
            ReferenceTypes.Clear();
            ReferenceData.Clear();
            AssemblyResolver.Clear();
            LoadContext?.Dispose();

            LoadContext = new MetadataLoadContext(AssemblyResolver);

            AddContextAssemblies();

            LoadAllAssemblies();
        }

        private void AddContextAssemblies()
        {
            Logger.Debug("Loading context assemblies");

            var root = new DirectoryInfo(BaseFolder);

            foreach (var glob in Config.Dependencies)
            {
                foreach (var file in root.GlobFiles(glob))
                {
                    Logger.Verbose("Adding dependency assembly at {0}", file);

                    AssemblyResolver.Add(file.FullName);
                }
            }

            foreach (var glob in Config.Assemblies)
            {
                foreach (var file in root.GlobFiles(glob))
                {
                    Logger.Verbose("Adding reference assembly at {0}", file);

                    AssemblyResolver.Add(file.FullName);
                }
            }
        }

        private void LoadAllAssemblies()
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
            Debug.Assert(LoadContext != null);

            Logger.Verbose("Loading assembly at {0}", dllPath);

            var ass = LoadContext.LoadFromAssemblyPath(dllPath);

            var xmlFile = new FileInfo(Path.ChangeExtension(dllPath, "xml"));
            if (xmlFile.Exists)
            {
                LoadXml(xmlFile.FullName);
            }

            foreach (var type in ass.ExportedTypes)
            {
                ReferenceTypes.Add(type);
            }

            void LoadXml(string path)
            {
                var doc = new XmlDocument();
                doc.Load(path);

                var membersNode = doc["doc"]["members"];
                if (membersNode == null)
                    throw new Exception("Invalid XML documentation file");

                using var reader = new XmlNodeReader(membersNode);

                foreach (var data in Ingestion.Load(reader))
                {
                    ReferenceData[data.Name.XmlKey] = data;
                }
            }
        }

        public void Dispose()
        {
            Debug.Assert(LoadContext != null);

            this.LoadContext.Dispose();
        }
    }
}
