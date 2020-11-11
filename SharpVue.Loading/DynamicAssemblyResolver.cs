using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace SharpVue.Loading
{
    public class DynamicAssemblyResolver : MetadataAssemblyResolver
    {
        private static readonly string[] RuntimeAssemblies;

        private readonly IDictionary<string, IList<string>> FileToPaths = new Dictionary<string, IList<string>>();

        static DynamicAssemblyResolver()
        {
            RuntimeAssemblies = Directory.GetFiles(RuntimeEnvironment.GetRuntimeDirectory(), "*.dll");
        }

        public DynamicAssemblyResolver()
        {
            AddRange(RuntimeAssemblies);
        }

        public void Add(string filePath)
        {
            var file = Path.GetFileNameWithoutExtension(filePath);

            if (!FileToPaths.TryGetValue(file, out var paths))
            {
                FileToPaths.Add(file, paths = new List<string>());
            }

            paths.Add(filePath);
        }

        public void AddRange(IEnumerable<string> paths)
        {
            foreach (var path in paths)
            {
                Add(path);
            }
        }

        public void Clear()
        {
            FileToPaths.Clear();
            AddRange(RuntimeAssemblies);
        }

        public override Assembly? Resolve(MetadataLoadContext context, AssemblyName assemblyName)
        {
            // Taken from https://github.com/dotnet/runtime/blob/a79df14b3cc62ade39382a6e08d3b25871d8ebb6/src/libraries/System.Reflection.MetadataLoadContext/src/System/Reflection/PathAssemblyResolver.cs

            Debug.Assert(assemblyName.Name != null);

            Assembly? candidateWithSamePkt = null;
            Assembly? candidateIgnoringPkt = null;

            if (FileToPaths.TryGetValue(assemblyName.Name, out var paths))
            {
                ReadOnlySpan<byte> pktFromName = assemblyName.GetPublicKeyToken();

                foreach (string path in paths)
                {
                    Assembly assemblyFromPath = context.LoadFromAssemblyPath(path);
                    AssemblyName assemblyNameFromPath = assemblyFromPath.GetName();

                    if (assemblyName.Name.Equals(assemblyNameFromPath.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        ReadOnlySpan<byte> pktFromAssembly = assemblyNameFromPath.GetPublicKeyToken();

                        // Find exact match on PublicKeyToken including treating no PublicKeyToken as its own entry.
                        if (pktFromName.SequenceEqual(pktFromAssembly))
                        {
                            // Pick the highest version.
                            if (candidateWithSamePkt == null || assemblyNameFromPath.Version > candidateWithSamePkt.GetName().Version)
                            {
                                candidateWithSamePkt = assemblyFromPath;
                            }
                        }
                        // If assemblyName does not specify a PublicKeyToken, or assemblyName is marked 'Retargetable',
                        // then still consider those with a PublicKeyToken and take the highest version available.
                        else if ((candidateWithSamePkt == null && pktFromName.IsEmpty) ||
                            ((assemblyName.Flags & AssemblyNameFlags.Retargetable) != 0))
                        {
                            // Pick the highest version.
                            if (candidateIgnoringPkt == null || assemblyNameFromPath.Version > candidateIgnoringPkt.GetName().Version)
                            {
                                candidateIgnoringPkt = assemblyFromPath;
                            }
                        }
                    }
                }
            }

            return candidateWithSamePkt ?? candidateIgnoringPkt;
        }
    }
}
