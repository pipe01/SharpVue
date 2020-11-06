using SharpVue.Common.Documentation;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Xml;

namespace SharpVue.Ingest
{
    public abstract class Ingestion
    {
        private readonly IDictionary<string, MemberData> LoadedMembers = new Dictionary<string, MemberData>();

        public void Load(XmlNode xml)
        {
            foreach (var member in Load(new XmlNodeReader(xml)))
            {
                LoadedMembers[member.Name.FullName] = member;

                if (LoadedMembers.TryGetValue(member.Name.Root, out var parent))
                {
                    member.Parent = parent;
                    parent.Children.Add(member);
                }
            }
        }

        protected abstract IEnumerable<MemberData> Load(XmlNodeReader reader);

        public MemberData? GetDataFor(Type type)
            => TryGetDataFor(type, out var d) ? d : null;

        public bool TryGetDataFor(Type type, [MaybeNullWhen(false)] out MemberData data)
            => LoadedMembers.TryGetValue(type.FullName, out data);
    }
}
