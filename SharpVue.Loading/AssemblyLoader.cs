using GlobExpressions;
using SharpVue.Common.Documentation;
using SharpVue.Ingest;
using SharpVue.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml;

namespace SharpVue.Loading
{
    public class AssemblyLoader : IDisposable
    {
        public IList<Type> ReferenceTypes { get; } = new List<Type>();
        public IDictionary<string, MemberData> ReferenceData { get; } = new Dictionary<string, MemberData>();

        private readonly Config Config;
        private readonly string BaseFolder;

        private readonly Ingestion Ingestion;
        private readonly MetadataLoadContext LoadContext;
        private readonly HashSet<string> ContextAssemblies;

        public AssemblyLoader(Config config, string baseFolder)
        {
            this.Config = config;
            this.BaseFolder = baseFolder;

            string[] runtimeAssemblies = Directory.GetFiles(RuntimeEnvironment.GetRuntimeDirectory(), "*.dll");
            this.ContextAssemblies = new HashSet<string>(runtimeAssemblies);
            Logger.Debug("Found {0} runtime assemblies", runtimeAssemblies.Length);

            AddAssemblies();

            this.Ingestion = new CSharpXmlIngestion();
            this.LoadContext = new MetadataLoadContext(new PathAssemblyResolver(ContextAssemblies));

            LoadTypes();
        }

        private void AddAssemblies()
        {
            Logger.Debug("Loading context assemblies");

            var root = new DirectoryInfo(BaseFolder);

            foreach (var glob in Config.Dependencies)
            {
                foreach (var file in root.GlobFiles(glob))
                {
                    Logger.Verbose("Adding dependency assembly at {0}", file);

                    ContextAssemblies.Add(file.FullName);
                }
            }

            foreach (var glob in Config.Assemblies)
            {
                foreach (var file in root.GlobFiles(glob))
                {
                    Logger.Verbose("Adding reference assembly at {0}", file);

                    ContextAssemblies.Add(file.FullName);
                }
            }
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
            this.LoadContext.Dispose();
        }
    }
}
